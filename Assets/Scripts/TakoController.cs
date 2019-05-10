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
            public static readonly string MovingBetweenStars = "MovingBetweenStars";
            public static readonly string StayingGoal = "StayingGoal";
        }

        [SerializeField] private GameObject currentStarStaying; // 今いる星.
        [SerializeField] public GameObject nextStar
        {
            get;
            private set;
        }
        [SerializeField] public GameObject previousStar
        {
            get;
            protected set;
        }

        private List<GameObject> MovingStarList; // KineticPower適応中の星のリスト

        protected void Awake()
        {
            MovingStarList = new List<GameObject>();
            
            // ステートを生成
            AddState(new StateNormal(this, gameObject));
            AddState(new StateWaitingForKineticPowerEnd(this, gameObject));
            AddState(new StateCommandDisable(this, gameObject));
            AddState(new StateMovingBetweenStars(this, gameObject));
            AddState(new StateStayingGoal(this, gameObject));
            // 現在のステートをセット
            SetCurrentState(StateList.Find(m => m.Name == StateName.Normal));
        }

        protected override void Update()
        {
            if (Mathf.Approximately(Time.timeScale, 0f))
            {
                return;
            }
            base.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == nextStar && CurrentState.Name == StateName.MovingBetweenStars)
            {
                transform.position = other.transform.position;
                currentStarStaying = other.gameObject;
                nextStar = null;

                string next = StateName.Normal;

                if (other.tag == ObjectTag.GoalStar)
                {
                    next = StateName.StayingGoal;
                }

                TransitState(next);

            }
        }

        public void SetCurrentStarStaying(GameObject Land)
        {
            if (currentStarStaying != null)
            {
                currentStarStaying.GetComponent<LandStarController>().LeaveThisLand();
                previousStar = currentStarStaying;
            }
            else
            {
                // null
            }

            currentStarStaying = Land;
            transform.position = Land.transform.position;
            Land.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.PLAYER_STAYING);
        }

        public GameObject GetCurrentStarStaying()
        {
            return currentStarStaying;
        }

        public bool AskKineticPowerAvailable(List<GameObject> neighvorList, bool isRight)
        {
            bool result = true;
            foreach (GameObject star in neighvorList)
            {
                var script = star.GetComponent<StarBase>();
                var pos = currentStarStaying.GetComponent<StarBase>().CellNum;
                if (!star.GetComponent<StarBase>().CheckKineticPowerCanBeUsed(currentStarStaying.GetComponent<StarBase>().CellNum, isRight))
                {
                    result = false;
                }
                else
                {
                    // null
                }
            }
            return result;
        }

        /* ----- 操作関数 ----- */
        // 星を移動する.
        private bool MoveFromCurrentStar(Direction direction)
        {
            if (direction == Direction.ENUM_MAX || direction == Direction.NONE)
            {
                return false;
            }
            else
            {
                // null
            }

            // 指定された方向に行けるLandがあるかチェック.
            GameObject newLand = StarMaker.Instance.GetStar(currentStarStaying.GetComponent<StarBase>().CellNum, StarBase.StarType.Land, direction);

            if (newLand == null)
            {
                return false;
            }
            else
            {
                // null
            }

            // 移動を開始する.
            currentStarStaying.GetComponent<LandStarController>().LeaveThisLand();

            previousStar = currentStarStaying;
            currentStarStaying = null;
            nextStar = newLand;
            return true;
        }

        private GameObject GetStarOnTheDirection(Direction direction)
        {
            // 今いる星の隣接星リストを取得.
            var neighborStarList = StarMaker.Instance.GetNeighvorList(currentStarStaying.GetComponent<StarBase>().CellNum);

            if (neighborStarList == null)
            {
                Debug.Log("Something wrong! neighvorStarList == null. Function name : GetStarOnTHeDirection");
            }
            // 方向
            foreach (GameObject star in neighborStarList)
            {
                // 0326現在,リストに含まれるのは着陸可能星のみになってます. -> 全て
                if (star.tag == ObjectTag.Land || star.tag == ObjectTag.GoalStar)
                {
                    var landScript = star.GetComponent<LandStarController>();
                    if ((landScript.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE)) && !landScript.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        // プレイヤーから星への単位ベクトルを作る.
                        var vecPlayerToStar = star.transform.position - transform.position;
                        vecPlayerToStar.Normalize(); // 正規化する.

                        // 比較するための単位ベクトルを取得する.
                        var vecComp = new Vector3(1.0f, 0.0f);

                        float EstimatedStarDegree = 0.0f;

                        if (direction == Direction.Top)
                        {
                            EstimatedStarDegree = -90.0f;
                        }
                        else if (direction == Direction.LeftTop)
                        {
                            EstimatedStarDegree = -135.0f;
                        }
                        else if (direction == Direction.Left)
                        {
                            EstimatedStarDegree = -180.0f;
                        }
                        else if (direction == Direction.LeftBottom)
                        {
                            EstimatedStarDegree = -225.0f;
                        }
                        else if (direction == Direction.Bottom)
                        {
                            EstimatedStarDegree = -270.0f;
                        }
                        else if (direction == Direction.RightBottom)
                        {
                            EstimatedStarDegree = -315.0f;
                        }
                        else if (direction == Direction.Right)
                        {
                            EstimatedStarDegree = 0.0f;
                        }
                        else if (direction == Direction.RightTop)
                        {
                            EstimatedStarDegree = -45.0f;
                        }
                        else
                        {
                            return null;
                        }

                        Vector3 vecSearchngStar = Quaternion.Euler(0.0f, EstimatedStarDegree, 0.0f) * vecComp;
                        if (Vector3.Angle(vecPlayerToStar.normalized, vecSearchngStar.normalized) <= 10.0f)
                        {
                            return star;
                        }
                    }
                }
            }
            return null;
        }

        public void KineticPower(float estimatedTimeToCirculate, bool isRight) // 隣接するすべてのLandに回るよう指示する.
        {
            List<GameObject> neighvorStarList = StarMaker.Instance.GetNeighvorList(currentStarStaying.GetComponent<StarBase>().CellNum);

            for (int i = 0; i < neighvorStarList.Count; i++)
            {
                if (neighvorStarList[i].tag == ObjectTag.Land)
                {
                    LandStarController scriptNeighvor = neighvorStarList[i].GetComponent<LandStarController>();

                    if (scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) && !scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        neighvorStarList[i].GetComponent<LandStarController>().SetMove(gameObject, estimatedTimeToCirculate, isRight);
                        MovingStarList.Add(neighvorStarList[i]);
                    }
                }
            }
        }

        private class TakoState : State
        {
            public TakoState(StateContex stateContex, GameObject tako) : base(stateContex)
            {
                this.tako = tako;
                this.takoScript = this.tako.GetComponent<TakoController>();
                update = UsualUpdate;
                OnEnter = UsualEnterEvent;
                OnExit = UsualExitEvent;
            }

            protected GameObject tako;
            protected TakoController takoScript;

            protected void UsualUpdate()
            {
                // 共通更新
            }

            protected void UsualEnterEvent()
            {

            }

            protected void UsualExitEvent()
            {

            }

        }

        private class StateNormal : TakoState
        {
            public StateNormal(StateContex stateContex, GameObject tako) : base(stateContex, tako)
            {
                Name = StateName.Normal;
                update += UpdateByCommand;
            }

            void UpdateByCommand()
            {
                // 入力を取得.
                bool rightStarRotationInput = ( Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Alpha3) );      // 右ボタン
                bool leftStarRotationInput = ( Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Alpha1) );      // 左ボタン
//                bool playerMove = ( Input.GetAxis() )

                // 星を渡る.
                bool isMovementStart = false;
                Direction whichDirection = Direction.NONE;

                if (Input.GetKeyDown(KeyCode.D))
                {
                    whichDirection = Direction.Right;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    whichDirection = Direction.RightTop;
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    whichDirection = Direction.Top;
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    whichDirection = Direction.LeftTop;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    whichDirection = Direction.Left;
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    whichDirection = Direction.LeftBottom;
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    whichDirection = Direction.Bottom;
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    whichDirection = Direction.RightBottom;
                }
                else
                {
                    // null
                }

                // 移動
                if (whichDirection != Direction.NONE)
                {
                    isMovementStart = takoScript.MoveFromCurrentStar(whichDirection);
                }
                else
                {
                    // null
                }

                if (isMovementStart)
                {
                    Context.TransitState(StateName.MovingBetweenStars);
                    return;
                }
                else
                {
                    // null
                }

                // KineticPower
                if (rightStarRotationInput)
                {
                    var list = StarMaker.Instance.GetNeighvorList(takoScript.currentStarStaying.GetComponent<LandStarController>().CellNum);

                    if (takoScript.AskKineticPowerAvailable(list, true))
                    {
                        takoScript.KineticPower(2.0f, true);
                        takoScript.TransitState(StateName.WaitingForKineticPowerEnd);
                    }
                    else
                    {
                        // できなかった時の処理
                    }
                }
                else if (leftStarRotationInput)
                {
                    var list = StarMaker.Instance.GetNeighvorList(takoScript.currentStarStaying.GetComponent<LandStarController>().CellNum);

                    if (takoScript.AskKineticPowerAvailable(list, false))
                    {
                        takoScript.KineticPower(2.0f, false);
                        takoScript.TransitState(StateName.WaitingForKineticPowerEnd);
                    }
                    else
                    {
                        // できなかった時の処理
                    }
                }
            }
        }
        private class StateWaitingForKineticPowerEnd : TakoState
        {
            public StateWaitingForKineticPowerEnd(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.WaitingForKineticPowerEnd;
                update += CheckMovingLand;
            }

            void CheckMovingLand()
            {
                bool result = true;

                // neighvorListに移動中のLandがあるか確認.
                foreach (var star in takoScript.MovingStarList)
                {
                    if (star == null)
                    {
                        continue;
                    }

                    if (star.GetComponent<LandStarController>().CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        result = false;
                    }
                }
                if (result)
                { // 無ければステートをNormalへ.
                    takoScript.MovingStarList.Clear();
                    Context.TransitState(StateName.Normal);
                }
            }
        }

        private class StateCommandDisable : TakoState
        {
            public StateCommandDisable(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.CommandDisable;
            }
        }

        private class StateMovingBetweenStars : TakoState
        {
            public float EstimatedTimeToLand
            {
                get;
                private set;
            }
            private Vector3 diff;

            public StateMovingBetweenStars(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.MovingBetweenStars;
                OnEnter += Init;
                update += WaitingSmallWindow;
            }

            private float timeToWait = 0.15f;
            private float timeExpired = 0.0f;

            void WaitingSmallWindow()
            {
                timeExpired += Time.deltaTime;

                if(timeToWait <= timeExpired)
                 {
                    update -= WaitingSmallWindow;
                    update += MoveToStar;
                 }

            }


            void MoveToStar()
            {
                tako.transform.position += diff * Time.deltaTime;
            }

            void Init()
            {
                if (EstimatedTimeToLand == 0.0f)
                {
                    EstimatedTimeToLand = 1.0f; // とりあえず
                }
                else
                {
                    // null
                }

                diff = (takoScript.nextStar.transform.position - tako.transform.position) / EstimatedTimeToLand;
            }
        }

        private class StateStayingGoal : TakoState
        {
            public StateStayingGoal(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.StayingGoal;
            }
        }
    }
}