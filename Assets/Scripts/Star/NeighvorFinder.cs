﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighvorFinder : MonoBehaviour
{
    private List<GameObject> neighborStarList = new List<GameObject>();// 隣合った乗れる星のリスト(今後すべての星を含むか要検討).

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 登録済みの星と何度もOnTriggerEnterしてしまうのを防ぎたい.
        // 重複チェック.
        for( int i = 0; i < neighborStarList.Count; i++ )
        {
            if( neighborStarList[ i ] == other.gameObject )
            {
                return; 
            }
        }
        if( ( other.tag == "Land" || other.tag == "GoalStar" ) && other.gameObject != transform.root.gameObject )
        {
            // 当たり判定で隣接判定をし,リストへ追加する.
            neighborStarList.Add(other.gameObject); // add to list

            Vector3 pos = other.gameObject.transform.position;
            float dist = Vector3.Distance(transform.position, other.gameObject.transform.position);
            Debug.Log("This is at( " + transform.position.x + "," + transform.position.y + " ).NeighborStar[" + neighborStarList.Count + "] pos = ( " + pos.x + "," + pos.y + " ).");
            if (other.tag == "GoalStar")
            {
                Debug.Log("New one is goal star.");
            }
            Debug.Log("Distance is " + dist);
            Debug.Log("This is at( " + transform.position.x + "," + transform.position.y + " ).I have" + neighborStarList.Count + " neighvor(s).");
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        for( int mc = 0; mc < neighborStarList.Count; mc++ )
        {
            if( neighborStarList[ mc ] == other.gameObject )
            {
                float dist = Vector3.Distance( transform.position, other.gameObject.transform.position );
                if (7.07f < dist ) // 多分ここの条件いらない.
                {
                    Debug.Log("This is at( " + transform.position.x + "," + transform.position.y + " ).NeighborStar[" + neighborStarList.Count + "] is removed.");
                    Debug.Log("Distance is " + dist);
                    Debug.Log("This is at( " + transform.position.x + "," + transform.position.y + " ).I have" + (neighborStarList.Count - 1) + " neighvor(s).");
                    neighborStarList.RemoveAt(mc);
                    return;
                }

            }

        }
    }

    public List<GameObject> GetNeighvorStarList()
    {
        return neighborStarList;
    }

    private void OnDestroy()
    {
        neighborStarList.Clear(); // 一応.
    }
}