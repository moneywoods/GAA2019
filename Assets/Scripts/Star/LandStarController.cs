using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandStarController : StarBase
{
    public enum ChildIndex
    {
        NeighvorFinder,
        ENUM_MAX
    }

    [Flags]
    public enum LANDSTAR_STAT // たぶん32bitだから大丈夫(?)
    {
        NEUTRAL                = 0 << 0, // 0000_0000_0000 // 何もない状態.
        // WAITING             = 1 << 0, // 0000_0000_0001 // 待機状態. これややこしくしてるだけかも.
        MOVING_RIGHT           = 1 << 1, // 0000_0000_0010 // プレイヤーの周りの星を右回転.
        MOVING_LEFT            = 1 << 2, // 0000_0000_0100 // プレイヤーの周りの星を左回転.
                                         // わからんけど開けておく下位4bitが移動待機を司る.
        PLAYER_STAYING         = 1 << 4, // 0000_0001_0000 // プレイヤーが滞在中.
        IN_MILKYWAY_AREA       = 1 << 5, // 0000_0010_0000 // 乳の領内に侵入中.
        GET_CAUGHT_BY_MILKYWAY = 1 << 6, // 0000_0100_0000 // 乳に飲まれて動けない.
        ALIVE                  = 1 << 7, // 0000_1000_0000 // 生きてる.( == キネティックパワーを受ける状態)
        DESTROYED              = 1 << 8, // 0001_0000_0000  // 破壊された.
        // フラグ抽出用
        MOVING                 = 6,      // 0000_0000_0110 // MOVING_LEFT | MOVING_RIGHT
        ENUM_MAX
    }

    public LANDSTAR_STAT CurrentStat { get; private set; }

    private Vector3 centerOfCircular;

    public GameObject explosionObject; // 自身にDESTROYEDフラグが立った時生成するエフェクトオブジェクト

    // 回すとき用
    float timeToCirculate; // 今回の回転に要する時間. 単位: 秒.
    float timePast;  // 回転している時間の累計(回転状態を解除されるたびにリセット)

    // 移住可能を示すエフェクト // 今後UIとかもっと他の物に置き換える予定
    public GameObject m_EffectCanMoveTo;
    private bool m_isCanMoveToEffectEmitting;

    public LandStarController() : base(StarType.Land)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        timePast = 0.0f;
        m_isCanMoveToEffectEmitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        // 回転
        if((CheckFlag(LANDSTAR_STAT.MOVING) && timeToCirculate != 0.0f))
        {
            float time = Time.deltaTime;

            if(timeToCirculate < Time.deltaTime + timePast) // 90度以上回らないように
            {
                time = timeToCirculate - timePast;
            }

            float degree = 0.0f;

            if(CheckFlag(LANDSTAR_STAT.MOVING_RIGHT)) // 左回りに移動中
            {
                degree = 90.0f / timeToCirculate * time;
            }
            else if(CheckFlag(LANDSTAR_STAT.MOVING_LEFT)) // 右回りに移動中
            {
                degree = -90.0f / timeToCirculate * time;
            }
            transform.RotateAround(centerOfCircular, new Vector3(0.0f, 1.0f, 0.0f), degree);

            timePast += time;

            if(timeToCirculate <= timePast)
            {
                timePast = 0.0f;
                timeToCirculate = 0.0f;
                if(CheckFlag(LANDSTAR_STAT.IN_MILKYWAY_AREA))
                {
                    AddStat(LANDSTAR_STAT.GET_CAUGHT_BY_MILKYWAY);
                }

                RemoveFlag(LANDSTAR_STAT.MOVING);
            }
        }

        if(CheckFlag(LANDSTAR_STAT.DESTROYED))
        {
            // 爆発エフェクト生成.
            Instantiate(explosionObject, transform.position, transform.rotation);
            var i = StarMaker.Instance.GetCellColliderBehavior(new Vector2Int(3, 3));
            StarMaker.Instance.GetCellColliderBehavior(CellNum).RemoveManually(gameObject);
            Destroy(gameObject);
        }
    }

    // --------------------------------------------------------------------------------------------
    //
    // public 操作関数
    //
    // --------------------------------------------------------------------------------------------
    public void SetMove(GameObject center, float estimatedTimeToCirculate, bool isRight)
    {
        if(CheckFlag(LANDSTAR_STAT.MOVING_RIGHT) || CheckFlag(LANDSTAR_STAT.MOVING_LEFT) || CheckFlag(LANDSTAR_STAT.GET_CAUGHT_BY_MILKYWAY) || CheckFlag(LANDSTAR_STAT.DESTROYED))
        {
            return; // 既に移動状態であるなら実行しない. 乳に飲まれている場合も実行しないゾ.
        }
        if(isRight)
        {
            AddStat(LANDSTAR_STAT.MOVING_RIGHT);

        }
        else
        {
            AddStat(LANDSTAR_STAT.MOVING_LEFT);
        }
        centerOfCircular = center.transform.position;
        timeToCirculate = estimatedTimeToCirculate;
    }

    public void ArriveThisLand(GameObject Character) // 自身にSTAYINGフラグを立て,引数のcurrentStarStayingをこのオブジェクトにする.
    {
        if(Character.tag == ObjectTag.PlayerCharacter)
        {
            Character.GetComponent<Tako.TakoController>().SetCurrentStarStaying(gameObject);
            AddStat(LANDSTAR_STAT.PLAYER_STAYING);
        }
    }

    public bool LeaveThisLand() // 自身にSTAYINGフラグを解除する.
    {
        if(!CheckFlag(LANDSTAR_STAT.PLAYER_STAYING))
        {
            return false;
        }
        else
        {
            // null
        }

        RemoveFlag(LANDSTAR_STAT.PLAYER_STAYING);
        return true;
    }


    public override bool CheckKineticPowerCanBeUsed(Vector2Int originCellNum, bool isRight)
    {
        var direction = StarMaker.GetDirection(originCellNum, CellNum);

        Vector2Int cp0 = CellNum;
        Vector2Int cp1 = CellNum;

        // チェックするコマを算出.
        if(isRight)
        {
            if(direction == Direction.Right)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Bottom);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.LeftBottom);
            }
            else if(direction == Direction.RightTop)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Bottom);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Bottom);
            }
            else if(direction == Direction.Top)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Right);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.RightBottom);
            }
            else if(direction == Direction.LeftTop)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Right);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Right);
            }
            else if(direction == Direction.Left)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Top);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.RightTop);
            }
            else if(direction == Direction.LeftBottom)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Top);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Top);
            }
            else if(direction == Direction.Bottom)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Left);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.LeftTop);
            }
            else if(direction == Direction.RightBottom)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Left);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Left);
            }
            else
            {
                return false; // この条件に正であることはありえない.
            }
        }
        else
        {
            if(direction == Direction.Right)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Top);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.LeftTop);
            }
            else if(direction == Direction.RightTop)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Left);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Left);
            }
            else if(direction == Direction.Top)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Left);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.LeftBottom);
            }
            else if(direction == Direction.LeftTop)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Bottom);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Bottom);
            }
            else if(direction == Direction.Left)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Bottom);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.RightBottom);
            }
            else if(direction == Direction.LeftBottom)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Right);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Right);
            }
            else if(direction == Direction.Bottom)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Right);
                cp1 += StarMaker.GetDifferenceByDirection(Direction.RightTop);
            }
            else if(direction == Direction.RightBottom)
            {
                cp0 += StarMaker.GetDifferenceByDirection(Direction.Top);
                cp1 = cp0 + StarMaker.GetDifferenceByDirection(Direction.Top);
            }
            else
            {
                return false; // この条件に正であることはありえない.
            }
        }

        // マップ領域内かチェック
        if(!(StarMaker.Instance.CheckLimitOfMap(cp0) || StarMaker.Instance.CheckLimitOfMap(cp1)))
        {
            return false;
        }
        else
        {
            // null
        }

        if(0 < StarMaker.Instance.GetStarList(cp0, StarType.Rock).Count ||
            0 < StarMaker.Instance.GetStarList(cp0, StarType.Rock).Count)
        {
            return false;
        }
        else
        {
            // null
        }

        return true;
    }
    // --------------------------------------------------------------------------------------------
    //
    // public フラグ関連
    //
    // --------------------------------------------------------------------------------------------
    public void SetStat(LANDSTAR_STAT newStat) // フラグ用変数に引数を代入.
    {
        CurrentStat = newStat;
    }
    public bool AddStat(LANDSTAR_STAT additionalStat) // フラグを立てる. 引数のフラグが既に立っている場合trueを戻し終了.
    {
        if((CurrentStat & additionalStat) != 0)
        {
            return true; // 引数のフラグが既に立っている場合trueを返す.
        }

        CurrentStat |= additionalStat;

        // フラグ別の追加処理
        if(additionalStat == LANDSTAR_STAT.DESTROYED)
        {
            RemoveFlag(LANDSTAR_STAT.ALIVE);
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if(additionalStat == LANDSTAR_STAT.ALIVE)
        {
            RemoveFlag(LANDSTAR_STAT.DESTROYED);
            GetComponent<Renderer>().material.color = Color.white;
        }
        else if(additionalStat == LANDSTAR_STAT.GET_CAUGHT_BY_MILKYWAY)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        return false;
    }

    public void RemoveFlag(LANDSTAR_STAT removingFlag)
    {
        CurrentStat &= ~removingFlag;

        // フラグ別の追加処理
        if(removingFlag == LANDSTAR_STAT.DESTROYED)
        {
            AddStat(LANDSTAR_STAT.ALIVE);
            GetComponent<Renderer>().material.color = Color.white;
        }
        else if(removingFlag == LANDSTAR_STAT.ALIVE)
        {
            AddStat(LANDSTAR_STAT.DESTROYED);
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if(removingFlag == LANDSTAR_STAT.GET_CAUGHT_BY_MILKYWAY)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    // フラグチェック
    public bool CheckFlag(LANDSTAR_STAT flag)
    {
        if((CurrentStat & flag) != 0)
        {
            return true;
        }
        return false;
    }
}
