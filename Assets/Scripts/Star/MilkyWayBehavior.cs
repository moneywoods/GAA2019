using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkyWayBehavior : StarBase
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
        if( collision.tag == ObjectTag.Land )
        {
            collision.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.IN_MILKYWAY_AREA);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        
    }
}
