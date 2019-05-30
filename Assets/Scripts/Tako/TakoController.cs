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

        // アニメーション用
        private Animator animator;
        private GameObject takoModel;
        TakoController takoScript;
        // State内で使うものですが、SerializeFieldを利用したかったのでこちらで
        [SerializeField] private float takoAltitude; // 移動時のTakoのモデルのジャンプの高さ
        [SerializeField] private float timeToWait = 0.0f;

        private class AnimationFlagName
        {
            public static string flagIsJump = "isJump";
            public static string flagIsMoveStar = "isMoveStar";
            public static string flagIsNotMoveStar = "isNotMoveStar";
            public static string[] flagArray =
            {
                flagIsJump,          //ジャンプ
                flagIsMoveStar,　　　//☆を動かす
                flagIsNotMoveStar　　//☆が行動不能の時に☆を動かすやつ
            };
        }

        protected void Awake()
        {
            // 変数初期化
            MovingStarList = new List<GameObject>();
            takoModel = transform.GetChild(0).gameObject;
            animator = takoModel.GetComponent<Animator>();
            takoScript = GetComponent<TakoController>();

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

        private void OnTriggerEnter(Collider other) // 目標の星と衝突したらその星についたこととする
        {
            if (other.gameObject == nextStar && CurrentState.Name == StateName.MovingBetweenStars)
            {
                //transform.position = other.transform.position;
                //currentStarStaying = other.gameObject;
                //nextStar = null;

                //string next = StateName.Normal;

                //if (other.tag == ObjectTag.GoalStar)
                //{
                //    next = StateName.StayingGoal;
                //}

                //TransitState(next);
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
                    SetAnimationFlagTrue(AnimationFlagName.flagIsJump);
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
        private bool CheckLandInDirection(Direction direction)
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

            // 目的地を変更
            nextStar = newLand;

            return true;
        }

        private void IsJump()
        {
            currentStarStaying.GetComponent<LandStarController>().LeaveThisLand();

            previousStar = currentStarStaying;
            currentStarStaying = null;
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

        // AnimationFlagName.flagArrayを参照して、指定されたbool型のアニメーションフラグをtrueに設定するだけ。
        // 複合条件には対応してません。
        void SetAnimationFlagTrue(string targetName)
        {
            for(int i = 0; i < AnimationFlagName.flagArray.GetLength(0); i++)
            {
                if (AnimationFlagName.flagArray[i] == targetName)
                {
                    animator.SetBool(AnimationFlagName.flagArray[i], true);
                    Debug.Log(AnimationFlagName.flagArray[i] + " is true");
                }
                else
                {
                    animator.SetBool(AnimationFlagName.flagArray[i], false);
                    Debug.Log(AnimationFlagName.flagArray[i] + " is false");
                }
            }
        }

        void ClearAnimationFlag()
        {
            for (int i = 0; i < AnimationFlagName.flagArray.GetLength(0); i++)
            {
                animator.SetBool(AnimationFlagName.flagArray[i], false);
                Debug.Log("tako animation flag is cleared");
            }
        }

        // ステート
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
            public Direction facingDirection;

            public StateNormal(StateContex stateContex, GameObject tako) : base(stateContex, tako)
            {
                Name = StateName.Normal;
                facingDirection = Direction.NONE;
                OnEnter += CheckAndSelectStarInFacingCell;
                OnEnter += takoScript.ClearAnimationFlag;
                update += UpdateByCommand;
            }

            // 向いている方のマスにLandがあるなら、そのLandをnextStarに設定する
            void CheckAndSelectStarInFacingCell()
            {
                if(facingDirection != Direction.NONE)
                {
                    takoScript.CheckLandInDirection(facingDirection);
                }
            }

            void UpdateByCommand()
            {
                IsNextStarCommand();

                RotateCharacter();

                IsJumpCommand();

                IsKineticPowerCommand();
            }

            void RotateCharacter()
            {
                if(takoScript.nextStar == null)
                {
                    return;
                }

                // モデルの向きを調整
                Transform target = takoScript.nextStar.transform;

                Vector3 targetDir = target.position - tako.transform.position;
                targetDir.y = tako.transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
                float step = 10.0f * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(tako.transform.forward, targetDir, step, 0.0f);
                tako.transform.rotation = Quaternion.LookRotation(newDir);
            }

            private void IsNextStarCommand()
            {
                // スティックのしきい値
                float INPUT_HORIZONTAL = 0.9f;
                float INPUT_VERTICAL = 0.9f;
                float INPUT_UP = 0.5f;
                float INPUT_DOWN = -0.5f;
                float INPUT_LEFT = -0.5f;
                float INPUT_RIGHT = 0.5f;


                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");

                // 入力を取得
                bool inputUp = (moveY >= INPUT_VERTICAL || Input.GetKeyDown(KeyCode.W));            // 上入力
                bool inputDown = (moveY <= -INPUT_VERTICAL || Input.GetKeyDown(KeyCode.X));         // 下入力
                bool inputLeft = (moveX <= -INPUT_HORIZONTAL || Input.GetKeyDown(KeyCode.A));       // 左入力
                bool inputRight = (moveX >= INPUT_HORIZONTAL || Input.GetKeyDown(KeyCode.D));       // 右入力

                bool inputLeftUp = (moveX <= INPUT_LEFT && moveY >= INPUT_UP || Input.GetKeyDown(KeyCode.Q));       // 左上
                bool inputLeftDown = (moveX <= INPUT_LEFT && moveY <= INPUT_DOWN || Input.GetKeyDown(KeyCode.Z));   // 左下
                bool inputRightUp = (moveX >= INPUT_RIGHT && moveY >= INPUT_UP || Input.GetKeyDown(KeyCode.E));     // 右上
                bool inputRightDown = (moveX >= INPUT_RIGHT && moveY <= INPUT_DOWN || Input.GetKeyDown(KeyCode.C)); // 右下

                Direction indexDirection = Direction.NONE;
                // 渡る星を選択
                if (inputLeft)
                {
                    indexDirection = Direction.Left;
                }
                if (inputRight)
                {
                    indexDirection = Direction.Right;
                }
                if (inputUp)
                {
                    indexDirection = Direction.Top;
                }
                if (inputDown)
                {
                    indexDirection = Direction.Bottom;
                }
                if (inputLeftUp)
                {
                    indexDirection = Direction.LeftTop;
                }
                if (inputLeftDown)
                {
                    indexDirection = Direction.LeftBottom;
                }
                if (inputRightUp)
                {
                    indexDirection = Direction.RightTop;
                }
                if (inputRightDown)
                {
                    indexDirection = Direction.RightBottom;
                }
                // 移動
                if (indexDirection != Direction.NONE)
                {
                    takoScript.CheckLandInDirection(indexDirection);
                    facingDirection = indexDirection;
                }
                else
                {
                    // null
                }
            }

            private void IsJumpCommand()
            {
                bool inputJump = (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space));                   // ジャンプボタン
                bool isJump = inputJump && takoScript.nextStar;
                if (isJump)
                {
                    takoScript.IsJump();
                    Context.TransitState(StateName.MovingBetweenStars);
                }
            }

            private void IsKineticPowerCommand()
            {
                bool inputRight = (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Alpha3));     // 右回転ボタン
                bool inputLeft = (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Alpha1));      // 左回転ボタン

                if (inputRight)
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
                else if (inputLeft)
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
                OnEnter += OnEnterEvent;
                OnExit += OnExitEvent;

                update += CheckMovingLand;
            }
            void OnEnterEvent()
            {
                takoScript.SetAnimationFlagTrue(AnimationFlagName.flagIsMoveStar);
            }

            void OnExitEvent()
            {

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
            public float EstimatedTimeToLand // Takoの移動時間 // なぜここに書いているのか
            {
                get;
                private set;
            }
            private Vector3 diff;

            public StateMovingBetweenStars(StateContex contex, GameObject tako) : base(contex, tako)
            {
                Name = StateName.MovingBetweenStars;
                OnEnter += Init;
                update += Update;
                OnExit += AdjustTakoModelOnExitState;
            }

            private float timeExpired = 0.0f;

            void WaitingSmallWindow()
            {
                if(takoScript.timeToWait <= timeExpired)
                 {
                    update -= WaitingSmallWindow;
                    update += AdjustTakoModelAltitude;
                    update += MoveToStar;

                    // 経過時間のリセット
                    timeExpired = 0.0f;
                 }

            }


            void MoveToStar()
            {
                tako.transform.position += diff * Time.deltaTime;

                //// モデルの向きを調整
                Transform target = takoScript.nextStar.transform;

                Vector3 targetDir = target.position - tako.transform.position;
                targetDir.y = tako.transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
                float step = 10.0f * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(tako.transform.forward, targetDir, step, 0.0f);
                tako.transform.rotation = Quaternion.LookRotation(newDir);

                // 経過時間が予定時間を過ぎている場合、次の☆についているはずなのでチェックして、着地
                if (EstimatedTimeToLand <= timeExpired)
                {
                    //// ありえないけど、一応距離が離れすぎていないかチェック
                    //if(!(Vector3.Distance(takoScript.nextStar.transform.position, tako.transform.position) < 1.0f))
                    //{
                    //    Debug.Log("予定時間を過ぎてるのにTakoが星についてないぞ！");
                    //    return;
                    //}

                    tako.transform.position = takoScript.nextStar.transform.position;　// 位置の調整　一応ね
                    takoScript.currentStarStaying = takoScript.nextStar.gameObject; // 滞在星を更新

                    // ステートを遷移
                    string next = StateName.Normal;

                    // ゴールに
                    if (takoScript.nextStar.tag == ObjectTag.GoalStar)
                    {
                        next = StateName.StayingGoal;
                    }

                    takoScript.nextStar = null; // nextStarはnullへ
                    takoScript.TransitState(next);
                }
            }

            // Takoの3Dモデルのローカル座標の変更
            // Takoの高さを変えて、ジャンプしているように見せたい
            void AdjustTakoModelAltitude()
            {
                if(timeExpired < EstimatedTimeToLand * 0.5f)
                {
                    var pos = takoScript.takoModel.transform.position;
                    pos.y += takoScript.takoAltitude / EstimatedTimeToLand * 0.5f * Time.deltaTime;
                    takoScript.takoModel.transform.position = pos;
                }
                else
                {
                    var pos = takoScript.takoModel.transform.position;
                    pos.y -= takoScript.takoAltitude / EstimatedTimeToLand * 0.5f * Time.deltaTime;
                    takoScript.takoModel.transform.position = pos;
                }
            }

            void Init()
            {
                timeExpired = 0.0f;
                update = Update;
                update += WaitingSmallWindow;

                // アニメーションステート遷移
                if (!takoScript.animator.GetBool(AnimationFlagName.flagIsJump))   //takoジャンプ
                {
                    takoScript.SetAnimationFlagTrue(AnimationFlagName.flagIsJump);
                }


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

            // 定例更新処理
            // 時間を足すだけ
            void Update()
            {
                timeExpired += Time.deltaTime;
            }

            void AdjustTakoModelOnExitState()
            {
                takoScript.takoModel.transform.localPosition = Vector3.zero;
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