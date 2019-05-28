using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class InGameMainCameraController : StateContex
{
    public static class StateName
    {
        public static readonly string Following = "Following";
        public static readonly string Floating = "Floating";
        public static readonly string MovingFromGoalToStart = "MovingFromGoalToStart";
        public static readonly string GameClearEvent = "GameClearEvent";
    }

    public GameObject target // Camera follows Target.
    {
        get;
        set;
    }
    
    public Vector3 offsetToTarget;

    [SerializeField] private float margin = 0.0f;
    [SerializeField] private float dist5x5;          // 5x5のマスを移すために必要な距離（を作りたかった）
    [SerializeField] private float degree = -130.0f; // カメラとプレイヤーキャラクタを結ぶ線が水平と為す角度

    [SerializeField] private Vector2Int ScreenSize;


    // StateGameClearEvent用
    [SerializeField] private Vector3 angleOffset;
    [SerializeField] private float radius;
    [SerializeField] private float timeToApproach = 1;

    private void Awake()
    {
        AddState(new StateFollowing(this, gameObject));
        AddState(new StateMovingFromGoalToStart(this, gameObject));
        AddState(new StateFloating(this, gameObject));

        SetCurrentState(StateName.Floating);
    }
    // Use this for initialization
    void Start ()
    {
        
	}
    
    protected override void Update()
    {
        base.Update();
    }

    // Update is called once per frame
    void LateUpdate ()
    {

    }

    public float GetDistXxX(int cellNum) // XかけるXのコマを映すカメラとターゲットの距離を戻す.
    {
        var cameraScript = GetComponent<Camera>();

        var mapInfo = StarMaker.Instance.CurrentMapInfo;

        // float fov = cameraScript.fieldOfView;
        float fov = cameraScript.fieldOfView * Screen.width / Screen.height;
        var radius = 0.5f * mapInfo.CellSize.x * cellNum * Mathf.Sqrt(2.0f);
        var dist = (radius) / Mathf.Sin(fov * 0.5f * Mathf.Deg2Rad);
        return dist;
    }

    public void MoveAndLookAtTarget()
    {
        var dist = new Vector3(0.0f, 0.0f, dist5x5);
        dist = Quaternion.Euler(degree, 0.0f, 0.0f) * dist;
        transform.position = target.transform.position + dist;
        transform.LookAt(target.transform);
    }
    
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    // ステート
    private class CameraState : State
    {
        public CameraState(StateContex contex, GameObject camera) : base(contex)
        {
            this.camera = camera;
            cameraScript = camera.GetComponent<InGameMainCameraController>();
            update += GetScreenInfo;
        }

        protected GameObject camera = null;
        protected InGameMainCameraController cameraScript = null;

        public void GetScreenInfo()
        {
            cameraScript.ScreenSize.x = Screen.width;
            cameraScript.ScreenSize.y = Screen.height;
        }
    }

    private class StateFollowing : CameraState
    {
        private Vector3 previousTargetPos;
        private float prevFov;
        private float prevDegree;

        public StateFollowing(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.Following;
            OnEnter += Init;
        }

        private void Init()
        {
            Debug.Log("camera is following mode");
            Debug.Log("target is " + cameraScript.target.ToString() + ".");

            InitPos();

            update += AdjustDist;
            update += FollowTarget;
        }

        private void InitPos()
        {
            cameraScript.dist5x5 = cameraScript.GetDistXxX(5);
            cameraScript.MoveAndLookAtTarget();
            previousTargetPos = cameraScript.target.transform.position;

            var cameraComponent = camera.GetComponent<Camera>();
            prevFov = cameraComponent.fieldOfView;

            prevDegree = cameraScript.degree;
        }

        private void AdjustDist()
        {
            var currentFov = camera.GetComponent<Camera>().fieldOfView;

            if(prevFov != currentFov ||
                prevDegree != cameraScript.degree)
            {
                InitPos();
            }
        }

        private void FollowTarget()
        {
            if(cameraScript.target == null)
            {
                update -= FollowTarget;
                return;
            }

            var diff = cameraScript.target.transform.position - previousTargetPos;
            camera.transform.position += diff;
            previousTargetPos = cameraScript.target.transform.position;
        }
    }

    private class StateMovingFromGoalToStart : CameraState
    {
        private GameObject Goal;
        private float durationG2S = 2.0f;
        private float durationToWait = 0.5f;
        private float durationZoomOut = 2.0f;
        private float timePast = 0.0f;
        private Vector3 diffG2S;
        private float diffDistZoomOut;
        private float originalDist5x5;

        public StateMovingFromGoalToStart(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.MovingFromGoalToStart;
            OnEnter += Init;
        }

        private void Init()
        {
            update += Search;
            timePast = 0.0f;
        }

        private void Search()
        {
            cameraScript.target = GameObject.FindGameObjectWithTag(ObjectTag.GoalStar);
            Goal = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter);

            if(cameraScript.target == null)
            {
                Debug.Log("target couldnt be found.");
                return;
            }
            else if(Goal == null)
            {
                Debug.Log("Destination couldnt be found.");
                return;
            }

            if(cameraScript.target != null && Goal != null)
            {
                originalDist5x5 = cameraScript.GetDistXxX(5);
                cameraScript.dist5x5 = cameraScript.GetDistXxX(2);

                diffDistZoomOut = (originalDist5x5 - cameraScript.dist5x5) / durationZoomOut;

                update -= Search;
                update += ZoomOutFromGoal;
                return;
            }
        }

        private void ZoomOutFromGoal()
        {
            var time = Time.deltaTime;

            if(durationZoomOut < time + timePast)
            {
                time = durationZoomOut - timePast;
            }

            cameraScript.dist5x5 += diffDistZoomOut * time;

            cameraScript.MoveAndLookAtTarget();

            timePast += time;

            if(durationZoomOut <= timePast)
            {
                timePast = 0.0f;
                update -= ZoomOutFromGoal;
                update += Wait;
                return;
            }
        }

        private void Wait()
        {
            var time = Time.deltaTime;

            if(durationToWait < time + timePast)
            {
                time = durationToWait - timePast;
            }

            timePast += time;

            if(durationToWait <= timePast)
            {
                timePast = 0.0f;
                diffG2S = (Goal.transform.position - cameraScript.target.transform.position) / durationG2S; // 単位時間当たりの変位
                update -= Wait;
                update += MoveFromGoalToStart;
                return;
            }
        }

        private void MoveFromGoalToStart()
        {
            // 移動する
            var time = Time.deltaTime;

            if(durationG2S < time + timePast)
            {
                time = durationG2S - timePast;
            }

            camera.transform.position += diffG2S * time;

            timePast += time;

            // スタート地点まで映した後はキャラクター追従ステートへ
            if(durationG2S <= timePast)
            {
                var tako = GameObject.FindWithTag(ObjectTag.PlayerCharacter);
                cameraScript.target = tako;
                tako.GetComponent<Tako.TakoController>().SetCurrentState(Tako.TakoController.StateName.Normal);
                Context.TransitState(StateName.Following);
                return;
            }
        }
    }

    private class StateFloating : CameraState
    {
        private float speed = 1.0f; // per sec
        public StateFloating(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.Floating;
            update += MoveByKeyInput;
        }

        public void MoveByKeyInput()
        {
            var vec = new Vector3();

            if(Input.GetKey(KeyCode.UpArrow))
            {
                vec = Vector3.forward * speed;
            }
            else if(Input.GetKey(KeyCode.LeftArrow))
            {
                vec = Vector3.right * -speed;
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                vec = Vector3.forward * -speed;
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                vec = Vector3.right * speed;
            }

            camera.transform.position += vec;
        }
    }

    private class StateGameClearEvent : CameraState
    {
        private Vector3 destination;
        private GameObject tako;
        private float timeExpired = 0.0f;

        StateGameClearEvent(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.GameClearEvent;
            OnEnter = Init;
        }

        void Init()
        {
            tako = GameObject.FindWithTag(ObjectTag.PlayerCharacter);

            if(tako == null)
            {
                Debug.Log("Camera failed to Init StateGameClearEvent.");
                cameraScript.TransitState(StateName.Following);
            }

            timeExpired = 0.0f;

            update += UpdateTime;
            update += Approach;
        }

        void Approach()
        {
            // 目標位置を計算する
            var vec = Vector3.forward * cameraScript.radius;
            destination = Quaternion.Euler(cameraScript.angleOffset.x, cameraScript.angleOffset.y, cameraScript.angleOffset.z) * vec + tako.transform.position;

            // このフレームで移動する距離を計算する
            var diff = destination - camera.transform.position / (cameraScript.timeToApproach - timeExpired);

            camera.transform.position += diff;
        }
        
        void UpdateTime()
        {
            timeExpired += Time.deltaTime;
        }
    }
}
