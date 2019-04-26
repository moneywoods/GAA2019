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

    
    [SerializeField]                // 表示領域                                
    public Vector3 rect = new Vector3(200, 0, 0);   // テキストの座標入力

    [SerializeField]                // 表示領域
    public float Timemax;           // 最大表示時間
    private float Timecount;        // 時間のカウント

   


    // Use this for initialization
    void Start()
    {
        // 初期化
        Textflag = 0;
        Textalpha = 0.0f;
        Timecount = 0.0f;
        Area = GetComponent<RectTransform>();     // テキストの座標位置取得の仕方 
        Area.localPosition = rect;                // テキスト座標の変更
        Qtext = GetComponentInChildren<Text>();   // UIのテキストの取得の仕方
        Qtext.text = "代替テキスト";              // テキストの変更
        
    }

    // Update is called once per frame
    void Update()
    {
        // Textの色変更
        Qtext.color = new Color(0, 0, 0, Textalpha);
        
        // Mキーを押してTextflagを1にする
        if (Input.GetKey(KeyCode.M))
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
        else if(Textflag == 2)
        {
            Textalpha -= Time.deltaTime;
        }

        // Textalphaが1より大きいときTimecountを増やす
        if (Textalpha > 1)
        {
            Textalpha = 1;
            Timecount += Time.deltaTime; 
        }

        // Textalphaが0より小さいときTextalpha、Textflag、Timecountを0にする
        else if (Textalpha< 0)
        {
            Textalpha = 0.0f;
            Textflag = 0;
            Timecount = 0.0f;
        }
    }
}
