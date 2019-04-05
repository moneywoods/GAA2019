//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TitleMenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var component = GetComponent<Button>();
        var color = component.colors;
        color.normalColor = Color.white;
        color.highlightedColor = Color.red;
        color.pressedColor = Color.blue;

        component.colors = color;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (gameObject.name == "BeginGame")
        {// 初めから
            FadeManager.FadeOut("scene0315");
        }
        if (gameObject.name == "ContinueGame")
        {// 続きから

        }
        if (gameObject.name == "SelectStage")
        {// ステージ選択

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
//        gameObject.GetComponent<Image>().color = new Color(231.0f / 255.0f, 20.0f / 255.0f, 20.0f / 255.0f, 255.0f / 255.0f);


//        button.GetComponentInChildren<Text>().text = "test";
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
