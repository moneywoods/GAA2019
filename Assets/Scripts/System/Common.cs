using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTag
{
    public static readonly string Land            = "Land";
    public static readonly string Neighvor        = "NeighvorFinder";
    public static readonly string GoalStar        = "GoalStar";
    public static readonly string StarMaker       = "StarMaker";
    public static readonly string BlackHole       = "BlackHole";
    public static readonly string MilkyWay        = "MilkyWay";
    public static readonly string PlayerCharacter = "PlayerCharacter";
    public static readonly string AbstructEffect  = "AbstructEffect";
    public static readonly string MenuCanvas      = "MenuCanvas";
    public static readonly string MenuBotton      = "MenuBotton";
    public static readonly string SceneMaster     = "SceneMaster";
    public static readonly string GridLine        = "GridLine";
    public static readonly string CellCollider    = "CellCollider";
    public static readonly string Rock            = "Rock";
}

public struct StageInfo
{
    public StageInfo( int stageNum, int chapterNum )
    {
        Stage = stageNum;
        Chapter = chapterNum;
    }
    public int Stage;
    public int Chapter;
}

static class Common
{
    public static readonly Vector2 CellSize = new Vector2(5.0f, 5.0f);
    public class DiffPosInDirection
    {

    }
}

public enum Direction
{
    // Q, W, E,
    // A,    D,
    // Z, X, C
    // この順番になっているのはfor文で回したときにDirection型n * 45度で角度が出せるからです. 
    Right,
    RightTop,
    Top,
    LeftTop,
    Left,
    LeftBottom,
    Bottom,
    RightBottom,
    ENUM_MAX,
    NONE
}
