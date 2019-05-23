using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessnger : MonoBehaviour
{
    public Text Qtext;              // Text取得
    private GameObject TextCanvas;  // TextCanvas取得
    private RectTransform Area;     // テキストの座標取得
    private int Textalphaflag;      // Textのalpha値管理フラグ
    private float Textalpha;        // Textのalpha値変更
    private float Timecount;        // テキスト表示時間のカウント
    private float texttime;         // テキスト全体の時間
    private float texttimer;        // テキスト全体のカウント
    private int textflag;           // テキストフラグ
    public int Textflag             // テキストフラグのゲッター、セッター
    {
        get { return textflag; }
        set {textflag = value; }
    }         

    public void MessngerInit()
    {
        // 初期化
        Textalphaflag = 0;
        Textalpha = 0.0f;
        Timecount = 0.0f;
        textflag = 0;
        texttime = 3.1f;
        texttimer = 0.0f;
        TextCanvas = GameObject.FindWithTag(ObjectTag.CanvasText);
        Instantiate(TextCanvas);
        Area = GetComponent<RectTransform>();                       // テキストの座標位置取得の仕方 
        Qtext = GetComponentInChildren<Text>();                     // UIのテキストの取得の仕方
    }

    public void Update()
    {
        Vector3 trect = new Vector3(0.0f, 0.0f, 0.0f);
        Color tcolor = new Color(255.0f, 0.0f, 0.0f, 1.0f);

        switch (textflag)
        {
            case 1:
                {
                    texttimer += Time.deltaTime;
                    MessngerUpdate("これ以上いけない", trect, tcolor, 1);
                    break;
                }

            case 2:
                {
                    texttimer += Time.deltaTime;
                    MessngerUpdate("星が破壊された", trect, tcolor, 1);
                    break;
                }

            case 3:
                {
                    texttimer += Time.deltaTime;
                    MessngerUpdate("動かない", trect, tcolor, 1);
                    break;
                }

            case 4:
                {
                    texttimer += Time.deltaTime;
                    MessngerUpdate("これ以上こちらには動かない", trect, tcolor, 1);
                    break;
                }

            case 5:
                {
                    texttimer += Time.deltaTime;
                    MessngerUpdate("この状態だと動かない", trect, tcolor, 1);
                    break;
                }
        }
    }


    // MessngerUpdate 引数(文字, テキスト表示位置, テキストカラー, 表示時間, on/off)
    public void MessngerUpdate(string textset, Vector3 rect, Color textcolor, float Timemax)
    {
        Area.localPosition = new Vector3(rect.x, rect.y, rect.z);                   // テキスト座標の変更
        Qtext.color = new Color(textcolor.r, textcolor.g, textcolor.b, Textalpha);  // Textの色変更
        Qtext.text = textset;                                                       // テキストの変更
         
        // Textalphaflagが0のときTextflagを1にする
        if (Textalphaflag == 0)
        {
            Textalpha = 0.0f;
            Textalphaflag = 1;
        }

        // TimecountがTimemax以上のとき、Textflagを2に、Timecountを0にする
        if (Timemax <= Timecount)
        {
            Textalphaflag = 2;
            Timecount = 0.0f;
        }

        // Textalphaflagが0以外の時に実行する
        if (Textalphaflag == 1)
        {
            Textalpha += Time.deltaTime;
        }

        if (Textalphaflag == 2)
        {
            Textalpha -= Time.deltaTime;
        }

        // Textalphaがtextcolor.a以上のときTimecountを増やす
        if (Textalpha >= textcolor.a)
        {
            Textalpha = textcolor.a;
            Timecount += Time.deltaTime;
        }

        // texttimerがtexttime以上のときTextalpha、Textalphaflag、texttimer、textflagを0にする
        if (texttime <= texttimer)
        {
            Textalpha = 0.0f;
            Textalphaflag = 0;
            texttimer = 0.0f;
            textflag = 0;

        }
    }
}
