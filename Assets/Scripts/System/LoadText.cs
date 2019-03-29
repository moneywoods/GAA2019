using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadText : MonoBehaviour
{
    public string[] textMessage; //テキストの加工前の一行を入れる変数
    public string[,] textWords; //テキストの複数列を入れる2次元配列 

    private int rowLength;      //テキスト内の行数を取得する変数
    private int columnLength;   //テキスト内の列数を取得する変数

    
    // テキストファイルの読み込み
    public void ReadText()
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("StageMaking", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        

        //Splitで一行づつを代入した1次配列を作成
        textMessage = TextLines.Split('\n'); //

        /*--------------------------------------*/

        for(int i = 0; i < textasset.text.Length; i++)
        {
            if(textasset.text.Substring(i, 1).Equals("S"))
            {
                Debug.Log("!!!!AAAA!!!!");
            }

            if(TextLines.Substring(i, 1).Equals("S"))
            {
                Debug.Log("!!!!BBBB!!!!");
            }
            if (TextLines.Substring(i, 1).Equals("\n"))
            {
                Debug.Log("!!!!CCCC!!!!");
            }
        }
        Debug.Log("Length = " + textasset.text.Length);
        
        /*--------------------------------------*/
        // 列数と行数を取得
        columnLength = textMessage[0].Split('\t').Length;
        rowLength = textMessage.Length;

        //2次配列を定義
        textWords = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++)
        {
            string[] tempWords = textMessage[i].Split('\t'); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入
            for (int n = 0; n < columnLength; n++)
            {
                textWords[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }
    }

    // ステージ用テキストの取得
    public string GetStageText(int i, int n)
    {
        return textWords[i, n];
    }
}
