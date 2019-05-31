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

    private GameObject m_ObjCamera;

    // Start is called before the first frame update
    void Start()
    {
        ChildObject = transform.GetChild(0).gameObject;

        Script = GetComponent<TakoController>();

        animator = ChildObject.GetComponent<Animator>();
         
        m_ObjCamera = GameObject.FindWithTag("MainCamera");

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが待機中
        if (Script.CurrentState.Name == TakoController.StateName.Normal)
        {
            animator.SetBool("is_wait", true);

            PlayerRotate();
        }

        else
        {
            animator.SetBool("is_wait", false);
        }

        //星と星の間を移動中
        if (Script.CurrentState.Name == TakoController.StateName.MovingBetweenStars)
        {
            animator.SetBool("is_run", true);

            PlayerRotate();
        }

        else
        {
            animator.SetBool("is_run", false);
        }

        //星がプレイヤーの周りを移動中
        if (Script.CurrentState.Name == TakoController.StateName.WaitingForKineticPowerEnd)
        {
            animator.SetBool("is_starmove", true);

            CameraLockOn();
        }

        else
        {
            animator.SetBool("is_starmove", false);
        }
    }


    private void PlayerRotate()
    {
        if (Script.nextStar != null)
        {
            target = Script.nextStar.transform;

            Vector3 targetDir = target.position - transform.position;
            targetDir.y = transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    private void CameraLockOn()
    {
        if(m_ObjCamera != null)
        {
            Transform target = m_ObjCamera.transform;

            Vector3 targetDir = target.position - transform.position;
            targetDir.y = transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

        }
    }
}

