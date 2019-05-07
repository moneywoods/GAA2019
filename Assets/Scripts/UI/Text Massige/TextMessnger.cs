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

   


    // Use this for initialization
    void Start()
    {
        
        MessngerInit();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 trect = new Vector3(-200.0f, 0.0f, 0.0f);
        Color tcolor = new Color(255.0f, 0.0f, 0.0f, 1.0f);
        MessngerUpdate("テキスト", trect, tcolor, 1);
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

    public void MessngerUpdate(string textset, Vector3 rect, Color textcolor, float Timemax)
    {
        Area.localPosition = new Vector3(rect.x, rect.y, rect.z);                   // テキスト座標の変更
        Qtext.color = new Color(textcolor.r, textcolor.g, textcolor.b, Textalpha);  // Textの色変更
        Qtext.text = textset;                                                       // テキストの変更

        // Mキーを押してTextflagを1にする
        if (Input.GetKey(KeyCode.M) && Textflag == 0)
        {
            Textflag = 1;
            Textalpha = 0.0f;
        }

        // TimemaxよりもTimecountが大きくなった時、Textflagを2にする
        if (Timemax < Timecount)
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

        // Textalphaが1より大きいときTimecountを増やす
        if (Textalpha > textcolor.a)
        {
            Textalpha = textcolor.a;
            Timecount += Time.deltaTime;
        }

        // Textalphaが0より小さいときTextalpha、Textflag、Timecountを0にする
        else if (Textalpha < 0)
        {
            Textalpha = 0.0f;
            Textflag = 0;
            Timecount = 0.0f;
        }

    }
}
