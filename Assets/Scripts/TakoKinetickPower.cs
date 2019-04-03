using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TakoController))]
public class TakoKinetickPower : MonoBehaviour
{
    TakoController takoControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        takoControllerScript = gameObject.GetComponent<TakoController>();
    }

    // Update is called once per frame
    void Update() // どうしてこのUpdateに書いてしまったのか.
    {
        // ゲームパッド
        bool rsh = Input.GetKeyDown(KeyCode.Joystick1Button5);      // 右ボタン
        bool lsh = Input.GetKeyDown(KeyCode.Joystick1Button4);      // 左ボタン

        if (Input.GetKeyDown(KeyCode.Alpha3) || rsh)
        {
            // 右回り
            // 今いる星の隣接星をすべて回転させる.
            GameObject staying = takoControllerScript.GetCurrentStarStaying();
            GameObject tmpsc = staying.transform.GetChild(0).gameObject;
            List<GameObject> neighvorStarList = tmpsc.GetComponent<NeighvorFinder>().GetNeighvorStarList();

            for (int i = 0; i < neighvorStarList.Count; i++)
            {
                if( neighvorStarList[i].tag == "Land" )
                {

                    LandStarController scriptNeighvor = neighvorStarList[i].GetComponent<LandStarController>();

                    if (scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) && !scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        KinetikPower(neighvorStarList[i], 2.0f, true);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || lsh)
        {
            // 左回り
            // 今いる星の隣接星をすべて回転させる.
            GameObject staying = takoControllerScript.GetCurrentStarStaying();
            GameObject tmpsc = staying.transform.GetChild(0).gameObject;
            List<GameObject> neighvorStarList = tmpsc.GetComponent<NeighvorFinder>().GetNeighvorStarList();

            for (int i = 0; i < neighvorStarList.Count; i++)
            {
                if (neighvorStarList[i].tag == "Land")
                {
                    LandStarController scriptNeighvor = neighvorStarList[i].GetComponent<LandStarController>();

                    if (scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.ALIVE) && !scriptNeighvor.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
                    {
                        KinetikPower(neighvorStarList[i], 2.0f, false);
                    }
                }
            }
        }
    }

    void KinetikPower( GameObject target, float estimatedTimeToCirculate, bool isRight ) // 回すやつ. 関数名がくそすぎる. ここ関数に分ける???
    {
        if( target.tag == "Land")
        {
            target.GetComponent<LandStarController>().SetMove(this.gameObject, estimatedTimeToCirculate, isRight);
        }
    }
}
