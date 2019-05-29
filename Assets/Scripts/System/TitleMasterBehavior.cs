using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleMasterBehavior : MonoBehaviour
{
    public GameObject m_ParentCanvasPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // フェードイン
        FadeManager.ClearState();
        FadeManager.AddState(FadeManager.State.A_TO_ZERO);
        FadeManager.SceneIn();

        // UI objectを生成.
        GameObject menu = Instantiate(m_ParentCanvasPrefab);
        menu.GetComponent<ParentMenuCanvasBehavior>().SetActivateSelectionCursor();
    }
}
