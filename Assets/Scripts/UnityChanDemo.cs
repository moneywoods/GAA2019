using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tako;

public class UnityChanDemo : MonoBehaviour
{
    private Animator animator;
    TakoController Script;

    GameObject ChildObject;

    // Start is called before the first frame update
    void Start()
    {
        ChildObject = transform.GetChild(0).gameObject;



        Script = GetComponent<TakoController>();

        animator = ChildObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが待機中
        if (Script.CurrentState.Name == TakoController.StateName.Normal)
        {
            animator.SetBool("is_wait", true);

        }

        else
        {
            animator.SetBool("is_wait", false);
        }

        //星と星の間を移動中
        if (Script.CurrentState.Name == TakoController.StateName.MovingBetweenStars)
        {
            animator.SetBool("is_run", true);

        }

        else
        {
            animator.SetBool("is_run", false);
        }

        //星がプレイヤーの周りを移動中
        if (Script.CurrentState.Name == TakoController.StateName.WaitingForKineticPowerEnd)
        {
            animator.SetBool("is_starmove", true);

        }

        else
        {
            animator.SetBool("is_starmove", false);
        }
    }
}

