using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBehavior : StarBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if( collision.tag == ObjectTag.Land)
        {
            LandStarController landScript = collision.GetComponent<LandStarController>();
            landScript.AddStat(LandStarController.LANDSTAR_STAT.DESTROYED);
            landScript.RemoveFlag(LandStarController.LANDSTAR_STAT.MOVING);

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        
    }
}
