using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBaseClass : MonoBehaviour
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

    void CaluculateCellPos()
    {
        // セル位置を計算。
        Vector3 vec = transform.position - new Vector3(-1.0f, -1.0f, -1.0f);
        vec.x = Mathf.Round(vec.x); // 丸める
        vec.y = Mathf.Round(vec.y);
        vec.z = Mathf.Round(vec.z);
    }
    public static GameObject FindStar()
    {
        GameObject starMaker = GameObject.FindGameObjectWithTag(ObjectTag.StarMaker);

        if(starMaker == null)
            return null;
        
        return null;
    }
}
