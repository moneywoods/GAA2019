using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    [Flags]
    public enum ObjectType
    {
        Character,
        Star,
        Other
    }

    public ObjectType objectType
    {
        get;
        protected set;
    }


    [SerializeField]private Vector2Int cellNum = new Vector2Int(-1, -1);

    public Vector2Int CellNum // 必ずプロパティを介して値を取得してください.
    {
        get
        {
            //  cellNum = StarMaker.Instance.CaluculateCellNum(transform.position);
            return cellNum;
        }
        set
        {
            cellNum = value;
        }
    }

    public MyGameObject(ObjectType type)
    {
        objectType = type;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void TriggerEnterCell(GameObject other)
    {

    }

    public virtual void TriggerExitCell(GameObject other)
    {

    }

    public virtual void TriggerOtherComeToSameCell(GameObject other)
    {

    }

    public virtual void TriggerOtherLeaveFromSameCell(GameObject other)
    {

    }
}
