﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningEventMasterBehaviour : MonoBehaviour
{
    public bool isSceneEnding = false;
    private float timeElapsed;
    private float estimatedTimeOfScene = 10.0f;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(estimatedTimeOfScene < timeElapsed)
        {
            isSceneEnding = true;
        }

        if(isSceneEnding)
        {

        }
    }
}
