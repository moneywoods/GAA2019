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

    private static StageInfo initiatingStage;
    public static int InitiatingChapter
    {
        get { return initiatingStage.Chapter; }
        set { initiatingStage.Chapter = value;
              IsChapterOver();
            }
    }

    [SerializeField]
    private static int STAGE_MAX;
    [SerializeField]
    private static int CHAPTER_MAX;


    private void Awake()
    {
        if (initiatingStage.Chapter == 0)
        {
            initiatingStage = new StageInfo(1, 1);
        }

        PauseTheGame.SetTimeScale(1.0f);
        FadeManager.FadeIn();

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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
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

        rangeChapter = chapter < CHAPTER_MAX || chapter > 0;
        rangeStage= stage < STAGE_MAX || stage > 0;
        bool range = rangeChapter || rangeStage;

        if (range)
        {// 正しいステージ数及びチャプター数ならば更新
            initiatingStage.Chapter = chapter;
            initiatingStage.Stage = stage;
        }else
        {
            
        }

    }
}
