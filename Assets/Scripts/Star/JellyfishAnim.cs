using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishAnim : MonoBehaviour
{
    private Animator animator;
    private GameObject JellyfishModel;

    // Start is called before the first frame update
    void Start()
    {
        JellyfishModel = gameObject;
        animator = JellyfishModel.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayAnim()
    {
    }
}
