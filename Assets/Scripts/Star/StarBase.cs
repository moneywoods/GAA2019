using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBase : MonoBehaviour
{
    private Vector2Int cellNum = new Vector2Int(-1, -1);
    
    public Vector2Int CellNum
    {
        get { return cellNum; }
        set { cellNum = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // マスの位置(列、行)を戻す。
    public Vector2Int CheckCellPos()
    {
        return CellNum;
    }

    public void CaluculateCellPos()
    {
        GameObject starMaker = GameObject.FindWithTag(ObjectTag.StarMaker);
        if(starMaker == null)
        {
            Debug.Log("StarMaker == null. Function: CalculateCellPos. This is " + gameObject.name);
        }
        StarMaker.MapInfo mapInfo = starMaker.GetComponent<StarMaker>().CurrentMapInfo;
        var offset = mapInfo.DeffaultOffset;
        // セル位置を計算。
        Vector3 vec = transform.position - offset;
        cellNum.x = (int) Math.Round(vec.x, MidpointRounding.AwayFromZero) / (int)mapInfo.CellSize.x;// 丸める
        cellNum.y = (int) Math.Round(-vec.y, MidpointRounding.AwayFromZero) / (int)mapInfo.CellSize.y;
    }
}
