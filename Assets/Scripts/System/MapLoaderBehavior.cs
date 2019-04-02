using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoaderBehavior : MonoBehaviour
{


    // マップ
    // S = スタート地点, L = 降りられる惑星, B = ブラックホール, M = 乳, W = 壁, 0 = 何もなし.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public char[,] LoadMap( uint stageNum, uint chapterNum )
    {
        string fileName = "MapData";
        fileName += stageNum.ToString() + "_" + chapterNum.ToString();
        TextAsset textAsset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string[] tmp = textAsset.text.Split('\n'); //テキスト全体をstring型で入れる変数を用意して入れる.

        var data = new char[tmp.GetLength(0) - 1, tmp[0].Length];

        var tmp0 = data.GetLength(0);
        var tmp1 = data.GetLength(1);

        for( int row = 0; row < data.GetLength(0); row++ )
        {
            for( int col = 0; col < data.GetLength(1); col++ )
            {
                data[row, col] = tmp[row][ col];
            }
        }
        return data;
    }
}
