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
        CAUGHT_BY_MILKYWAY = 1 << 6, // 0000_0100_0000 // 乳に飲まれて動けない.
        ALIVE                  = 1 << 8, // 0001_0000_0000  // 破壊された.
        // フラグ抽出用
        MOVING                 = 6,      // 0000_0000_0110 // MOVING_LEFT | MOVING_RIGHT
        STUCKED                = CAUGHT_BY_MILKYWAY, 
        ENUM_MAX
    }

    public LANDSTAR_STAT CurrentStat { get; protected set; }
    
    public Vector3 centerOfCircular
    {
        get;
        protected set;
    }

    [SerializeField]private GameObject explosionObject; // 自身にDESTROYEDフラグが立った時生成するエフェクトオブジェクト

    // 回すとき用
    public float timeToCirculate // 今回の回転に要する時間. 単位: 秒.
    {
        get;
        set;
    }
    public float timePast // 回転している時間の累計(回転状態を解除されるたびにリセット)
    {
        get;
        set;
    }
    
    
    public GameObject uitext;   // テキストのスクリプト取得
    private int textchange;     // テキストの表示フラグ

    // 移住可能を示すエフェクト // 今後UIとかもっと他の物に置き換える予定
    public GameObject m_EffectCanMoveTo;
    protected bool m_isCanMoveToEffectEmitting;

    // 移住可能を示すエフェクト // 今後UIとかもっと他の物に置き換える予定
    public GameObject m_EffectCanMoveTo;
    protected bool m_isCanMoveToEffectEmitting;

    public LandStarController() : base(StarType.Land)
    {
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        uitext = GameObject.FindWithTag(ObjectTag.MessageText);
        timePast = 0.0f;
        m_isCanMoveToEffectEmitting = false;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        if (CheckFlag(LANDSTAR_STAT.MOVING) && !CheckFlag(LANDSTAR_STAT.IN_MILKYWAY_AREA)) // IN_MILKYWAY_AREA時はMW側が動かします。
        {
            float time = Time.deltaTime;

            if (timeToCirculate < Time.deltaTime + timePast) // 90度以上回らないように
            {
                time = timeToCirculate - timePast;
            }

            float degree = 0.0f;

            if (CheckFlag(LANDSTAR_STAT.MOVING_RIGHT)) // 左回りに移動中
            {
                degree = 90.0f / timeToCirculate * time;
            }
            else if (CheckFlag(LANDSTAR_STAT.MOVING_LEFT)) // 右回りに移動中
            {
                degree = -90.0f / timeToCirculate * time;
            }
            transform.RotateAround(centerOfCircular, new Vector3(0.0f, 1.0f, 0.0f), degree);

            timePast += time;

            if (timeToCirculate <= timePast)
            {
                timePast = 0.0f;
                timeToCirculate = 0.0f;
                //if(CheckFlag(LANDSTAR_STAT.IN_MILKYWAY_AREA))
                //{
                //    AddStat(LANDSTAR_STAT.GET_CAUGHT_BY_MILKYWAY);
                //}

                RemoveFlag(LANDSTAR_STAT.MOVING);
            }
        }

        if (!CheckFlag(LANDSTAR_STAT.ALIVE))
        {
            // 爆発エフェクト生成.
            Instantiate(explosionObject, transform.position, transform.rotation);
            var i = StarMaker.Instance.GetCellColliderBehavior(new Vector2Int(3, 3));
            StarMaker.Instance.GetCellColliderBehavior(CellNum).RemoveManually(gameObject);
            textchange = 2;
            uitext.GetComponent<TextMessnger>().Textflag = textchange;
            Destroy(gameObject);
        }
        textchange = uitext.GetComponent<TextMessnger>().Textflag;
    }

    // --------------------------------------------------------------------------------------------
    //
    // public 操作関数
    //
    // --------------------------------------------------------------------------------------------
    public void SetMove(GameObject center, float estimatedTimeToCirculate, bool isRight)
    {
        if(CheckFlag(LANDSTAR_STAT.MOVING_RIGHT) || CheckFlag(LANDSTAR_STAT.MOVING_LEFT) || CheckFlag(LANDSTAR_STAT.CAUGHT_BY_MILKYWAY) || !CheckFlag(LANDSTAR_STAT.ALIVE))
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
        // スタック中は動けないのでtrueを戻す
        if(CheckFlag(LANDSTAR_STAT.STUCKED))
        {
            return true;
        }
        var starMaker = StarMaker.Instance;
        var direction = StarMaker.GetDirection(originCellNum, CellNum);

        Vector2Int cp0 = CellNum;
        Vector2Int cp1 = CellNum;

        

        // チェックするコマを算出.
        if (isRight)
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
        if (!(starMaker.CheckLimitOfMap(cp0) && starMaker.CheckLimitOfMap(cp1)))
        {
            textchange = 1;
            uitext.GetComponent<TextMessnger>().Textflag = textchange;
            return false;
        }
        
        // 絶対にtrueなパターンのチェック
        if(starMaker.GetStar(cp0,StarType.BlackHole)) // 先1マス目がブラックホール
        {
            return true;
        }

        // 移動経路に邪魔する要素があるかチェック
        if(0 < starMaker.GetStarList(cp0, StarType.Rock).Count) // 先1マスにRockがある
        {
            textchange = 3;
            uitext.GetComponent<TextMessnger>().Textflag = textchange;
            return false;
        }
        else if(starMaker.GetStarList(cp0, StarType.Land).Exists(obj => obj.GetComponent<LandStarController>().CheckFlag(LANDSTAR_STAT.STUCKED)) || // いずれのマスにミルキーウェイにつかまっているLandがある
            starMaker.GetStarList(cp1, StarType.Land).Exists(obj => obj.GetComponent<LandStarController>().CheckFlag(LANDSTAR_STAT.STUCKED)))
        {
            textchange = 4;
            uitext.GetComponent<TextMessnger>().Textflag = textchange;
            return false;
        }
        else if(starMaker.GetStarList(cp0, StarType.Land).Exists(obj => !obj.GetComponent<LandStarController>().CheckFlag(LANDSTAR_STAT.STUCKED)) && // 1マス先に動けるLandがいて、2マス先にミルキーウェイがある. 
            0 < starMaker.GetStarList(cp1, StarType.MilkyWay).Count)
        {
            textchange = 5;
            uitext.GetComponent<TextMessnger>().Textflag = textchange;
            return false;
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

        if(additionalStat == LANDSTAR_STAT.IN_MILKYWAY_AREA && CheckFlag(LANDSTAR_STAT.MOVING))
        {
            if(timePast < timeToCirculate * 0.5f)
            {
                timeToCirculate *= 0.5f;
            }
        }

        CurrentStat |= additionalStat;
        return false;
    }

    public void RemoveFlag(LANDSTAR_STAT removingFlag)
    {
        CurrentStat &= ~removingFlag;
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