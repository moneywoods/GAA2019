﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandBehavior : MonoBehaviour
{
    private GameObject m_CurrentSceneMenu;
    [SerializeField] private GameObject ResetSpritePrefab;

    [SerializeField]
    private GameObject m_ObjStageCanvas;
    private GameObject m_ObjStageSelect;


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        m_ObjStageSelect = m_ObjStageCanvas.transform.GetChild(2).gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        /* ----- ゲームパッド用ボタン番号 ----- */
        bool startButton = Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape);      // STARTボタン

        // リセットボタン
        if( Input.GetKeyDown(KeyCode.L) )
        {
            GameObject starMaker = GameObject.FindWithTag(ObjectTag.StarMaker);
            if( starMaker != null )
            {
                Instantiate(ResetSpritePrefab);
                starMaker.GetComponent<StarMaker>().ResetWorld();
            }
        }

        if (startButton && m_CurrentSceneMenu != null)
        {
            if (FadeManager.CheckIsFade()) return;
            if (m_ObjStageSelect.activeSelf) return;

            var menuScript = m_CurrentSceneMenu.GetComponent<ParentMenuCanvasBehavior>();
            if( menuScript != null )
            {
                menuScript.SwitchActive();
            }
        }
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
}