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

        if (_Direction == PlayerCommandBehavior.Direction.Top )
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Top);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.LeftTop)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.LeftTop);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.Left)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Left);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.LeftBottom)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.LeftBottom);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.Bottom)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Bottom);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.RightBottom)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.RightBottom);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.Right)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.Right);
        }
        else if (_Direction == PlayerCommandBehavior.Direction.RightTop)
        {
            newLand = GetStarOnTheDirection(PlayerCommandBehavior.Direction.RightTop);
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

    public void GetKeyCommand( KeyCode keyCode )
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.Top);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.RightTop);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.Right);

        }
        else if (Input.GetKey(KeyCode.C))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.RightBottom);

        }
        else if (Input.GetKey(KeyCode.X))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.Bottom);

        }
        else if (Input.GetKey(KeyCode.Z))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftBottom);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftBottom);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            MoveFromCurrentStar(PlayerCommandBehavior.Direction.LeftTop);
        }
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

                if (_Direction == PlayerCommandBehavior.Direction.Top)
                {
                    EstimatedStarDegree = 90.0f; // 下方向Y正, 右方向X正 に注意!
                }
                else if (_Direction == PlayerCommandBehavior.Direction.LeftTop)
                {
                    EstimatedStarDegree = 135.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.Left)
                {
                    EstimatedStarDegree = 180.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.LeftBottom)
                {
                    EstimatedStarDegree = 225.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.Bottom)
                {
                    EstimatedStarDegree = 270.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.RightBottom)
                {
                    EstimatedStarDegree = 315.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.Right)
                {
                    EstimatedStarDegree = 0.0f;
                }
                else if (_Direction == PlayerCommandBehavior.Direction.RightTop)
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
