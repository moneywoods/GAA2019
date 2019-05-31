using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyAnime : MonoBehaviour
{

    private Animator animator;
    LandStarController Sprit;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objLand = GameObject.FindWithTag("Land");
        
        Sprit = objLand.GetComponent<LandStarController>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EatAnim()
    {
        //障害物につかまってる時
        if (Sprit.CheckFlag(LandStarController.LANDSTAR_STAT.STUCKED))
        {
            animator.SetBool("is_Catch", true);
            Debug.Log(Sprit.CheckFlag(LandStarController.LANDSTAR_STAT.STUCKED));

        }
        else
        {
            animator.SetBool("is_Catch", false);
        }
    }
}
