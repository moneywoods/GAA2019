using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LandStarController))]

public class JellyAnime : MonoBehaviour
{

    private Animator animator;
    LandStarController Sprit;

    // Start is called before the first frame update
    void Start()
    {
        Sprit = GetComponent<LandStarController>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //障害物につかまってる時
        if (Sprit.CheckFlag(LandStarController.LANDSTAR_STAT.STUCKED))
        {
            animator.SetBool("test", true);
            Debug.Log(Sprit.CheckFlag(LandStarController.LANDSTAR_STAT.STUCKED));

        }

        else
        {
            animator.SetBool("test", false);
        }
    }
}
