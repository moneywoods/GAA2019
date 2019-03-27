using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target; // Camera follows Target.

    public Vector3 offsetToTarget;

	// Use this for initialization
	void Start ()
    {
        
	}

    void Update()
    {
        if( target == null )
        {
            target = GameObject.FindGameObjectWithTag("PlayerCharacter");
            // offsetToTarget = this.transform.position - target.transform.position;
        }
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        if( target != null )
        {
            Vector3 tmp = transform.position;
            tmp.x = target.transform.position.x;
            tmp.y = target.transform.position.y;
            transform.position = tmp;
        }
        //  this.transform.position = target.transform.position + offsetToTarget;
    }
}
