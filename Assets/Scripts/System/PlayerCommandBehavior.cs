﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandBehavior : MonoBehaviour
{
    private GameObject m_CurrentSceneMenu;
    [SerializeField] private GameObject ResetSpritePrefab;
    
    private GameObject m_ObjStageSelect;

    GameObject m_ObjMenuCanvas;
    

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
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
            bool returnFrag = FadeManager.CheckIsFade();            
            if (returnFrag) return;

            var menuScript = m_CurrentSceneMenu.GetComponent<ParentMenuCanvasBehavior>();
            if (menuScript != null)
            {
                menuScript.SwitchActive();
            }
        }
    }

}