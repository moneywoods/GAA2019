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
        if(takoCon.nextStar == null)
        {
            transform.position = new Vector3(114, 514, 1919);
            return;
        }

        // 場所の更新
        cellNum = takoCon.nextStar.GetComponent<StarBase>().CellNum;
        if(cellNum == new Vector2Int(-1, -1))
        {
            Debug.Log("cellnum = -1, -1");
            return;
        }

        var pos = StarMaker.Instance.GetCenterPositionOfCell(cellNum);
        transform.position = pos;
    }


}
