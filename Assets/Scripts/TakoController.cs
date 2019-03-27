using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakoController : MonoBehaviour
{
    GameObject currentStarStaying;

    // 移動可能な隣接星を明示するエフェクト作成用.
    List<GameObject> neighvorList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 移動可能な隣接星を明示するエフェクト作成.
        // この処理は他のファイルとかにした方がゴチャらなくてなくていいかも
        foreach( GameObject obj in neighvorList )
        {
            if( obj.tag == "Land" || obj.tag == "GoalStar") // 今のところ着陸可能星しかリストに入ってないのでこの条件いらないけど一応.
            {
                obj.GetComponent<LandStarController>().SetCanMoveToEffect(CheckDirection(obj));
            }
        }
    }

    // 星を移動する.
    public bool MoveFromCurrentStar( PlayerCommandBehavior.Direction _Direction )
    {
        // 星を移動する.
        GameObject newLand = null;

        if (_Direction == PlayerCommandBehavior.Direction.W )
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.W);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.Q)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Q);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.A)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.A);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.Z)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Z);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.X)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.X);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.C)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.C);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.D)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.D);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.E)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.E);
        }
        // 星を渡る.
        if (newLand != null)
        {
            // 移動処理
            foreach( GameObject obj in neighvorList)
            {
                obj.GetComponent<LandStarController>().DiscardCanMoveToEffect();
            }
            var currentStarScript = currentStarStaying.GetComponent<LandStarController>();
            currentStarScript.LeaveThisLand(gameObject);

            newLand.GetComponent<LandStarController>().ArriveThisLand(gameObject);
            return true;
        }

        return false;
    }
    public void SetCurrentStarStaying( GameObject Land )
    {
        currentStarStaying = Land;
        transform.position = Land.transform.position;
        neighvorList = Land.transform.GetChild(0).GetComponent<NeighvorFinder>().GetNeighvorStarList();
        Land.GetComponent<LandStarController>().AddStat(LandStarController.LANDSTAR_STAT.PLAYER_STAYING);
    }

    public GameObject GetCurrentStarStaying()
    {
        return currentStarStaying;
    }

    private GameObject GetStarOnTheDirection( PlayerCommandBehavior.Direction _Direction )
    {
        // 今いる星の隣接星リストを取得.
        GameObject tmp = currentStarStaying.transform.GetChild(0).gameObject;
        NeighvorFinder script = tmp.GetComponent<NeighvorFinder>();
        List<GameObject> neighborStarList = script.GetNeighvorStarList();
        // List<GameObject> neighborList = currentStarStaying.transform.GetChild(0).GetComponent<NeighvorFinder>().GetNeighvorStarList(); 1行で書くとこんな感じ?
        if( neighborStarList == null )
        {
            Debug.Log("Something wrong! neighvorStarList == null. Function name : GetStarOnTHeDirection");
        }
        // 方向
        foreach ( GameObject land in neighborStarList )
        {
            // 0326現在,リストに含まれるのは着陸可能星のみになってます.
            var landScript = land.GetComponent<LandStarController>();
            if ( (landScript.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) ) && !landScript.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING) )
            {
                // プレイヤーから星への単位ベクトルを作る.
                var vecPlayerToStar = land.transform.position - transform.position;
                vecPlayerToStar.Normalize(); // 正規化する.

                // 比較するための単位ベクトルを取得する.
                var vecComp = new Vector3(1.0f, 0.0f); // 真上へのベクトル.

                float EstimatedStarDegree = 0.0f;

                if (_Direction == PlayerCommandBehavior.Direction.W)
                {
                    EstimatedStarDegree = 90.0f; // 下方向Y正, 右方向X正 に注意!
                }
                else if (_Direction == PlayerCommandBehavior.Direction.Q)
                {
                    EstimatedStarDegree = 135.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.A)
                {
                    EstimatedStarDegree = 180.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.Z)
                {
                    EstimatedStarDegree = 225.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.X)
                {
                    EstimatedStarDegree = 270.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.C)
                {
                    EstimatedStarDegree = 315.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.D)
                {
                    EstimatedStarDegree = 0.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.E)
                {
                    EstimatedStarDegree = 45.0f;
                }
                else
                {
                    return null;
                }

                Vector3 vecSearchngStar = Quaternion.Euler(0.0f, 0.0f, EstimatedStarDegree) * vecComp;
                if (Vector3.Angle(vecPlayerToStar.normalized, vecSearchngStar.normalized) <= 10.0f)
                {
                    return land;
                }

            }
        }
        return null;
    }

    PlayerCommandBehavior.Direction CheckDirection(GameObject obj)
    {
        var vecPlayerToStar = obj.transform.position - transform.position;


        // 比較するための単位ベクトルを取得する.
        var vecComp = new Vector3(1.0f, 0.0f); // 真上へのベクトル.

        // 方向に応じたベクトルを作成し,プレイヤーから星へのベクトルと比較.
        Vector3 vecSearchingStar;
        for (PlayerCommandBehavior.Direction i = 0; i < PlayerCommandBehavior.Direction.ENUM_MAX; i++ )
        {
            vecSearchingStar = Quaternion.Euler(0.0f, 0.0f, (uint)i * 45.0f ) * vecComp;

            if (Vector3.Angle(vecPlayerToStar.normalized, vecSearchingStar.normalized) <= 10.0f)
            {
                return i;
            }
        }
        return PlayerCommandBehavior.Direction.NONE;
    }
}
