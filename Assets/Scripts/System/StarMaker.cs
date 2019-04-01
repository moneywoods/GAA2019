using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMaker : MonoBehaviour
{
    /* Prefab 置き場*/
    public GameObject landStar;
    public GameObject blackHole;
    public GameObject milkyWay;
    public GameObject PlayerCharacter;
    public GameObject goalStar;




    public float marginX; // 一コマ当たりの幅.
    public float marginY; // 一コマ当たりの高さ.

    private string [,] m_CurrentMapData;
    // ステージを作るためにテキストファイル情報読み込み
    private void LoadStage()
    {

    }

    private void Awake()
    {

    }

    // マップをロードし,インスタンスを生成する.
    public void MakeWorld( string[ , ] mapData )
    {
        m_CurrentMapData = mapData; // 現在のマップを保持.

        int cntStart = 0; // スタート地点が複数個設置されていないかチェックするため.

        int row = mapData.GetLength(0);
        int col = mapData.GetLength(1);

        // マップに配置
        for ( uint rc = 0; rc < row; rc++ )
        {
            for (uint cc = 0; cc < col; cc++)
            {
                if (mapData[rc, cc] == "L")
                {
                    // プレイヤーが乗れる星.
                    PlaceStar(landStar, rc, cc).GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                }
                else if (mapData[rc, cc] == "S")
                {
                    // プレイヤーのスタートする星.
                    // 現状はTakoを置いてます.
                    // 星を生成
                    GameObject startingStar = PlaceStar(landStar, rc, cc);
                    startingStar.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                    LandStarController script = startingStar.GetComponent<LandStarController>();
                    script.AddStat(LandStarController.LANDSTAR_STAT.PLAYER_STAYING);
                    // 生成した星にプレイヤーを配置
                    GameObject player = Instantiate(PlayerCharacter);
                    script.ArriveThisLand(player);
                    cntStart++;
                }
                else if (mapData[rc, cc] == "B")
                {
                    // ブラックホール.
                    PlaceStar(blackHole, rc, cc);
                }
                else if (mapData[rc, cc] == "M")
                {
                    // 乳.
                    PlaceStar(milkyWay, rc, cc);
                }
                else if (mapData[rc, cc] == "G")
                {
                    // ゴールの星.
                    GameObject star = PlaceStar(goalStar, rc, cc);
                    star.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.ALIVE);
                }
                else if(mapData[rc, cc] == "W")
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

        // プレイヤーコントローラにプレイヤーキャラクターをセット. // ここでやること？ほかでやること？
        GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerCommandBehavior>().SetPlayerCharacter(GameObject.FindGameObjectWithTag("PlayerCharacter"));

    }

    public void ResetWorld()
    {
        DestroyWorld();
        MakeWorld(m_CurrentMapData);
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
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        
    }
    // Place Stars
    GameObject PlaceStar( GameObject star, uint row, uint col )
    {
        //配置する座標を設定
        Vector3 placePosition = new Vector3(marginX * col, -marginY * row, 0);
        //配置する回転角を設定
        Quaternion q = new Quaternion();
        q = Quaternion.identity;

        return Instantiate( star, placePosition, q);
    }
}
