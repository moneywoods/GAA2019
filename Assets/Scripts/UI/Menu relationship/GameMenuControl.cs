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

    [SerializeField]
    public float m_Rotation;


    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.red;
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
    // ボタンが選ばれた
    //==================
    public void OnSelect()
    {
    }

    //==================
    // ボタンが選ばれている
    //==================
    public void UpdateSelected()
    {
//        Quaternion rotation = gameObject.transform.rotation;
//
//        rotation.y += m_Rotation;
//        gameObject.transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
//        
    }
    
    //==================
    // ボタンから離れた
    //==================
    public void OnDeselct()
    {
        gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }

}
