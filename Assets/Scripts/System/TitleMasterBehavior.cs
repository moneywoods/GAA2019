using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleMasterBehavior : MonoBehaviour
{
    public GameObject m_ParentCanvasPrefab;
    public GameObject m_EventObjectPrefab;


    // Start is called before the first frame update
    void Start()
    {
        // フェードイン
        FadeManager.FadeIn();

        // Event objectを生成. UIの前に必ず生成!
        Instantiate(m_EventObjectPrefab);

        // UI objectを生成.
        GameObject menu = Instantiate(m_ParentCanvasPrefab);
        menu.GetComponent<ParentMenuCanvasBehavior>().SetActivateSelectionCursor();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
