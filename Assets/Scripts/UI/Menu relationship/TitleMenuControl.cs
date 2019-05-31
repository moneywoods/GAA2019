using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TitleMenuControl : MonoBehaviour
{
    GameObject m_ObjMenuCanvas;

    GameObject m_ObjStageSelectCanvas;

    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.red;
        color.pressedColor = Color.blue;

        component.colors = color;

        m_ObjMenuCanvas = transform.parent.gameObject;

        int stageSelectCanvas = 2;
        m_ObjStageSelectCanvas = transform.root.GetChild(stageSelectCanvas).gameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if(FadeManager.CheckIsFade())
        {
            return;
        }
        if (gameObject.name == "BeginGame")
        {// 初めから
            GameMasterBehavior.InitiatingStage = new StageInfo(1, 1);

            FadeManager.BeginSetting();
            FadeManager.NextColor = Color.black;
            FadeManager.SetUnmaskImage(FadeManager.ImageIndex.STAR);
            FadeManager.AddState(FadeManager.State.UNMASK);
            FadeManager.AddState(FadeManager.State.UNMASK_BIGGER);
            FadeManager.UnmaskSize_Start = new Vector2(Screen.width * 10, Screen.height * 10);
            FadeManager.UnmaskSize_End = new Vector2(0.01f, 0.01f);
            GameMasterBehavior.isInitiationEvent = true;
            FadeManager.SceneOut("BeginingEventScene");
        }
        if (gameObject.name == "ContinueGame")
        {// 続きから
            FadeManager.BeginSetting();
            FadeManager.NextColor = Color.black;
            FadeManager.SetUnmaskImage(FadeManager.ImageIndex.STAR);
            FadeManager.AddState(FadeManager.State.UNMASK);
            FadeManager.AddState(FadeManager.State.UNMASK_BIGGER);
            FadeManager.UnmaskSize_Start = new Vector2(Screen.width * 10, Screen.height * 10);
            FadeManager.UnmaskSize_End = new Vector2(0.01f, 0.01f); 
            GameMasterBehavior.isInitiationEvent = true;
            FadeManager.SceneOut("scene0315");
        }
        if (gameObject.name == "SelectStage")
        {// ステージ選択
            m_ObjStageSelectCanvas.gameObject.SetActive(true);
            m_ObjMenuCanvas.gameObject.SetActive(false);
        }
        if (gameObject.name == "End")
        {// ゲーム終了
            Quit();
        }
    }

    //==================
    // ボタンを選んでいる
    //==================
    public void Select()
    {
    }

    //==================
    // ボタンが離れた
    //==================
    public void DeSelect()
    {

    }

    //==================
    // ゲーム終了
    //==================
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }

}
