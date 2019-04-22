using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBase : MyGameObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool CheckKineticPowerCanBeUsed(Vector2Int originCellNum, bool isRight)
    {
        return true;
    }
}
