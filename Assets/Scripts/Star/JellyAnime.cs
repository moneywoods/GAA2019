using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyAnime : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CatchAnim()
    {
        //障害物につかまってる時
        animator.SetBool("is_Catch", true);        
    }

    public void StopAnim()
    {
        animator.SetBool("is_Catch", false);
    }
}
