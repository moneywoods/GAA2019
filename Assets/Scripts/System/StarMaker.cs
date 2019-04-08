using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMaker : MonoBehaviour
{
    public class MapInfo
    {
        public MapInfo(char[,] mapData, Vector2 cellSize, Vector2 position)
        {
            MapData = mapData;
            CellCnt = new Vector2Int(mapData.GetLength(1), mapData.GetLength(0));
            CellSize = cellSize;
            Position = position;
        }
        public char[,] MapData;
        public Vector2Int CellCnt;
        public Vector2 CellSize;

        public Vector2 Position;
    }

    /* Prefab 置き場*/
    public GameObject m_LandStarPrefab;
    public GameObject m_BlackHolePrefab;
    public GameObject m_MilkyWayPrefab;
    public GameObject m_PlayerCharacterPrefab;
    public GameObject m_GoalStarPrefab;

    /* 変数 */
    static private MapInfo currentMapInfo;
    public MapInfo CurrentMapInfo
    {
        get
        {
            return currentMapInfo;
        }
        set
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
        if(grid !=null)
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

    private void DestroyObject( string tag )
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
    GameObject PlaceStar( GameObject star, uint row, uint col )
    {
        //配置する座標を設定
        Vector2 diff = new Vector2(currentMapInfo.CellSize.x * 0.5f * currentMapInfo.CellCnt.x, currentMapInfo.CellSize.y * 0.5f * currentMapInfo.CellCnt.y);
        Vector3 placePosition = new Vector3(currentMapInfo.CellSize.x * col - diff.x, -currentMapInfo.CellSize.y * row + diff.y, 0);
        //配置する回転角を設定
        Quaternion q = new Quaternion();
        q = Quaternion.identity;

        return Instantiate( star, placePosition, q);
    }
}
