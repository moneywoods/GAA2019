using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkyWayBehavior : StarBase
{
    public MilkyWayBehavior() : base(StarType.MilkyWay)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.Land)
        {
            other.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.IN_MILKYWAY_AREA);
        }
    }
}
