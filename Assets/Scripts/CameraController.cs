using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target; // Camera follows Target.
    public Vector3 offsetToTarget;

    [SerializeField]private float dist5x5;

	// Use this for initialization
	void Start ()
    {
        
	}
    
    void Update()
    {
        if( target == null )
        {
            target = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter);
            Init(StarMaker.Instance.CurrentMapInfo);
        }
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
}
