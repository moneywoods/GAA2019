using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAnim : MonoBehaviour
{
    private Animator animator;
    private GameObject SharkModel;

    private class AnimationFlagName
    {
        public static string flagIsEat = "isEat";
        public static string[] flagArray =
        {
                flagIsEat,          //ジャンプ
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        SharkModel = gameObject;
        animator = SharkModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnim(string targetName)
    {
        for (int i = 0; i < AnimationFlagName.flagArray.GetLength(0); i++)
        {
            if (AnimationFlagName.flagArray[i] == targetName)
            {
                animator.SetBool(AnimationFlagName.flagArray[i], true);
                Debug.Log(AnimationFlagName.flagArray[i] + " is true");
            }
            else
            {
                animator.SetBool(AnimationFlagName.flagArray[i], false);
                Debug.Log(AnimationFlagName.flagArray[i] + " is false");
            }
        }
    }
}
