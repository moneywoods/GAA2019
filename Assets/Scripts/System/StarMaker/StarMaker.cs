﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMaker : SingletonPattern<StarMaker>
{
    public class MapInfo
    {
        public MapInfo(char[,] mapData, Vector3 cellSize, Vector3 position)
        {
            MapData = mapData;
            CellCnt = new Vector2Int(mapData.GetLength(1), mapData.GetLength(0));
            CellSize = cellSize;
            Position = position;
            DeffaultOffset = new Vector3( -CellSize.x * CellCnt.x * 0.5f + CellSize.x * 0.5f, 0.0f, CellSize.y * CellCnt.y * 0.5f + CellSize.y * 0.5f) + Position;
        }

        public char[,] MapData;  // マップの初期配置
        public Vector2Int CellCnt; // マスの列数,行数
        public Vector2 CellSize; // 1マスのサイズ

        public Vector3 Position; // マップの中心座標
        public Vector3 DeffaultOffset; // 星の座標のオフセット
    }

    /* Prefab 置き場*/
    public GameObject m_LandStarPrefab;
    public GameObject m_BlackHolePrefab;
    public GameObject m_MilkyWayPrefab;
    public GameObject m_PlayerCharacterPrefab;
    public GameObject m_GoalStarPrefab;
    public GameObject m_CellCollider;

    /* 変数 */
    private MapInfo currentMapInfo;
    public MapInfo CurrentMapInfo
    {
        get
        {
            return currentMapInfo;
        }
        private set
        {
            currentMapInfo = value;
        }
    }
    private GameObject[,] Cell;

    public CellColliderBehaviour[,] CellColliderBehaviourScript
    {
        get;
        private set;
    }

    // マップをロードし,インスタンスを生成する.
    public void MakeWorld( char[ , ] mapData, Vector2 cellSize )
    {
        // 現在のMapInfoを更新.
        currentMapInfo = new MapInfo(mapData, cellSize, new Vector2(transform.position.x, transform.position.y));

        // コマの当たり判定オブジェクトの生成とBehaviourへのアクセスを取得.
        Cell = new GameObject[CurrentMapInfo.CellCnt.y, CurrentMapInfo.CellCnt.x];
        CellColliderBehaviourScript = new CellColliderBehaviour[CurrentMapInfo.CellCnt.y, CurrentMapInfo.CellCnt.x];

        for(uint cy = 0; cy < CurrentMapInfo.CellCnt.y; cy++)
        {
            for(uint cx = 0; cx < CurrentMapInfo.CellCnt.x; cx++)
            {
                Cell[cy, cx] = PlaceStar(m_CellCollider, cy, cx);
                Cell[cy, cx].transform.parent = transform;
                Cell[cy, cx].name += "(" + cx.ToString() + "," + cy.ToString() + ")";
                CellColliderBehaviourScript[cy, cx] = Cell[cy, cx].GetComponent<CellColliderBehaviour>();
            }
        }

        int cntStart = 0; // スタート地点が複数個設置されていないかチェックするため.

        // マップに配置
        for ( uint rc = 0; rc < currentMapInfo.CellCnt.y; rc++ )
        {
            for (uint cc = 0; cc < currentMapInfo.CellCnt.x; cc++)
            {
                if (mapData[rc, cc] == 'L')
                {
                    // プレイヤーが乗れる星.
                    PlaceStar(m_LandStarPrefab, rc, cc).GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                }
                else if (mapData[rc, cc] == 'S')
                {
                    // プレイヤーのスタートする星.
                    // 現状はTakoを置いてます.
                    // 星を生成
                    GameObject startingStar = PlaceStar(m_LandStarPrefab, rc, cc);
                    startingStar.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                    LandStarController script = startingStar.GetComponent<LandStarController>();
                    script.AddStat(LandStarController.LANDSTAR_STAT.PLAYER_STAYING);
                    // 生成した星にプレイヤーを配置
                    GameObject player = Instantiate(m_PlayerCharacterPrefab);
                    script.ArriveThisLand(player);
                    cntStart++;
                }
                else if (mapData[rc, cc] == 'B')
                {
                    // ブラックホール.
                    PlaceStar(m_BlackHolePrefab, rc, cc);
                }
                else if (mapData[rc, cc] == 'M')
                {
                    // 乳.
                    PlaceStar(m_MilkyWayPrefab, rc, cc);
                }
                else if (mapData[rc, cc] == 'G')
                {
                    // ゴールの星.
                    GameObject star = PlaceStar(m_GoalStarPrefab, rc, cc);
                    star.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                }
                else if(mapData[rc, cc] == 'I')
                {
                    // ミルキーウェイに入ったLand
                    PlaceStar(m_MilkyWayPrefab, rc, cc);
                    GameObject star = PlaceStar(m_LandStarPrefab, rc, cc);
                    star.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                    star.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.CAUGHT_BY_MILKYWAY);
                    CellColliderBehaviourScript[rc, cc].AddManually(star);

                }
                else if(mapData[rc, cc] == 'W')
                {
                    // 壁用のオブジェクト
                    // 実装これから
                }
                if( 1 < cntStart) // スタート地点が複数個セットされてたら通知.
                {
                    Debug.Log("!!!!!!!!!!!!!!Multiple start planet has been detected.");
                }

            }
        }
        
        // グリッドを調整
        GameObject grid = GameObject.FindWithTag(ObjectTag.GridLine);
        if(grid != null)
        {
            grid.GetComponent<GridLineBehaviour>().CurrentMapInfo = this.CurrentMapInfo;
        }
    }

    public void ResetWorld()
    {
        FadeManager.BeginSetting();
        FadeManager.NextColor = Color.black;
        FadeManager.SetUnmaskImage(FadeManager.ImageIndex.STAR);
        FadeManager.AddState(FadeManager.State.UNMASK);
        FadeManager.AddState(FadeManager.State.UNMASK_BIGGER);
        FadeManager.UnmaskSize_Start = new Vector2(Screen.width * 10, Screen.height * 10);
        FadeManager.UnmaskSize_End = new Vector2(0.01f, 0.01f);
        GameMasterBehavior.isInitiationEvent = false;
        FadeManager.SceneOut("scene0315");
    }

    public void DestroyWorld() // このスクリプトで生成した(であろう)オブジェクト達を消す.
    {
        DestroyObject(ObjectTag.Land);
        DestroyObject(ObjectTag.GoalStar);
        DestroyObject(ObjectTag.BlackHole);
        DestroyObject(ObjectTag.MilkyWay);
        DestroyObject(ObjectTag.PlayerCharacter);
        DestroyObject(ObjectTag.CellCollider);
    }

    private void DestroyObject(string tag)
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag(tag);

        foreach( GameObject obj in array )
        {
            Destroy(obj);
        }
    }

    public Vector2Int GetCurrentCellSize()
    {
        return currentMapInfo.CellCnt;
    }
    
    // Place Stars
    GameObject PlaceStar(GameObject star, uint row, uint col)
    {
        //配置する座標を設定
        Vector3 placePosition = new Vector3(currentMapInfo.CellSize.x * col, 0.0f, -currentMapInfo.CellSize.y * row) + currentMapInfo.DeffaultOffset;
        //配置する回転角を設定
        Quaternion q = new Quaternion();
        q = Quaternion.identity;

        return Instantiate( star, placePosition, q);
    }
    
    public Vector2Int CaluculateCellNum(Vector3 position)
    {
        MapInfo mapInfo = CurrentMapInfo;
        var offset = mapInfo.DeffaultOffset;
        // セル位置を計算。
        Vector3 vec = position - offset;
        Vector2Int cellNum = new Vector2Int();
        cellNum.x = (int) Math.Round(vec.x, MidpointRounding.AwayFromZero) / (int) mapInfo.CellSize.x;
        cellNum.y = (int) Math.Round(-vec.z, MidpointRounding.AwayFromZero) / (int) mapInfo.CellSize.y;
        return cellNum;
    }

    public Vector3 GetCenterPositionOfCell(Vector2Int cellNum)
    {
        return new Vector3(currentMapInfo.CellSize.x * cellNum.x, 0.0f, -currentMapInfo.CellSize.y * cellNum.y) + currentMapInfo.DeffaultOffset;
    }

    public bool CheckLimitOfMap(Vector2Int cellNum)
    {
        var cellCnt = CurrentMapInfo.CellCnt;

        if(cellNum.x < cellCnt.x && 0 <= cellNum.x && 
            cellNum.y < cellCnt.y && 0 <= cellNum.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
    public bool CheckLimitOfMap(Vector2Int startPos, Direction direction) // 指定した方向にマスがあるか.
    {
        if(direction == Direction.Right)
        {
            if(Check4Direction(startPos, Direction.Right))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.RightTop)
        {
            if(Check4Direction(startPos, Direction.Right) && Check4Direction(startPos, Direction.Top))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.Top)
        {
            if(Check4Direction(startPos, Direction.Top))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.LeftTop)
        {
            if(Check4Direction(startPos, Direction.Top) && Check4Direction(startPos, Direction.Left))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.Left)
        {
            if(Check4Direction(startPos, Direction.Left))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.LeftBottom)
        {
            if(Check4Direction(startPos, Direction.Left) && Check4Direction(startPos, Direction.Bottom))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.Bottom)
        {
            if(Check4Direction(startPos, Direction.Bottom))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.RightBottom)
        {
            if(Check4Direction(startPos, Direction.Bottom) && Check4Direction(startPos, Direction.Right))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool Check4Direction(Vector2Int startPos, Direction direction) // 4方向をチェック
    {
        // para0が上下左右かチェック. 一応... 
        if(direction == Direction.LeftTop ||
            direction == Direction.LeftBottom ||
            direction == Direction.RightBottom ||
            direction == Direction.RightTop)
        {
            // 斜めは関数が別.
            return CheckLimitOfMap(startPos, direction);            
        }
        
        Vector2Int mapSize = CurrentMapInfo.CellCnt;
        if(direction == Direction.Right)
        {
            if(startPos.x + 1 < mapSize.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.Top)
        {
            if(0 <= startPos.y - 1 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.Left)
        {
            if(0 <= startPos.x - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(direction == Direction.Bottom)
        {
            if(startPos.y + 1 < mapSize.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public GameObject GetStar(Vector2Int cellNum, string tag) // 指定したコマのリストから指定したタグの星を戻す.
    {
        if(!CheckLimitOfMap(cellNum))
        {
            return null;
        }
        else
        {
            // null
        }

        return CellColliderBehaviourScript[cellNum.y, cellNum.x].List.Find(obj => obj.tag == tag.ToString());
    }

    public GameObject GetStar(Vector2Int cellNum, string tag, Direction direction)
    {
        cellNum += GetDifferenceByDirection(direction);

        return GetStar(cellNum, tag);
    }

    public GameObject GetStar(Vector2Int cellNum, StarBase.StarType type)
    {
        if(!CheckLimitOfMap(cellNum))
        {
            return null;
        }
        else
        {
            // null
        }
        
        // StarBaseサブクラス以外にも入っているので取り除く.
        var list = new List<GameObject>(GetStarList(cellNum));
        var removingList = list.FindAll(obj => obj.GetComponent<MyGameObject>().objectType != MyGameObject.ObjectType.Star);

        foreach(GameObject obj in removingList)
        {
            list.Remove(obj);
        }
        
        return list.Find(obj => (obj.GetComponent<StarBase>().starType & type) != 0);
    }

    public GameObject GetStar(Vector2Int cellNum, StarBase.StarType type, Direction direction)
    {
        cellNum += GetDifferenceByDirection(direction);

        return GetStar(cellNum, type);
    }

    public List<GameObject> GetStarList(Vector2Int cellNum)
    {
        if(!CheckLimitOfMap(cellNum))
        {
            return new List<GameObject>();
        }
        else
        {
            // null
        }

        return Cell[cellNum.y, cellNum.x].GetComponent<CellColliderBehaviour>().List;
    }

    public List<GameObject> GetStarList(Vector2Int cellNum, StarBase.StarType type)
    {
        if(!CheckLimitOfMap(cellNum))
        {
            return new List<GameObject>();
        }
        else
        {

        }

        var list = new List<GameObject>(GetStarList(cellNum));
        var removingList = list.FindAll(obj => obj.GetComponent<MyGameObject>().objectType != MyGameObject.ObjectType.Star);

        foreach(GameObject obj in removingList)
        {
            list.Remove(obj);
        }
        
        return list.FindAll(obj => (obj.GetComponent<StarBase>().starType & type) != 0);
    }

    public List<GameObject> GetStarList(Vector2Int cellNum, StarBase.StarType type, Direction direction)
    {
        cellNum += GetDifferenceByDirection(direction);

        return GetStarList(cellNum, type, direction);
    }

    public List<GameObject> GetStarList(Vector2Int cellNum, Direction direction) // 指定された方向のマスにある星を取得する. 指定された方向がマップの端を超える場合nullを戻す.
    {
        // マップ限界のチェック
        if(!CheckLimitOfMap(cellNum, direction))
        {
            return new List<GameObject>();
        }
        else
        {
            // null
        }

        // 指定された方向のコマを戻す.
        if(direction == Direction.Right)
        {
            return Cell[cellNum.y, cellNum.x + 1].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.RightTop)
        {
            return Cell[cellNum.y - 1, cellNum.x + 1].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.Top)
        {
            return Cell[cellNum.y - 1, cellNum.x].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.LeftTop)
        {
            return Cell[cellNum.y - 1, cellNum.x - 1].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.Left)
        {
            return Cell[cellNum.y, cellNum.x - 1].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.LeftBottom)
        {
            return Cell[cellNum.y + 1, cellNum.x - 1].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.Bottom)
        {
            return Cell[cellNum.y + 1, cellNum.x].GetComponent<CellColliderBehaviour>().List;
        }
        else if(direction == Direction.RightBottom)
        {
            return Cell[cellNum.y + 1, cellNum.x + 1].GetComponent<CellColliderBehaviour>().List;
        }
        else
        {
            return null;
        }
    }

    public CellColliderBehaviour GetCellColliderBehavior(Vector2Int cellNum)
    {
        if(!CheckLimitOfMap(cellNum))
        {
            return null;
        }
        return Cell[cellNum.y, cellNum.x].GetComponent<CellColliderBehaviour>();
    }

    public List<GameObject> GetNeighvorList(Vector2Int cellNum)
    {
        var list = new List<GameObject>(); // 戻すリスト

        foreach(Direction value in Enum.GetValues(typeof(Direction)))
        {
            if(value == Direction.ENUM_MAX || value == Direction.NONE)
            {
                return list;
            }
            var tmpList = GetStarList(cellNum, value);

            // マップの端を超えてしていた場合nullが戻されるので.
            if(tmpList != null)
            {
                list.AddRange(tmpList);
            }
        }
        return list;
    }

    public static Direction GetDirection(Vector2Int origin, Vector2Int target) // targetがoriginの周囲1マスにない場合,NONEを戻すことに注意.
    {
        var diff = target - origin;
        if(diff == new Vector2Int(1, 0))
        {
            return Direction.Right;
        }
        else if(diff == new Vector2Int(1, -1))
        {
            return Direction.RightTop;
        }
        else if(diff == new Vector2Int(0, -1))
        {
            return Direction.Top;
        }
        else if(diff == new Vector2Int(-1, -1))
        {
            return Direction.LeftTop;
        }
        else if(diff == new Vector2Int(-1, 0))
        {
            return Direction.Left;
        }
        else if(diff == new Vector2Int(-1, 1))
        {
            return Direction.LeftBottom;
        }
        else if(diff == new Vector2Int(0, 1))
        {
            return Direction.Bottom;
        }
        else if(diff == new Vector2Int(1, 1))
        {
            return Direction.RightBottom;
        }
        else
        {
            return Direction.NONE;
        }
    }

    public static Vector2Int GetDifferenceByDirection(Direction direction)
    {
        int x = 0;
        int y = 0;

        // 左右判定
        if(direction == Direction.Right || direction == Direction.RightTop || direction == Direction.RightBottom)
        {
            x = 1;
        }
        else if(direction == Direction.Left || direction == Direction.LeftTop || direction == Direction.LeftBottom)
        {
            x = -1;
        }
        else
        {
            // null
        }

        // 上下判定
        if(direction == Direction.Top || direction == Direction.RightTop || direction == Direction.LeftTop)
        {
            y = -1;
        }
        else if(direction == Direction.Bottom || direction == Direction.RightBottom || direction == Direction.LeftBottom)
        {
            y = 1;
        }
        else
        {
            // null
        }

        return new Vector2Int(x, y);
    }
}
