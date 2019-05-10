using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessnger : MonoBehaviour
{
    
    [SerializeField]                // 表示領域
    public Text Qtext;              // Text取得
    private int Textflag;           // Textのalpha値管理フラグ
    private float Textalpha;        // Textのalpha値変更
    RectTransform Area;             // テキストの座標取得
    private float Timecount;        // 時間のカウント
    private bool testflag;          // テスト用フラグ
    float testtime;                 // テスト用動作時間

    // Use this for initialization
    void Start()
    {
        MessngerInit();
        testflag = false;
        testtime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            testflag = true;
        }

        //テスト用
        if (testflag == true)
        {
            Vector3 trect = new Vector3(0.0f, 0.0f, 0.0f);
            Color tcolor = new Color(255.0f, 0.0f, 0.0f, 1.0f);
            MessngerUpdate("テキスト", trect, tcolor, 1, true);
            testtime += Time.deltaTime;
        if (testtime > 3.0f)
        {
            testflag = false;
            testtime = 0.0f;
        }
    }


}

    public void MessngerInit()
    {
        // 初期化
        Textflag = 0;
        Textalpha = 0.0f;
        Timecount = 0.0f;
        Area = GetComponent<RectTransform>();                       // テキストの座標位置取得の仕方 
        Qtext = GetComponentInChildren<Text>();                     // UIのテキストの取得の仕方
    }

    // MessngerUpdate 引数(文字, テキスト表示位置, テキストカラー, 表示時間, on/off)
    public void MessngerUpdate(string textset, Vector3 rect, Color textcolor, float Timemax, bool Onflag )
    {
        Area.localPosition = new Vector3(rect.x, rect.y, rect.z);                   // テキスト座標の変更
        Qtext.color = new Color(textcolor.r, textcolor.g, textcolor.b, Textalpha);  // Textの色変更
        Qtext.text = textset;                                                       // テキストの変更

        // OnflagがtrueかつTextflagが0のときTextflagを1にする
        if (Onflag && Textflag == 0)
       {
           Textflag = 1;
           Textalpha = 0.0f;
       }

       // TimemaxよりもTimecountが大きくなった時、Textflagを2にする
       if (Timemax <= Timecount)
       {
           Textflag = 2;
       }

       // Textflagが0以外の時に実行する
       if (Textflag == 1)
       {
           Textalpha += Time.deltaTime;
       }
       else if (Textflag == 2)
       {
           Textalpha -= Time.deltaTime;
       }

        // Textalphaがtextcolor.aより大きいときTimecountを増やす
        if (Textalpha >= textcolor.a)
       {
           Textalpha = textcolor.a;
           Timecount += Time.deltaTime;
       } 
       // Textalphaが0より小さいときTextalpha、Textflag、Timecountを0にする
       else if (Textalpha <= 0)
       {
           Textalpha = 0.0f;
           Textflag = 0;
           Timecount = 0.0f;
            Onflag = false;
       }
    }
}
