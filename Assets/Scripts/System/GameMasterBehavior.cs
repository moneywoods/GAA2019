using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterBehavior : MonoBehaviour
{
    public GameObject m_StarMakerPrefab = null;
    public GameObject m_PlayerCommand;

    // UI
    public GameObject m_EventObjectPrefab;
    public GameObject m_MenuCanvas;
    public GameObject m_GridLinePrefab;
    public static StageInfo InitiatingStage = new StageInfo(1, 1);

    private void Awake()
    {
        FadeManager.FadeIn();

        // ステージ情報を書いたテキストファイルの読み込み
        var mapData = MapLoader.LoadMap(InitiatingStage);

        // グリッド線を生成
        Instantiate(m_GridLinePrefab);

        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        starMaker.GetComponent<StarMaker>().MakeWorld(mapData, Common.CellSize);


        // Event objectを生成. UIの前に必ず生成!
        Instantiate(m_EventObjectPrefab);

        // UIオブジェクトを生成.
        GameObject menu = Instantiate(m_MenuCanvas);
        menu.SetActive(false);

        // プレイヤーコントローラを生成し,にプレイヤーキャラクターをセット.
        GameObject playerController = Instantiate(m_PlayerCommand);
        var scriptPlayerController = playerController.GetComponent<PlayerCommandBehavior>();
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
