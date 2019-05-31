using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject m_GridCylinderPrefab;
    [SerializeField] private GameObject m_GridLinePrefab;

    [SerializeField] public static bool isInitiationEvent = false;
    [SerializeField] private GameObject m_ParticleManagerPrefab;
    
    GameObject text;


    public  Material InitSkyBox;/* ! ｺｺ! */

    private void Start()
    {
        if (initiatingStage.Chapter == 0)
        {
            initiatingStage = new StageInfo(1, 1);
        }

        if(BeginningEventMasterBehaviour.skyBox == null)
        {
            RenderSettings.skybox = BeginningEventMasterBehaviour.skyBox = InitSkyBox;;
        }
        else
        {
           RenderSettings.skybox = BeginningEventMasterBehaviour.skyBox;
        }


        PauseTheGame.SetTimeScale(1.0f);
        FadeManager.BeginSetting();
        FadeManager.NextColor = Color.black;
        FadeManager.SetImage(FadeManager.ImageIndex.NONE);
        FadeManager.AddState(FadeManager.State.A_TO_ZERO);
        FadeManager.SceneIn();

        // ステージ情報を書いたテキストファイルの読み込み
        var mapData = MapLoader.LoadMap(initiatingStage);

        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        StarMaker.Instance.MakeWorld(mapData, Common.CellSize);

        // グリッド線を生成する.
        var gc = Instantiate(m_GridCylinderPrefab);
        gc.GetComponent<GridCylinderBehaviour>().Init();

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

        Instantiate(m_ParticleManagerPrefab);

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

        // GetComponent<AudioSource>().Play(); 音だすやつ


        // 土屋君、ここの"ステージ == 数値"を増やしてほしいにゃぁ。コピー版
        if (GameMasterBehavior.InitiatingStage.Stage == 1 || GameMasterBehavior.InitiatingStage.Stage == 0)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
        }
        else if (GameMasterBehavior.InitiatingStage.Stage == 2)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround 2", LoadSceneMode.Additive);
        }
        else if (GameMasterBehavior.InitiatingStage.Stage == 3)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround 3", LoadSceneMode.Additive);
        }
        else if (GameMasterBehavior.InitiatingStage.Stage == 4)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround 4", LoadSceneMode.Additive);
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

        rangeChapter = chapter <= CHAPTER_MAX && chapter > 0;
        rangeStage= stage <= STAGE_MAX && stage > 0;
        bool range = rangeChapter && rangeStage;

        if (range)
        {// 正しいステージ数及びチャプター数ならば更新
            initiatingStage.Chapter = chapter;
            initiatingStage.Stage = stage;
        }else
        {

        }
    }

    public static bool EndingGame()
    {
        int GAME_END = 5;

        if(InitiatingStage.Stage == GAME_END)
        {
            InitiatingStage = new StageInfo(1, 1);
            return true;
        }
        else
        {
            return false;
        }

    }
}