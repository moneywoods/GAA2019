using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakoController : MonoBehaviour
{
    internal GameObject currentStarStaying;
    TakoState.StateContex stateContex;

    // 移動可能な隣接星を明示するエフェクト作成用.
    internal List<GameObject> neighvorList;

    // Start is called before the first frame update
    void Start()
    {
        stateContex = new TakoState.StateContex(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // 移動可能な隣接星を明示するエフェクト作成.
        // この処理は他のファイルとかにした方がゴチャらなくてなくていいかも
        foreach( GameObject obj in neighvorList )
        {
            if(obj.tag == ObjectTag.Land || obj.tag == ObjectTag.GoalStar) // 今のところ着陸可能星しかリストに入ってないのでこの条件いらないけど一応.
            {
                obj.GetComponent<LandStarController>().SetCanMoveToEffect(CheckDirection(obj));
            }
        }
        // 状態ごとの更新
        stateContex.StateUpdate();
    }

    void SetState(TakoState.StateIndex state)
    {
        stateContex.CurrentState = TakoState.StateIndex.Normal;
    }

    public void SetCurrentStarStaying(GameObject Land)
    {
        currentStarStaying = Land;
        transform.position = Land.transform.position;
        neighvorList = Land.transform.GetChild(0).GetComponent<NeighvorFinder>().GetNeighvorStarList();
        Land.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.PLAYER_STAYING);
    }

    public GameObject GetCurrentStarStaying()
    {
        return currentStarStaying;
    }


    PlayerCommandBehavior.Direction CheckDirection(GameObject obj)
    {
        var vecPlayerToStar = obj.transform.position - transform.position;


        // 比較するための単位ベクトルを取得する.
        var vecComp = new Vector3(1.0f, 0.0f); // 真上へのベクトル.

        // 方向に応じたベクトルを作成し,プレイヤーから星へのベクトルと比較.
        Vector3 vecSearchingStar;
        for (PlayerCommandBehavior.Direction i = 0; i < PlayerCommandBehavior.Direction.ENUM_MAX; i++ )
        {
            vecSearchingStar = Quaternion.Euler(0.0f, 0.0f, (uint)i * 45.0f ) * vecComp;

            if (Vector3.Angle(vecPlayerToStar.normalized, vecSearchingStar.normalized) <= 10.0f)
            {
                return i;
            }
        }
        return PlayerCommandBehavior.Direction.NONE;
    }
}


namespace TakoState
{
    public enum StateIndex
    {
        Normal = 0,
        CommandDisable,
        ENUM_MAX
    }
    public class StateContex
    {
        StateIndex _currentState;
        public StateIndex CurrentState
        {
            set { _currentState = value; }
            get { return _currentState; }
        }
        public StateContex(GameObject tako)
        {
            _currentState = StateIndex.Normal;
            _State = new AbstractState[(int) StateIndex.ENUM_MAX];
            AbstractState.SetTako(tako);
            _State[(int) StateIndex.Normal] = new StateNormal();
        }

        // AbstractState本体
        private AbstractState[] _State;

        public void StateUpdate()
        {
            _State[(int) CurrentState].StateUpdate();
        }
    }

    // AbstractStateクラス
    public abstract class AbstractState
    {
        protected static GameObject tako;
        protected static TakoController takoScript;
        public static void SetTako(GameObject newTako)
        {
            tako = newTako;
            takoScript = tako.GetComponent<TakoController>();
        }
        // Update
        // デリゲート
        public delegate void excuteUpdate();
        private excuteUpdate execUpdate;

        // 実行処理
        public virtual void StateUpdate()
        {
            if( execUpdate != null)
            {
                execUpdate();
            }
        }

        /* ----- 操作関数 ----- */
        // 星を移動する.
        protected bool MoveFromCurrentStar(PlayerCommandBehavior.Direction _Direction)
        {
            // 行きたい方向に行けるLandがあるかチェック.
            GameObject newLand = null;

            if( _Direction == PlayerCommandBehavior.Direction.Top )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Top);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.LeftTop )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.LeftTop);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.Left )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Left);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.LeftBottom )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.LeftBottom);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.Bottom )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Bottom);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.RightBottom )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.RightBottom);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.Right )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Right);
            }
            else if( _Direction == PlayerCommandBehavior.Direction.RightTop )
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.RightTop);
            }
            // 星を渡る.
            if( newLand != null )
            {
                // 移動処理
                foreach( GameObject obj in takoScript.neighvorList )
                {
                    obj.GetComponent<LandStarController>().DiscardCanMoveToEffect();
                }
                var currentStarScript = takoScript.currentStarStaying.GetComponent<LandStarController>();
                currentStarScript.LeaveThisLand(tako);

                newLand.GetComponent<LandStarController>().ArriveThisLand(tako);
                return true;
            }

            return false;
        }

        protected GameObject GetStarOnTheDirection(PlayerCommandBehavior.Direction _Direction)
        {
            // 今いる星の隣接星リストを取得.
            GameObject tmp = takoScript.currentStarStaying.transform.GetChild(0).gameObject;
            NeighvorFinder script = tmp.GetComponent<NeighvorFinder>();
            List<GameObject> neighborStarList = script.GetNeighvorStarList();

            if( neighborStarList == null )
            {
                Debug.Log("Something wrong! neighvorStarList == null. Function name : GetStarOnTHeDirection");
            }
            // 方向
            foreach( GameObject land in neighborStarList )
            {
                // 0326現在,リストに含まれるのは着陸可能星のみになってます.
                var landScript = land.GetComponent<LandStarController>();
                if( (landScript.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE)) && !landScript.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING) )
                {
                    // プレイヤーから星への単位ベクトルを作る.
                    var vecPlayerToStar = land.transform.position - tako.transform.position;
                    vecPlayerToStar.Normalize(); // 正規化する.

                    // 比較するための単位ベクトルを取得する.
                    var vecComp = new Vector3(1.0f, 0.0f); // 真上へのベクトル.

                    float EstimatedStarDegree = 0.0f;

                    if( _Direction == PlayerCommandBehavior.Direction.Top )
                    {
                        EstimatedStarDegree = 90.0f; // 下方向Y正, 右方向X正 に注意!
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.LeftTop )
                    {
                        EstimatedStarDegree = 135.0f;
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.Left )
                    {
                        EstimatedStarDegree = 180.0f;
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.LeftBottom )
                    {
                        EstimatedStarDegree = 225.0f;
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.Bottom )
                    {
                        EstimatedStarDegree = 270.0f;
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.RightBottom )
                    {
                        EstimatedStarDegree = 315.0f;
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.Right )
                    {
                        EstimatedStarDegree = 0.0f;
                    }
                    else if( _Direction == PlayerCommandBehavior.Direction.RightTop )
                    {
                        EstimatedStarDegree = 45.0f;
                    }
                    else
                    {
                        return null;
                    }

                    Vector3 vecSearchngStar = Quaternion.Euler(0.0f, 0.0f, EstimatedStarDegree) * vecComp;
                    if( Vector3.Angle(vecPlayerToStar.normalized, vecSearchngStar.normalized) <= 10.0f )
                    {
                        return land;
                    }

                }
            }
            return null;
        }

        protected void KineticPower(float estimatedTimeToCirculate, bool isRight) // 隣接するすべてのLandに回るよう指示する.
        {
            GameObject starStaying = takoScript.GetCurrentStarStaying();
            GameObject starScript = starStaying.transform.GetChild(0).gameObject; // LandのChildの先頭はNeighvorFinderになるように調整すること.
            List<GameObject> neighvorStarList = starScript.GetComponent<NeighvorFinder>().GetNeighvorStarList();

            for(int i = 0; i < neighvorStarList.Count; i++)
            {
                if(neighvorStarList[i].tag == ObjectTag.Land)
                {
                    LandStarController scriptNeighvor = neighvorStarList[i].GetComponent<LandStarController>();

                    if(scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) && !scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        neighvorStarList[i].GetComponent<LandStarController>().SetMove(tako.gameObject, estimatedTimeToCirculate, isRight);                       
                    }
                }
            }
           
        }

    }

    public class StateNormal : AbstractState
    {
        public override void StateUpdate()
        {
            // 入力を取得.
            // ゲームパッド
            bool rsh = Input.GetKeyDown(KeyCode.Joystick1Button5);      // 右ボタン
            bool lsh = Input.GetKeyDown(KeyCode.Joystick1Button4);      // 左ボタン


            // 星を渡る.
            if(Input.GetKey(KeyCode.W))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.Top);
            }
            else if(Input.GetKey(KeyCode.E))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.RightTop);

            }
            else if(Input.GetKey(KeyCode.D))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.Right);

            }
            else if(Input.GetKey(KeyCode.C))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.RightBottom);

            }
            else if(Input.GetKey(KeyCode.X))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.Bottom);

            }
            else if(Input.GetKey(KeyCode.Z))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftBottom);

            }
            else if(Input.GetKey(KeyCode.A))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftBottom);
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftTop);
            }


            if(Input.GetKeyDown(KeyCode.Alpha3) || rsh)
            {
                KineticPower(2.0f, true);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1) || lsh)
            {
                KineticPower(2.0f, false);
            }
        }
    }

    public class StateDisableCommand : AbstractState
    {
        public override void StateUpdate()
        {
            // no acceptable command 
        }
    }
}