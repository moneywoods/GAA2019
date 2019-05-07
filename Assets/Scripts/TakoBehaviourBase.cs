using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakoBehaviourBase : MonoBehaviour
{
    [SerializeField]
    private GameObject currentStarStaying; // 今いる星.
    [SerializeField]
    private GameObject nextStar;
    [SerializeField]
    public GameObject previousStar
    {
        get;
        protected set;
    }


    protected List<GameObject> MovingStarList; // KineticPower適応中の星のリスト

    protected void Awake()
    {
        MovingStarList = new List<GameObject>();
    }

    public void SetCurrentStarStaying(GameObject Land)
    {
        if(currentStarStaying != null)
        {
            currentStarStaying.GetComponent<LandStarController>().LeaveThisLand();
            previousStar = currentStarStaying;
        }
        else
        {
            // null
        }

        currentStarStaying = Land;
        transform.position = Land.transform.position;
        Land.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.PLAYER_STAYING);
    }

    public GameObject GetCurrentStarStaying()
    {
        return currentStarStaying;
    }

    public bool AskKineticPowerAvailable(List<GameObject> neighvorList, bool isRight)
    {
        bool result = true;
        foreach(GameObject star in neighvorList)
        {
            var script = star.GetComponent<StarBase>();
            var pos = currentStarStaying.GetComponent<StarBase>().CellNum;
            if(!star.GetComponent<StarBase>().CheckKineticPowerCanBeUsed(currentStarStaying.GetComponent<StarBase>().CellNum, isRight))
            {
                result = false;
            }
            else
            {
                // null
            }
        }
        return result;
    }

    /* ----- 操作関数 ----- */
    // 星を移動する.
    private bool MoveFromCurrentStar(Direction direction)
    {
        if(direction == Direction.ENUM_MAX || direction == Direction.NONE)
        {
            return false;
        }
        else
        {
            // null
        }

        // 指定された方向に行けるLandがあるかチェック.
        GameObject newLand = StarMaker.Instance.GetStar(currentStarStaying.GetComponent<StarBase>().CellNum, StarBase.StarType.Land, direction);

        if(newLand == null)
        {
            return false;
        }
        else
        {
            // null
        }

        // 移動を開始する.
        currentStarStaying.GetComponent<LandStarController>().LeaveThisLand();

        previousStar = currentStarStaying;
        currentStarStaying = null;
        nextStar = newLand;
        return true;
    }

    private GameObject GetStarOnTheDirection(Direction direction)
    {
        // 今いる星の隣接星リストを取得.
        var neighborStarList = StarMaker.Instance.GetNeighvorList(currentStarStaying.GetComponent<StarBase>().CellNum);

        if(neighborStarList == null)
        {
            Debug.Log("Something wrong! neighvorStarList == null. Function name : GetStarOnTHeDirection");
        }
        // 方向
        foreach(GameObject star in neighborStarList)
        {
            // 0326現在,リストに含まれるのは着陸可能星のみになってます. -> 全て
            if(star.tag == ObjectTag.Land || star.tag == ObjectTag.GoalStar)
            {
                var landScript = star.GetComponent<LandStarController>();
                if((landScript.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE)) && !landScript.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                {
                    // プレイヤーから星への単位ベクトルを作る.
                    var vecPlayerToStar = star.transform.position - transform.position;
                    vecPlayerToStar.Normalize(); // 正規化する.

                    // 比較するための単位ベクトルを取得する.
                    var vecComp = new Vector3(1.0f, 0.0f);

                    float EstimatedStarDegree = 0.0f;

                    if(direction == Direction.Top)
                    {
                        EstimatedStarDegree = -90.0f;
                    }
                    else if(direction == Direction.LeftTop)
                    {
                        EstimatedStarDegree = -135.0f;
                    }
                    else if(direction == Direction.Left)
                    {
                        EstimatedStarDegree = -180.0f;
                    }
                    else if(direction == Direction.LeftBottom)
                    {
                        EstimatedStarDegree = -225.0f;
                    }
                    else if(direction == Direction.Bottom)
                    {
                        EstimatedStarDegree = -270.0f;
                    }
                    else if(direction == Direction.RightBottom)
                    {
                        EstimatedStarDegree = -315.0f;
                    }
                    else if(direction == Direction.Right)
                    {
                        EstimatedStarDegree = 0.0f;
                    }
                    else if(direction == Direction.RightTop)
                    {
                        EstimatedStarDegree = -45.0f;
                    }
                    else
                    {
                        return null;
                    }

                    Vector3 vecSearchngStar = Quaternion.Euler(0.0f, EstimatedStarDegree, 0.0f) * vecComp;
                    if(Vector3.Angle(vecPlayerToStar.normalized, vecSearchngStar.normalized) <= 10.0f)
                    {
                        return star;
                    }
                }
            }
        }
        return null;
    }

    public void KineticPower(float estimatedTimeToCirculate, bool isRight) // 隣接するすべてのLandに回るよう指示する.
    {
        List<GameObject> neighvorStarList = StarMaker.Instance.GetNeighvorList(currentStarStaying.GetComponent<StarBase>().CellNum);

        for(int i = 0; i < neighvorStarList.Count; i++)
        {
            if(neighvorStarList[i].tag == ObjectTag.Land)
            {
                LandStarController scriptNeighvor = neighvorStarList[i].GetComponent<LandStarController>();

                if(scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) && !scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                {
                    neighvorStarList[i].GetComponent<LandStarController>().SetMove(gameObject, estimatedTimeToCirculate, isRight);
                    MovingStarList.Add(neighvorStarList[i]);
                }
            }
        }
    }
}
