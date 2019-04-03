﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandBehavior   : MonoBehaviour
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

    private GameObject m_PlayerCharacter;
    private TakoController m_PlayerScript;
    private GameObject m_CurrentSceneMenu;
    // public GameObject LandStarType;

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
        // プレイヤーキャラクター系
        if( m_PlayerCharacter == null )
        {
            FindPlayerCharacter();
        }

        if( m_PlayerScript != null )
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.Top);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.RightTop);

            }
            else if (Input.GetKey(KeyCode.D))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.Right);

            }
            else if (Input.GetKey(KeyCode.C))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.RightBottom);

            }
            else if (Input.GetKey(KeyCode.X))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.Bottom);

            }
            else if (Input.GetKey(KeyCode.Z))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.LeftBottom);

            }
            else if (Input.GetKey(KeyCode.A))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.Left);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                m_PlayerScript.MoveFromCurrentStar(Direction.LeftTop);
            }
        }

        // リセットボタン
        if( Input.GetKey( KeyCode.L ) )
        {
            GameObject starMaker = GameObject.FindWithTag(ObjectTag.StarMaker);
            if( starMaker != null )
            {
                starMaker.GetComponent<StarMaker>().ResetWorld();
            }
        }

        if( Input.GetKey( KeyCode.Escape) && m_CurrentSceneMenu != null )
        {
            var menuScript = m_CurrentSceneMenu.GetComponent<ParentMenuCanvasBehavior>();
            if( menuScript != null )
            {
                menuScript.SwitchActive();
            }
        }
    }

    public void SetPlayerCharacter( GameObject _PlayerCharacter )
    {
        m_PlayerCharacter = _PlayerCharacter;
        m_PlayerScript = m_PlayerCharacter.GetComponent<TakoController>();
    }

    public void DiscardPlayerCharacterControll()
    {
        if( m_PlayerCharacter != null)
        {
            m_PlayerCharacter = null;
        }
    }

    private void FindPlayerCharacter() // シーン中の"Player"タグのついたオブジェクトを検索し,自身にセットする(現状"Player"オブジェクトはシーン中1つとしてます).
    {
        if( m_PlayerCharacter == null )
        {
            SetPlayerCharacter( GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter) );
        }
    }
    public  void SetCurrentSceneMenu( GameObject Menu )
    {
        if( Menu.tag == ObjectTag.MenuCanvas)
        {
            m_CurrentSceneMenu = Menu;
        }
    }
}
