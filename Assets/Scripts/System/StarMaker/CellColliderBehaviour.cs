using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColliderBehaviour : MonoBehaviour
{
    public List<GameObject> List
    {
        get;
        private set;
    }

    private void Awake()
    {
        // リストのnew
        List = new List<GameObject>();

        // BoxColliderを追加&設定
        var colliderScript = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        colliderScript.isTrigger = true;
        colliderScript.size = new Vector3(StarMaker.Instance.CurrentMapInfo.CellSize.x, 0.0f, StarMaker.Instance.CurrentMapInfo.CellSize.y) - new Vector3(0.1f, 0.0f, 0.1f);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == ObjectTag.Land ||
            collision.tag == ObjectTag.BlackHole || 
            collision.tag == ObjectTag.GoalStar ||
            collision.tag == ObjectTag.MilkyWay)
        {
            foreach(GameObject obj in List) // 重複チェック
            {
                if(obj.gameObject == collision.gameObject)
                {
                    return;
                }
            }
            // 追加
            List.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // 星であるかチェック
        if(collision.tag == ObjectTag.Land ||
            collision.tag == ObjectTag.BlackHole ||
            collision.tag == ObjectTag.GoalStar ||
            collision.tag == ObjectTag.MilkyWay)
        { 
            for(int c = 0; c < List.Count; c++)
            {
                if(List[c].gameObject == collision.gameObject)
                {
                    List.RemoveAt(c);
                    return;
                }
            }
        }
    }
}
