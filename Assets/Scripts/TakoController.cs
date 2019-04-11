using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;
namespace Tako
{
    public class TakoController : StateContex
    {
        public class StateName
        {
            public static readonly string Normal = "Normal";
            public static readonly string WaitingForKineticPowerEnd = "WaitingForKineticPowerEnd";
            public static readonly string CommandDisable = "CommandDisable";
        }
        internal GameObject currentStarStaying; // 今いる星.
        internal List<GameObject> neighvorList; // 今いる星の隣接星.星を移動したら必ず更新すること.

        protected void Awake()
        {
            // ステートを生成
            AddState(new StateNormal(this, gameObject));
            AddState(new StateWaitingForKineticPowerEnd(this, gameObject));
            AddState(new StateCommandDisable(this, gameObject));
            // 現在のステートをセット
            SetCurrentState(StateList.Find(m => m.Name == StateName.Normal));
        }

        protected override void Update()
        {
            base.Update();
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


        public PlayerCommandBehavior.Direction CheckDirection(GameObject obj)
        {
            var vecPlayerToStar = obj.transform.position - transform.position;


            // 比較するための単位ベクトルを取得する.
            var vecComp = new Vector3(1.0f, 0.0f); // 真上へのベクトル.

            // 方向に応じたベクトルを作成し,プレイヤーから星へのベクトルと比較.
            Vector3 vecSearchingStar;
            for (PlayerCommandBehavior.Direction i = 0; i < PlayerCommandBehavior.Direction.ENUM_MAX; i++)
            {
                vecSearchingStar = Quaternion.Euler(0.0f, 0.0f, (uint)i * 45.0f) * vecComp;

                if (Vector3.Angle(vecPlayerToStar.normalized, vecSearchingStar.normalized) <= 10.0f)
                {
                    return i;
                }
            }
            return PlayerCommandBehavior.Direction.NONE;
        }


        /* ----- 操作関数 ----- */
        // 星を移動する.
        public bool MoveFromCurrentStar(PlayerCommandBehavior.Direction _Direction)
        {
            // 行きたい方向に行けるLandがあるかチェック.
            GameObject newLand = null;

            if (_Direction == PlayerCommandBehavior.Direction.Top)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Top);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.LeftTop)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.LeftTop);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.Left)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Left);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.LeftBottom)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.LeftBottom);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.Bottom)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Bottom);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.RightBottom)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.RightBottom);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.Right)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Right);
            }
            else if (_Direction == PlayerCommandBehavior.Direction.RightTop)
            {
                newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.RightTop);
            }
            // 星を渡る.
            if (newLand != null)
            {
                // 移動処理
                foreach (GameObject obj in neighvorList)
                {
                    obj.GetComponent<LandStarController>().DiscardCanMoveToEffect();
                }
                var currentStarScript = currentStarStaying.GetComponent<LandStarController>();
                currentStarScript.LeaveThisLand(gameObject);

                newLand.GetComponent<LandStarController>().ArriveThisLand(gameObject);
                return true;
            }

            return false;
        }

        public GameObject GetStarOnTheDirection(PlayerCommandBehavior.Direction _Direction)
        {
            // 今いる星の隣接星リストを取得.
            GameObject tmp = currentStarStaying.transform.GetChild((int)LandStarController.ChildIndex.NeighvorFinder).gameObject;
            NeighvorFinder script = tmp.GetComponent<NeighvorFinder>();
            List<GameObject> neighborStarList = script.GetNeighvorStarList();

            if (neighborStarList == null)
            {
                Debug.Log("Something wrong! neighvorStarList == null. Function name : GetStarOnTHeDirection");
            }
            // 方向
            foreach (GameObject land in neighborStarList)
            {
                // 0326現在,リストに含まれるのは着陸可能星のみになってます.
                var landScript = land.GetComponent<LandStarController>();
                if ((landScript.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE)) && !landScript.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                {
                    // プレイヤーから星への単位ベクトルを作る.
                    var vecPlayerToStar = land.transform.position - transform.position;
                    vecPlayerToStar.Normalize(); // 正規化する.

                    // 比較するための単位ベクトルを取得する.
                    var vecComp = new Vector3(1.0f, 0.0f); // 真上へのベクトル.

                    float EstimatedStarDegree = 0.0f;

                    if (_Direction == PlayerCommandBehavior.Direction.Top)
                    {
                        EstimatedStarDegree = 90.0f; // 下方向Y正, 右方向X正 に注意!
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.LeftTop)
                    {
                        EstimatedStarDegree = 135.0f;
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.Left)
                    {
                        EstimatedStarDegree = 180.0f;
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.LeftBottom)
                    {
                        EstimatedStarDegree = 225.0f;
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.Bottom)
                    {
                        EstimatedStarDegree = 270.0f;
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.RightBottom)
                    {
                        EstimatedStarDegree = 315.0f;
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.Right)
                    {
                        EstimatedStarDegree = 0.0f;
                    }
                    else if (_Direction == PlayerCommandBehavior.Direction.RightTop)
                    {
                        EstimatedStarDegree = 45.0f;
                    }
                    else
                    {
                        return null;
                    }

                    Vector3 vecSearchngStar = Quaternion.Euler(0.0f, 0.0f, EstimatedStarDegree) * vecComp;
                    if (Vector3.Angle(vecPlayerToStar.normalized, vecSearchngStar.normalized) <= 10.0f)
                    {
                        return land;
                    }

                }
            }
            return null;
        }

        public void KineticPower(float estimatedTimeToCirculate, bool isRight) // 隣接するすべてのLandに回るよう指示する.
        {
            GameObject starScript = currentStarStaying.transform.GetChild(0).gameObject; // LandのChildの先頭はNeighvorFinderになるように調整すること.
            List<GameObject> neighvorStarList = starScript.GetComponent<NeighvorFinder>().GetNeighvorStarList();

            for (int i = 0; i < neighvorStarList.Count; i++)
            {
                if (neighvorStarList[i].tag == ObjectTag.Land)
                {
                    LandStarController scriptNeighvor = neighvorStarList[i].GetComponent<LandStarController>();

                    if (scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) && !scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        neighvorStarList[i].GetComponent<LandStarController>().SetMove(gameObject, estimatedTimeToCirculate, isRight);
                    }
                }
            }
        }

        public class TakoState : State
        {
            public TakoState(StateContex stateContex, GameObject tako) : base(stateContex)
            {
                this.tako = tako;
                this.takoScript = this.tako.GetComponent<TakoController>();
            }

            protected GameObject tako;
            protected TakoController takoScript;
        }

        public class StateNormal : TakoState
        {
            public StateNormal(StateContex stateContex, GameObject tako) : base(stateContex, tako)
            {
                Name = StateName.Normal;
                update = UpdateByCommand;
            }

            void UpdateByCommand()
            {
                // 入力を取得.
                // ゲームパッド
                bool rsh = Input.GetKeyDown(KeyCode.Joystick1Button5);      // 右ボタン
                bool lsh = Input.GetKeyDown(KeyCode.Joystick1Button4);      // 左ボタン

                // 星を渡る.
                if (Input.GetKeyDown(KeyCode.W))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.Top);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.RightTop);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.Right);

                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.RightBottom);

                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.Bottom);

                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftBottom);

                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftBottom);
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    takoScript.MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftTop);
                }


                if (Input.GetKeyDown(KeyCode.Alpha3) || rsh)
                {
                    takoScript.KineticPower(2.0f, true);
                    takoScript.TransitState(StateName.WaitingForKineticPowerEnd);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) || lsh)
                {
                    takoScript.KineticPower(2.0f, false);
                    takoScript.TransitState(StateName.WaitingForKineticPowerEnd);
                }
            }
        }
        public class StateWaitingForKineticPowerEnd : TakoState
        {
            public StateWaitingForKineticPowerEnd(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.WaitingForKineticPowerEnd;
                update = CheckMovingLand;
            }

            void CheckMovingLand()
            {
                // neighvorListに移動中のLandがあるか確認.
                var foundItem = takoScript.neighvorList.Find(item => item.GetComponent<LandStarController>().CheckFlag(LandStarController.LANDSTAR_STAT.MOVING) == true); // yokonagada...
                if (foundItem == null)
                { // 無ければステートをNormalへ.
                    Context.TransitState(StateName.Normal);
                }
            }
        }
        public class StateCommandDisable : TakoState
        {
            public StateCommandDisable(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.CommandDisable;
            }
        }
    }
}