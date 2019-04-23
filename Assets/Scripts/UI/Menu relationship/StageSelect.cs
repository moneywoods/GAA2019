﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{

    GameObject m_ObjMageMaster;

    GameObject m_MenuCanvas;
    GameObject m_StageCanvas;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjMageMaster = GameObject.FindWithTag("SceneMaster");

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
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick(int stagenum)
    {
        GameMasterBehavior.SetStageAndChapter(stagenum);

        if (stagenum == 0) BackMenu();

        if (stagenum != 0)
        {// ステージが選択された
            PauseTheGame.SetTimeScale(1.0f);
            FadeManager.FadeOut("scene0315");
        }
    }

    // メニューを一つ前に戻す(ゲームのメインメニュー)
    private void BackMenu()
    {
        m_StageCanvas.SetActive(false);
        m_MenuCanvas.SetActive(true);
    }
}