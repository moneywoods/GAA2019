using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBase : MyGameObject
{
    [Flags]
    public enum StarType
    {
        Land            = 1 << 0, // 0000_0001
        GoalStar        = 1 << 1, // 0000_0010
        BlackHole       = 1 << 2, // 0000_0100
        MilkyWay        = 1 << 3, // 0000_1000
        Rock            = 1 << 4, // 0001_0000
    }
    public StarType starType
    {
        get;
        protected set;
    }

    public StarBase(StarType type)
    {
        this.starType = type;
    }

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
