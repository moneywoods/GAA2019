﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleMasterBehavior : MonoBehaviour
{
    public GameObject m_ParentCanvasPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // フェードイン
        FadeManager.FadeIn();

        // UI objectを生成.
        GameObject menu = Instantiate(m_ParentCanvasPrefab);
        menu.GetComponent<ParentMenuCanvasBehavior>().SetActivateSelectionCursor();

        // 背景用のシーン読込
        SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
    }
}
