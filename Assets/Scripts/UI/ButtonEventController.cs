using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ButtonEventController : MonoBehaviour
{
    private Text text;

    private ParentMenuCanvasBehavior m_MenuDelete;
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.red;
        color.pressedColor = Color.blue;

        component.colors = color;
        text = transform.root.GetComponent<Text>();

        m_MenuDelete = transform.root.gameObject.GetComponent<ParentMenuCanvasBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }

// ----------------------------------------------------------------------------------
//
// Event Trigger
//
// ----------------------------------------------------------------------------------
public void OnSelected()
    {

    }

    public void OnDeseleccted()
    {

    }

    public void OnClick()
    {

        /* ----- タイトル画面のメニュー ----- */
        if (gameObject.name == "BeginGame")
        {// 初めから
            FadeManager.NextColor = Color.clear;
            FadeManager.AddState(FadeManager.State.A_TO_ONE);
            FadeManager.SceneOut("scene0315");
        }
        if (gameObject.name == "ContinueGame")
        {// 続きから

        }
        if (gameObject.name == "SelectStage")
        {// ステージ選択

        }
        if (gameObject.name == "End")
        {// ゲーム終了

        }

        /* ----- ゲーム画面のメニュー ----- */
        if (gameObject.name == "ReturnToGame")
        {// ゲームに戻る
            PauseTheGame.SetTimeScale(1.0f);         // 一時停止解除
            m_MenuDelete.SwitchActive();        // メニューを消す
        }
        if (gameObject.name == "SelectStage")
        {// ステージ選択

        }
        if(gameObject.name == "ControllerLayout")
        {// 操作説明

        }
        if(gameObject.name == "BackToTitle")
        {// タイトルに戻る
            PauseTheGame.SetTimeScale(1.0f);
            FadeManager.AddState(FadeManager.State.A_TO_ONE);
            FadeManager.SceneOut("TitleScene");
        }

        Debug.Log("Button click!" + gameObject.name );
    }

    public void OnSubmit()
    {
        Debug.Log("Submit!");
    }
}
