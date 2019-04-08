using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameMenuControl : MonoBehaviour
{
    ParentMenuCanvasBehavior m_MenuDelete;      // メニューデリート用

    GameObject m_ObjPlayerCmdBhv;

    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.green;
        color.pressedColor = Color.blue;

        component.colors = color;

        m_MenuDelete = transform.root.gameObject.GetComponent<ParentMenuCanvasBehavior>();
        m_ObjPlayerCmdBhv = GameObject.FindWithTag("PlayerCommand");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (gameObject.name == "ReturnToGame")
        {// ゲームに戻る
            PauseTheGame.GameReStart();
            m_MenuDelete.SwitchActive();
        }
        if (gameObject.name == "RestartStage")
        {// ステージをやり直す
            PauseTheGame.GameReStart();
            m_ObjPlayerCmdBhv.GetComponent<PlayerCommandBehavior>().RedoTheStage();
            m_MenuDelete.SwitchActive();
        }
        if (gameObject.name == "SelectStage")
        {// ステージ選択

        }
        if (gameObject.name == "ControllerLayout")
        {// 操作説明

        }
        if (gameObject.name == "BackToTitle")
        {// タイトルに戻る
            PauseTheGame.GameReStart();
            FadeManager.FadeOut("TitleScene");
        }
    }

    //==================
    // ボタンが選ばれている
    //==================
    public void OnSelect()
    {
//        Button button = GetComponent<Button>();
//        var color = button.colors;
//        color.highlightedColor = Color.red;
//        button.colors = color;

        var component = GetComponent<Button>();
        var color = component.colors;
//        color.normalColor = Color.white;
        color.highlightedColor = Color.yellow;
//        color.pressedColor = Color.blue;

        component.colors = color;

    }

    //==================
    // ボタンから離れた
    //==================
    public void OnDeselct()
    {
        Button button = GetComponent<Button>();
        var color = button.colors;
        color.highlightedColor = Color.white;

        button.colors = color;        
    }

}
