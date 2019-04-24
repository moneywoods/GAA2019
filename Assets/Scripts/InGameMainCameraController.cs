using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;
public class InGameMainCameraController : StateContex
{
    public static class StateName
    {
        public static readonly string FollowingPlayer = "FollowingPlayer";
        public static readonly string Floating = "Floating";
        public static readonly string MovingFromGoalToStart = "MovingFromGoalToStart";
    }
    public GameObject target; // Camera follows Target.
    
    public Vector3 offsetToTarget;

    [SerializeField] private float dist5x5;
    [SerializeField] private float degree = -130.0f;
    InGameMainCameraController()
    {
        AddState(new StateFollowingPlayer(this, gameObject));
        AddState(new StateMovingFromGoalToStart(this, gameObject));

        SetCurrentState(StateName.FollowingPlayer);
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

    public void Init(StarMaker.MapInfo mapInfo)
    {
        var cameraScript = GetComponent<Camera>();
        float fov = cameraScript.fieldOfView;
        var width = mapInfo.CellSize.x * 5;
        dist5x5 = 0.6f * 0.5f * width * Mathf.Sqrt((3 - Mathf.Cos(fov)) / (1 - Mathf.Cos(fov)));
    }
    
    // ステート
    private class CameraState : State
    {
        public CameraState(StateContex contex, GameObject camera) : base(contex)
        {
            this.camera = camera;
            cameraScript = camera.GetComponent<InGameMainCameraController>();
        }

        protected GameObject camera = null;
        protected InGameMainCameraController cameraScript = null;
    }

    private class StateFollowingPlayer : CameraState
    {
        private Vector3 previousTargetPos;
        public StateFollowingPlayer(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.FollowingPlayer;
            OnEnter += Init;
            update += UsualUpdate;
        }

        private void Init()
        {
            Debug.Log("camera is following mode");
        }

        private void UsualUpdate()
        {
            if(cameraScript.target == null)
            {
                cameraScript.target = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter);
                if(cameraScript.target != null)
                {
                    cameraScript.Init(StarMaker.Instance.CurrentMapInfo);
                    update += FollowTarget;

                    var dist = new Vector3(0.0f, 0.0f, cameraScript.dist5x5);
                    dist = Quaternion.Euler(cameraScript.degree, 0.0f, 0.0f) * dist;
                    camera.transform.position = cameraScript.target.transform.position + dist;
                    camera.transform.LookAt(cameraScript.target.transform);
                    previousTargetPos = cameraScript.target.transform.position;
                }
            }
        }

        private void FollowTarget()
        {
            if(cameraScript.target == null)
            {
                update -= FollowTarget; // これまずいかな?
                return;
            }
            var diff = cameraScript.target.transform.position - previousTargetPos;
            camera.transform.position += diff;
            previousTargetPos = cameraScript.target.transform.position;
        }
    }

    private class StateMovingFromGoalToStart : CameraState
    {
        [SerializeField] private GameObject Destination;
        [SerializeField] private float duration;
        [SerializeField] private Vector3 diff;

        public StateMovingFromGoalToStart(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.MovingFromGoalToStart;
            OnEnter += Init;
        }

        private void Init()
        {
            cameraScript.target = GameObject.FindGameObjectWithTag(ObjectTag.GoalStar);
            Destination = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter);

            if(cameraScript.target != null && Destination != null)
            {
                cameraScript.Init(StarMaker.Instance.CurrentMapInfo);

                diff = (Destination.transform.position - cameraScript.target.transform.position) / duration; // 単位時間当たりの変位

                var dist = new Vector3(0.0f, 0.0f, cameraScript.dist5x5);
                dist = Quaternion.Euler(cameraScript.degree, 0.0f, 0.0f) * dist;
                camera.transform.position = cameraScript.target.transform.position + dist;
                camera.transform.LookAt(cameraScript.target.transform);

                update += MoveFromGoalToStart;
                return;
            }

            if(cameraScript.target == null)
            {
                Debug.Log("target couldnt be found.");
            }
            else if(Destination == null)
            {
                Debug.Log("Destination couldnt be found.");
            }
        }

        private void MoveFromGoalToStart()
        {
            camera.transform.position += diff * Time.deltaTime;
        }
    }
}
