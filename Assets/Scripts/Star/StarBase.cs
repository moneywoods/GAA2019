﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBase : MonoBehaviour
{
    private Vector2Int cellNum = new Vector2Int(-1, -1);
    
    public Vector2Int CellNum // 必ずプロパティを介して値を取得してください.
    {
        get
        {
            cellNum = StarMaker.Instance.CaluculateCellPos(transform.position);
            return cellNum;
        }
        set
        {
            cellNum = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
