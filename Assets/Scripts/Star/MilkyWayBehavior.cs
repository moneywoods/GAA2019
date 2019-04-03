using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkyWayBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 当たり判定はとりあえず3にしてます.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == ObjectTag.Land )
        {
            collision.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.IN_MILKYWAY_AREA);
            Debug.Log("This is MilkyWay at " + transform.position.ToString("F2") + "."
                        + " Land at " + collision.transform.position.ToString("F2") + "is now [ IN_MILKY_WAY ]. " );
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
