using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMasterBehavior : MonoBehaviour
{
    [SerializeField] private GameObject m_StarMakerPrefab = null;
    [SerializeField] private GameObject m_PlayerCommand;

    // UI
    [SerializeField] private GameObject m_MenuCanvas;
    [SerializeField] private GameObject m_EventSystem;
    [SerializeField] private GameObject m_GridLinePrefab;
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
