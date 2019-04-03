using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterBehavior : MonoBehaviour
{

    public GameObject m_MapLoaderPrefab = null;
    public GameObject m_StarMakerPrefab = null;
    public GameObject m_PlayerCommand;

    // UI
    public GameObject m_EventObjectPrefab;
    public GameObject m_MenuCanvas;


    private void Awake()
    {
        FadeManager.FadeIn();

        // ステージ情報を書いたテキストファイルの読み込み
        GameObject loadText = Instantiate(m_MapLoaderPrefab);
        var mapData = loadText.GetComponent<MapLoaderBehavior>().LoadMap(1, 1);

        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        starMaker.GetComponent<StarMaker>().MakeWorld(mapData);

        // Event objectを生成. UIの前に必ず生成!
        Instantiate(m_EventObjectPrefab);

        // UIオブジェクトを生成.
        GameObject menu = Instantiate(m_MenuCanvas);
        menu.SetActive(false);

        // プレイヤーコントローラを生成し,にプレイヤーキャラクターをセット.
        GameObject playerController = Instantiate(m_PlayerCommand);
        var scriptPlayerController = playerController.GetComponent<PlayerCommandBehavior>();
        scriptPlayerController.SetPlayerCharacter(GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter));
        scriptPlayerController.SetCurrentSceneMenu(menu);
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
