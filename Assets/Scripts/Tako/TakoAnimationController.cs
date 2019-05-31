using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tako;

public class TakoAnimationController : MonoBehaviour
{
    private Animator animator;
    private GameObject takoModel;
    TakoController takoScript;

    string flagIsWait = "isWait";
    string flagIsJump = "isJump";

    // Start is called before the first frame update
    void Start()
    {
        takoModel = transform.GetChild(0).gameObject;
        animator = takoModel.GetComponent<Animator>();
        takoScript = GetComponent<TakoController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(takoScript.CurrentState.Name == TakoController.StateName.Normal)
        //{
        //    animator.SetBool(flagIsWait, true);
        //}
    }
}
