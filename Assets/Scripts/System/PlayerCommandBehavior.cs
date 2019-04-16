﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandBehavior : MonoBehaviour
{
    public enum Direction
    {
        // Q, W, E,
        // A,    D,
        // Z, X, C
        // この順番になっているのはfor文で回したときにDirection型n * 45度で角度が出せるからです. 
        Right,
        RightTop,
        Top,
        LeftTop,
        Left,
        LeftBottom,
        Bottom,
        RightBottom,
        ENUM_MAX,
        NONE
    }

    private GameObject m_CurrentSceneMenu;
    [SerializeField] private GameObject ResetSpritePrefab;


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
        /* ----- ゲームパッド用ボタン番号 ----- */
        bool startButton = Input.GetKeyDown(KeyCode.Joystick1Button7);      // STARTボタン

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

        if( (Input.GetKeyDown(KeyCode.Escape) || startButton) && m_CurrentSceneMenu != null )
        {
            var menuScript = m_CurrentSceneMenu.GetComponent<ParentMenuCanvasBehavior>();
            if( menuScript != null )
            {
                PauseTheGame.GamePauseSwitch();
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
}