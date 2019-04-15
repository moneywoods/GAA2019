using System;
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
            DeffaultOffset = new Vector3( -CellSize.x * CellCnt.x * 0.5f + 2.5f, CellSize.y * CellCnt.y * 0.5f + 2.5f, 0.0f) + Position;
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
    
    // マップをロードし,インスタンスを生成する.
    public void MakeWorld( char[ , ] mapData, Vector2 cellSize )
    {
        // 現在のMapInfoを更新.
        currentMapInfo = new MapInfo(mapData, cellSize, new Vector2(transform.position.x, transform.position.y));
        
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
            grid.GetComponent<GridLineBehaviour>().CurrentMapInfo = CurrentMapInfo;
        }
    }

    public void ResetWorld()
    {
        DestroyWorld();
        MakeWorld(currentMapInfo.MapData, currentMapInfo.CellSize);
    }
    public void DestroyWorld() // このスクリプトで生成した(であろう)オブジェクト達を消す.
    {
        DestroyObject("Land");
        DestroyObject("GoalStar");
        DestroyObject("BlackHole");
        DestroyObject("MilkyWay");
        DestroyObject("PlayerCharacter");

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
        Vector3 placePosition = new Vector3(currentMapInfo.CellSize.x * col, -currentMapInfo.CellSize.y * row , 0) + currentMapInfo.DeffaultOffset;
        //配置する回転角を設定
        Quaternion q = new Quaternion();
        q = Quaternion.identity;

        return Instantiate( star, placePosition, q);
    }
    
    public Vector2Int CaluculateCellPos(Vector3 position)
    {
        MapInfo mapInfo = CurrentMapInfo;
        var offset = mapInfo.DeffaultOffset;
        // セル位置を計算。
        Vector3 vec = position - offset;
        Vector2Int cellNum = new Vector2Int();
        cellNum.x = (int) Math.Round(vec.x, MidpointRounding.AwayFromZero) / (int) mapInfo.CellSize.x;
        cellNum.y = (int) Math.Round(-vec.y, MidpointRounding.AwayFromZero) / (int) mapInfo.CellSize.y;
        return cellNum;
    }

    public bool CheckLimitOfMap(Direction direction, Vector2Int startPos) // 指定した方向にマスがあるか.
    {
        if(direction == Direction.Right)
        {
            if(Check4Direction(Direction.Right, startPos))
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
            if(Check4Direction(Direction.Right, startPos) && Check4Direction(Direction.Top, startPos))
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
            if(Check4Direction(Direction.Top, startPos))
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
            if(Check4Direction(Direction.Top, startPos) && Check4Direction(Direction.Left, startPos))
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
            if(Check4Direction(Direction.Left, startPos))
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
            if(Check4Direction(Direction.Left, startPos) && Check4Direction(Direction.Bottom, startPos))
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
            if(Check4Direction(Direction.Bottom, startPos))
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
            if(Check4Direction(Direction.Bottom, startPos) && Check4Direction(Direction.Right, startPos))
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

    private bool Check4Direction(Direction direction, Vector2Int startPos) // 4方向をチェック
    {
        // para0が上下左右かチェック. 一応... 
        if(direction == Direction.LeftTop ||
            direction == Direction.LeftBottom ||
            direction == Direction.RightBottom ||
            direction == Direction.RightTop)
        {
            // 斜めは関数が別.
            return CheckLimitOfMap(direction, startPos);            
        }
        
        Vector2Int mapSize = CurrentMapInfo.CellCnt;
        if(direction == Direction.Right)
        {
            if(startPos.x + 1 <= mapSize.x)
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
            if(0 <= startPos.y - 1)
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
            if(startPos.y + 1 <= mapSize.y)
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
}
