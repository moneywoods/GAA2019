using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBehavior : StarBase
{
    BlackHoleBehavior() : base(StarType.BlackHole)
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
    
    public override bool CheckKineticPowerCanBeUsed(Vector2Int originCellNum, bool isRight)
    {
        return base.CheckKineticPowerCanBeUsed(originCellNum, isRight);
    }

    public override void TriggerEnterCell(GameObject other)
    {
        base.TriggerEnterCell(other);
    }

    public override void TriggerExitCell(GameObject other)
    {
        base.TriggerExitCell(other);
    }

    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.Land)
        {
            LandStarController landScript = other.GetComponent<LandStarController>();
            landScript.AddStat(LandStarController.LANDSTAR_STAT.DESTROYED);
            landScript.RemoveFlag(LandStarController.LANDSTAR_STAT.MOVING);
        }
    }

    public override void TriggerOtherLeaveFromSameCell(GameObject other)
    {
        base.TriggerOtherLeaveFromSameCell(other);
    }

}
