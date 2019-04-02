using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializerBehavior : MonoBehaviour
{
    public GameObject m_MapLoaderPrefab = null;
    public GameObject m_StarMakerPrefab = null;
    public GameObject m_PlayerCommand = null;


    private void Awake()
    {
        // ステージ情報を書いたテキストファイルの読み込み
        GameObject loadText = Instantiate(m_MapLoaderPrefab);
        var mapData = loadText.GetComponent<MapLoaderBehavior>().LoadMap( 1, 2 );
        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        starMaker.GetComponent<StarMaker>().MakeWorld(mapData);

        GameObject pc = Instantiate(m_PlayerCommand);
        pc.GetComponent<PlayerCommandBehavior>().SetPlayerCharacter(GameObject.FindGameObjectWithTag("PlayerCharacter"));

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
