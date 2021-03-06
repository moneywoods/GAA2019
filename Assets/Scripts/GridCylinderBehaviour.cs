﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCylinderBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject CylinderPrefab;

    [SerializeField] private Vector2 stepSize; // グリッド中の1マスの幅高さ
    [SerializeField] private Vector2Int cellCnt; // 線の数
    [SerializeField] private Vector3 offset; // グリッド左上へのオフセット
    [SerializeField] private Material material;
    private float Radius = 0.05f;
    [SerializeField] private StarMaker.MapInfo currentMapInfo = null;

    private int horizontalNum;
    private int verticalNum;

    // Start is called before the first frame update
    public void Init()
    {
        currentMapInfo = StarMaker.Instance.CurrentMapInfo;

        cellCnt = currentMapInfo.CellCnt;
        stepSize = currentMapInfo.CellSize;
        // StarMakerの座標に合わせるオフセット
        var topLeftOffset = currentMapInfo.CellSize * currentMapInfo.CellCnt * 0.5f;
        topLeftOffset.x *= -1;
        topLeftOffset.y += currentMapInfo.CellSize.y;

        offset = new Vector3(topLeftOffset.x, 0.0f, topLeftOffset.y) + StarMaker.Instance.gameObject.transform.position;

        // horizontal line
        var halfH = stepSize.x * 0.5f;

        for (int row = 0; row < cellCnt.y + 1; row++)
        {
            for (int col = 0; col < cellCnt.x; col++)
            {
                var pos = new Vector3(col * stepSize.x, 0.0f, -row * stepSize.y) +  new Vector3(halfH, 0.0f, 0.0f) + offset;
                var obj = CreateCylinder(pos, true);
                obj.name = "GridCylinder_H (" + col.ToString() + ", " + row.ToString() + ")";
                
            }
        }

        // vertical line
        var halfV = stepSize.y * 0.5f;

        for (int col = 0; col < cellCnt.x + 1; col++)
        {
            for (int row = 0; row < cellCnt.y; row++)
            {
                var pos = new Vector3(col * stepSize.x, 0.0f, -row * stepSize.y) - new Vector3(0.0f, 0.0f, halfV) + offset;
                var obj = CreateCylinder(pos, false);
                obj.name = "GridCylinder_V (" + col.ToString() + ", " + row.ToString() + ")";

            }
        }

        // マスを形成するシリンダを取得するためにセット
        horizontalNum = cellCnt.x * (cellCnt.y + 1);
        verticalNum = (cellCnt.x + 1) * cellCnt.y; 
    }

    GameObject CreateCylinder(Vector3 pos, bool isHorizontal)
    {
        var q = new Quaternion();
        var obj = Instantiate(CylinderPrefab, pos, q);

        float length = (isHorizontal ? currentMapInfo.CellSize.x : currentMapInfo.CellSize.y) * 0.5f; // どうしてマスが正方形でない想定がなされているのか。
        obj.transform.localScale = new Vector3(Radius, length, Radius);
        Vector3 vec = isHorizontal ? Vector3.forward : Vector3.right;
        obj.transform.Rotate(vec, 90.0f);
        obj.transform.parent = gameObject.transform;
        return obj;
    }
    
    public List<GameObject> GetCylinderList(Vector2Int cellNum)
    {

        var list = new List<GameObject>();
        
        int h0 = cellNum.x + cellCnt.x * cellNum.y;
        int h1 = cellNum.x + cellCnt.x * (cellNum.y + 1);
        int v0 = cellNum.y + cellCnt.y * cellNum.x + horizontalNum;
        int v1 = cellNum.y + cellCnt.y * (cellNum.x + 1) + horizontalNum;

        list.Add(transform.GetChild(h0).gameObject);
        list.Add(transform.GetChild(h1).gameObject);
        list.Add(transform.GetChild(v0).gameObject);
        list.Add(transform.GetChild(v1).gameObject);

        return list;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
