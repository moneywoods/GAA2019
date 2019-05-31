using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ChosenCellBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector2Int cellNum = new Vector2Int( -1, -1 );
    private Tako.TakoController takoCon = null;

    // Start is called before the first frame update
    void Start()
    {
        var tako = GameObject.FindGameObjectWithTag(ObjectTag.PlayerCharacter);

        if(tako != null)
        {
            takoCon = tako.GetComponent<Tako.TakoController>();
        }
        else
        {
            Debug.Log("No tako is found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TakoにnextStarが設定されていないときは表示しない
        if(takoCon.nextStar == null ||
            takoCon.nextStar.GetComponent<StarBase>().CellNum == new Vector2Int(-1, -1) ||
            takoCon.nextStar.GetComponent<LandStarController>().CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
        {
            transform.position = new Vector3(114, 514, 1919);
            return;
        }

        // 場所の更新
        cellNum = takoCon.nextStar.GetComponent<StarBase>().CellNum;
        var pos = StarMaker.Instance.GetCenterPositionOfCell(takoCon.nextStar.GetComponent<StarBase>().CellNum);
        transform.position = pos;
    }
}
