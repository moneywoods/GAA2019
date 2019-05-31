using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelect : MonoBehaviour
{
    GameObject m_MenuCanvas;
    GameObject m_StageCanvas;

    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.red;
        color.pressedColor = Color.blue;
        component.colors = color;

        // ParentMenuCanvasInGameの第一子
        int menuCanvas = 0;
        m_MenuCanvas = transform.root.GetChild(menuCanvas).gameObject;
        // ステージ毎に取り付けたオブジェクトの親
        int stageCanvas = 2;
        m_StageCanvas = transform.root.gameObject.transform.GetChild(stageCanvas).gameObject;

        GameObject objStage = GameObject.Find("Stage1-1");
        if (objStage)
        {
            EventSystem.current.SetSelectedGameObject(objStage);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick(int stagenum)
    {
        if(FadeManager.CheckIsFade())
        {
            return;
        }

        GameMasterBehavior.SetStageAndChapter(stagenum);

        if (stagenum == 0) BackMenu();

        if (stagenum != 0)
        {// ステージが選択された
            PauseTheGame.SetTimeScale(1.0f);
            FadeManager.BeginSetting();
            FadeManager.NextColor = Color.black;
            FadeManager.SetUnmaskImage(FadeManager.ImageIndex.STAR);
            FadeManager.AddState(FadeManager.State.UNMASK);
            FadeManager.AddState(FadeManager.State.UNMASK_BIGGER);
            FadeManager.UnmaskSize_Start = new Vector2(Screen.width * 10, Screen.height * 10);
            FadeManager.UnmaskSize_End = new Vector2(0.01f, 0.01f);
            GameMasterBehavior.isInitiationEvent = true;

            NextSceneIndex(stagenum);
        }
    }

    // メニューを一つ前に戻す(ゲームのメインメニュー)
    private void BackMenu()
    {
        m_StageCanvas.SetActive(false);
        m_MenuCanvas.SetActive(true);
    }

    // 必要ないなら消しましょう。
    private void NextSceneIndex(int num)
    {
        int chapter = num % 10;

        if (chapter == 1)
        {
            FadeManager.SceneOut("BeginingEventScene");
        }else
        {
            FadeManager.SceneOut("scene0315");
        }

    }
}
