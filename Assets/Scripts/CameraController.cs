using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;
public class CameraController : StateContex
{
    public static class StateName
    {
        public static readonly string FollowingPlayer = "FollowingPlayer";
        public static readonly string Floating = "Floating";
        public static readonly string MovingFromGoalToStart = "MovingFromGoalToStart";
    }
    public GameObject target; // Camera follows Target.
    
    public Vector3 offsetToTarget;

    [SerializeField]private float dist5x5;

    void Awake()
    {
        AddState(new StateFollowingPlayer(this, gameObject));

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
        if( target != null )
        {
            var dist = new Vector3(0.0f, 0.0f, dist5x5);
            dist = Quaternion.Euler(-130.0f, 0.0f, 0.0f) * dist;
            transform.position = target.transform.position + dist;
        }
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
        }

        protected GameObject camera = null;
        protected CameraController cameraScript = null;
    }

    private class StateFollowingPlayer : CameraState
    {
        public StateFollowingPlayer(StateContex contex, GameObject camera) : base(contex, camera)
        {
            Name = StateName.FollowingPlayer;
            this.camera = camera;
            cameraScript = camera.GetComponent<CameraController>();

            update = UsualUpdate;
        }

        public void UsualUpdate()
        {
            if(cameraScript.target == null)
            {
                cameraScript.target = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter);
                if(cameraScript.target != null)
                {
                    cameraScript.Init(StarMaker.Instance.CurrentMapInfo);
                    update += FollowTarget;
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
            var dist = new Vector3(0.0f, 0.0f, cameraScript.dist5x5);
            dist = Quaternion.Euler(-130.0f, 0.0f, 0.0f) * dist;
            camera.transform.position = cameraScript.target.transform.position + dist;
        }
    }
}
