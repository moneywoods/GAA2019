using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
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

    public static char[,] LoadMap( StageInfo stageInfo )
    {
        string fileName = "MapData";
        fileName += stageInfo.Stage.ToString() + "_" + stageInfo.Chapter.ToString();
        TextAsset textAsset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得

        // 改行コード'\n'を取り除く
        string[] tmp = textAsset.text.Split('\n'); //テキスト全体をstring型で入れる変数を用意して入れる.
        
        // '\r'を取り除く
        for(int i = 0; i < tmp.GetLength(0); i++)
        {
            tmp[i] = tmp[i].TrimEnd('\r');
        }
        
        var data = new char[tmp.GetLength(0) - 1, tmp[0].Length];

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