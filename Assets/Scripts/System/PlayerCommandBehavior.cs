﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandBehavior : MonoBehaviour
{
    private GameObject m_CurrentSceneMenu;
    [SerializeField] private GameObject ResetSpritePrefab;

    [SerializeField]
    private GameObject m_ObjStageCanvas;
    private GameObject m_ObjStageSelect;

    GameObject m_ObjMenuCanvas;

    readonly int STAGESELECT = 2;
    readonly int CANVASMENU = 0;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ObjStageSelect = m_ObjStageCanvas.transform.GetChild(STAGESELECT).gameObject;
        m_ObjMenuCanvas = m_ObjStageCanvas.transform.GetChild(CANVASMENU).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ResetButton();
        StartButton();
    }
    
    public void SetCurrentSceneMenu(GameObject Menu)
    {
        if( Menu.tag == ObjectTag.MenuCanvas )
        {
            m_CurrentSceneMenu = Menu;
        }
    }

    public void RedoTheStage()
    {
        GameObject starMaker = GameObject.FindWithTag(ObjectTag.StarMaker);
        if (starMaker != null)
        {
            starMaker.GetComponent<StarMaker>().ResetWorld();
        }
    }

    // リセットボタン（キーボードのみ）
    private void ResetButton()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject starMaker = GameObject.FindWithTag(ObjectTag.StarMaker);
            if (starMaker != null)
            {
                Instantiate(ResetSpritePrefab);
                StarMaker.Instance.ResetWorld();
            }
        }
    }

    // スタートボタン
    private void StartButton()
    {
        /* ----- ゲームパッド用ボタン番号 ----- */
        bool startButton = Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape);      // STARTボタン
        

        if (startButton && m_CurrentSceneMenu != null)
        {
            GameObject stageSelect = m_ObjStageCanvas.transform.GetChild(STAGESELECT).gameObject;
            GameObject canvasMenu = m_ObjStageCanvas.transform.GetChild(CANVASMENU).gameObject;
            bool menuActive = canvasMenu.activeSelf;       // メニューがアクティブかどうかの判定
            bool stageSelectActive = stageSelect.activeSelf;
            bool returnFrag = menuActive && FadeManager.CheckIsFade() && stageSelectActive;
            Debug.Log("canvasMenu.activeSelf =" + canvasMenu.activeSelf);
            Debug.Log("canvasMenu.activeInHierarchy =" + canvasMenu.activeInHierarchy);
            Debug.Log("stageSelect.activeSelf =" + stageSelect.activeSelf);
            Debug.Log("stageSelect.activeInHierarchy =" + stageSelect.activeInHierarchy);

            //            if (!menuActive) return;
            //            if (FadeManager.CheckIsFade()) return;
            //            if (m_ObjStageSelect.activeSelf) return;
            if (returnFrag) return;

            var menuScript = m_CurrentSceneMenu.GetComponent<ParentMenuCanvasBehavior>();
            if (menuScript != null)
            {
                menuScript.SwitchActive();
            }
        }
    }

}