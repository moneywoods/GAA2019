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

    public List<GameObject> OtherList
    {
        get;
        private set;
    }

    public Vector2Int CellNum;
    [SerializeField] int ListCnt = 0;
    [SerializeField] int OtherListCnt = 0;

    private void Awake()
    {
        // リストのnew
        List = new List<GameObject>();
        OtherList = new List<GameObject>();

        CellNum = StarMaker.Instance.CaluculateCellNum(transform.position);

        // BoxColliderを追加&設定
        var colliderScript = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        colliderScript.isTrigger = true;
        colliderScript.size = new Vector3(StarMaker.Instance.CurrentMapInfo.CellSize.x, 10.0f, StarMaker.Instance.CurrentMapInfo.CellSize.y) - new Vector3(1.0f, 0.0f, 1.0f);
    }

    private void Add(GameObject target)
    {
        // 重複チェック
        foreach(GameObject obj in List)
        {
            if(obj.gameObject == target.gameObject)
            {
                return;
            }
        }
        
        // マス数をセット.
        var newoneScript = target.GetComponent<MyGameObject>();
        newoneScript.CellNum = CellNum;

        // リスト中の星と同じマスに入った時のイベントを行う.
        foreach(var other in List)
        {
            target.GetComponent<MyGameObject>().TriggerEnterCell(other);
        }
        foreach(var other in List)
        {
            other.GetComponent<MyGameObject>().TriggerOtherComeToSameCell(target);
        }

        // 適したリストに追加.
        if(target.GetComponent<MyGameObject>().objectType != MyGameObject.ObjectType.Star)
        {
            OtherList.Add(target);
            OtherListCnt++;
        }
        else
        {
            List.Add(target.gameObject);
            ListCnt++;
        }

    }

    private void Remove(GameObject target)
    {
        // リスト中の星と同じマスから離れる時のイベントを行う.
        foreach(var other in List)
        {
            other.GetComponent<MyGameObject>().TriggerOtherLeaveFromSameCell(target);
        }
        foreach(var other in List)
        {
            target.GetComponent<MyGameObject>().TriggerExitCell(other);
        }

        // 適したリストに追加.
        if(target.GetComponent<MyGameObject>().objectType != MyGameObject.ObjectType.Star)
        {
            OtherList.Remove(target);
            OtherListCnt--;
        }
        else
        {
            List.Remove(target);
            ListCnt--;
        }
    }

    public void AddManually(GameObject star)
    {
        Add(star);
        Debug.Log(star.name + " is added manually in the list of " + gameObject.name);
    }

    public void RemoveManually(GameObject star)
    {
        Remove(star);
        Debug.Log(star.name + " is removed manually from the list of " + gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 星であるかチェック
        if(other.tag == ObjectTag.Land ||
           other.tag == ObjectTag.BlackHole ||
           other.tag == ObjectTag.GoalStar ||
           other.tag == ObjectTag.MilkyWay ||
           other.tag == ObjectTag.PlayerCharacter)
        {
            Add(other.gameObject);
            Debug.Log(other.name + " is in the list of " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 星であるかチェック
        if(other.tag == ObjectTag.Land ||
           other.tag == ObjectTag.BlackHole ||
           other.tag == ObjectTag.GoalStar ||
           other.tag == ObjectTag.MilkyWay ||
           other.tag == ObjectTag.PlayerCharacter)
        {
            // リスト中の星と同じマスから離れる時のイベントを行う.
            Remove(other.gameObject);
            Debug.Log(other.name + " is removed from the list of " + gameObject.name);
        }
    }
}
