using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializerBehavior : MonoBehaviour
{
    public GameObject m_LoadText = null;
    public GameObject m_StarMakerPrefab = null;


    // マップ
    // S = スタート地点, L = 降りられる惑星, B = ブラックホール, M = 乳, W = 壁, 0 = 何もなし.
    readonly string[,] initMap = new string[8, 7] {    { "W", "W", "W", "W", "W", "W", "W" },
                                                       { "W", "0", "0", "B", "0", "G", "W" },
                                                       { "W", "L", "0", "L", "0", "0", "W" },
                                                       { "W", "0", "S", "B", "0", "0", "W" },
                                                       { "W", "0", "0", "M", "0", "0", "W" },
                                                       { "W", "B", "0", "0", "0", "0", "W" },
                                                       { "W", "0", "0", "B", "0", "0", "W" },
                                                       { "W", "W", "W", "W", "W", "W", "W" } };

    private void Awake()
    {
        // ステージ情報を書いたテキストファイルの読み込み
        GameObject loadText = Instantiate(m_LoadText);
        //loadText.GetComponent<LoadText>().ReadText();
        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        starMaker.GetComponent<StarMaker>().MakeWorld(initMap);
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
