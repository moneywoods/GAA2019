using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LandStarController))]

public class LandStarAnimationController : MonoBehaviour
{
    LandStarController Script;

    // Start is called before the first frame update
    void Start()
    {
        Script = GetComponent<LandStarController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //☆が動く時
        if (Script.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING) )
        {
            transform.Rotate(0, 50000 * Time.deltaTime, 0);
        }

        //障害物につかまってる時
        if (Script.CheckFlag(LandStarController.LANDSTAR_STAT.STUCKED))
        {
            transform.Rotate(0, 100 * Time.deltaTime, 0);
        }

        //プレイヤーが乗ってる時
        if (Script.CheckFlag(LandStarController.LANDSTAR_STAT.PLAYER_STAYING))
        {
            transform.Rotate(0, 0 * Time.deltaTime, 0);
        }

        //待機中
        else
        {
            transform.Rotate(0, 300 * Time.deltaTime, 0);
        }
    }
}


