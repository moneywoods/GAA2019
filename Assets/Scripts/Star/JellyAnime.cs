using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyAnime : MonoBehaviour
{

    private Animator animator;
    LandStarController Script;

    // Start is called before the first frame update
    void Start()
    {
        Script = GetComponent<LandStarController>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //障害物につかまってる時
        if (Script.CheckFlag(LandStarController.LANDSTAR_STAT.STUCKED))
        {
            animator.SetBool("is_catch", true);

        }

        else
        {
            animator.SetBool("is_catch", false);
        }
    }
}
