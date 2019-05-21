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

    private static StageInfo initiatingStage;
    public static int InitiatingChapter
    {
        get { return initiatingStage.Chapter; }
        set { initiatingStage.Chapter = value;
              IsChapterOver();
            }
    }
    public static StageInfo InitiatingStage
    {
        get { return initiatingStage; }
        set { initiatingStage = value; }
    }
    
    private readonly static int STAGE_MAX = 4;
    private readonly static int CHAPTER_MAX = 4;

    [SerializeField] private GameObject m_MenuCanvas;
    [SerializeField] private GameObject m_EventSystem;
    [SerializeField] private GameObject m_GridLinePrefab;

    [SerializeField] public static bool isInitiationEvent = false;


    private void Start()
    {
        if (initiatingStage.Chapter == 0)
        {
            initiatingStage = new StageInfo(1, 1);
        }

        PauseTheGame.SetTimeScale(1.0f);
        FadeManager.FadeIn();
        FadeManager.NextColor = Color.white;

        // ステージ情報を書いたテキストファイルの読み込み
        var mapData = MapLoader.LoadMap(initiatingStage);

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

        // Camera
        var camera = GameObject.FindGameObjectWithTag(ObjectTag.MainCamera);
        var cameraScript = camera.GetComponent<InGameMainCameraController>();
        // ゲームスタート時イベント有り無し
        if(isInitiationEvent)
        {
            // ゴールからスタートまで星を映すモード
            cameraScript.SetCurrentState(InGameMainCameraController.StateName.MovingFromGoalToStart);
            var takoScript = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter).GetComponent<Tako.TakoController>();
            takoScript.SetCurrentState(Tako.TakoController.StateName.CommandDisable);
        }
        else
        {
            cameraScript.SetTarget(GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter));
            cameraScript.SetCurrentState(InGameMainCameraController.StateName.Following);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ステージクリア用
    private static void IsChapterOver()
    {
        if(initiatingStage.Chapter > CHAPTER_MAX)
        {
            initiatingStage.Chapter = 1;
            initiatingStage.Stage += 1;
        }
    }

    // ステージ選択用
    public static void SetStageAndChapter(int num)
    {
        int chapter;        // チャプター
        int stage;          // ステージ
        bool rangeChapter;  // 範囲内のチャプター
        bool rangeStage;    // 範囲内のステージ

        chapter = num % 10;
        stage = num / 10;

        rangeChapter = chapter < CHAPTER_MAX && chapter > 0;
        rangeStage= stage < STAGE_MAX && stage > 0;
        bool range = rangeChapter && rangeStage;

        if (range)
        {// 正しいステージ数及びチャプター数ならば更新
            initiatingStage.Chapter = chapter;
            initiatingStage.Stage = stage;
        }else
        {
            
        }

    }
}
