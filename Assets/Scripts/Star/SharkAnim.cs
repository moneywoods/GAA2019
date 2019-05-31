using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAnim : MonoBehaviour
{
    private Animator animator;
    private bool IsCatch = false;
    private float m_Timer = 0f;
    private readonly float ENDANIM = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsCatch)
        {
            m_Timer += Time.deltaTime;
            if(m_Timer >= ENDANIM)
            {
                StopAnim();
                m_Timer = 0f;
            }
        }
    }

    public void EatAnim()
    {
        //障害物につかまってる時
        animator.SetBool("isShark", true);
        IsCatch = true;
    }

    private void StopAnim()
    {
        animator.SetBool("isShark", false);
        IsCatch = false;
    }
}
