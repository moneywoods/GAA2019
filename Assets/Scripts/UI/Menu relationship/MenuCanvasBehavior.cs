using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuCanvasBehavior : MonoBehaviour
{
    private GameObject m_MenuCanvas;
    public GameObject MenuCanvas
    {
        get { return gameObject; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetActivateSelectionCursor() // これのためにMenuWindowの子にする順番を気を付けて.
    {
        // 子を見て,"MenuCanvas"であるならさらにその子を見る.
        // "MenuBotton"であるならば,それを現在選択されているGameObjectに設定して,関数終了.
        for (var i = 0; i < transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            if (obj.tag == ObjectTag.MenuCanvas)
            {
                obj.gameObject.GetComponent<MenuCanvasBehavior>().SetActivateSelectionCursor();
            }
            else if (obj.tag == ObjectTag.MenuBotton)
            {
                if (EventSystem.current.currentSelectedGameObject == obj.gameObject) return true;

                EventSystem.current.SetSelectedGameObject(obj.gameObject);
                return true;
            }
        }
        return false;
    }
    

}
