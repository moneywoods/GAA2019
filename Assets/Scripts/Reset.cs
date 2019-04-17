using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
        
    public float timeOut;
    private float timeElapsed;

     void Start()
    {
        timeOut = 1.0f;

    }

    void Update()
    {

      

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeOut)
        {
            // Do anything
            Destroy(gameObject);
            timeElapsed = 0.0f;
            
        }
    }

}
