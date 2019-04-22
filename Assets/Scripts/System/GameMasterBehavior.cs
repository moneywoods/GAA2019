using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterBehavior : MonoBehaviour
{
    public GameObject m_StarMakerPrefab = null;
    public GameObject m_PlayerCommand;

    // UI
    public GameObject m_MenuCanvas;
    public GameObject m_GridLinePrefab;
    private static StageInfo initiatingStage = new StageInfo(1, 1);
    public static StageInfo InitiatingStage
    {
        get { return initiatingStage; }
        set { initiatingStage = value; }
    }

    private void Awake()
    {
        PauseTheGame.SetTimeScale(1.0f);
        FadeManager.FadeIn();

        // ステージ情報を書いたテキストファイルの読み込み
        var mapData = MapLoader.LoadMap(InitiatingStage);

        // グリッド線を生成
        Instantiate(m_GridLinePrefab);

        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        StarMaker.Instance.MakeWorld(mapData, Common.CellSize);

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
