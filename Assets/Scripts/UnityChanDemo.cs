using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tako;

public class UnityChanDemo : MonoBehaviour
{
    private Animator animator;
    TakoController Script;

    GameObject ChildObject;

    Transform target;
    float speed = 10f;

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

            target = Script.nextStar.transform;

            Vector3 targetDir = target.position - transform.position;
            targetDir.y = transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

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

