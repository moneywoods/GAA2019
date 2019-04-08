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
}