using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMenuCanvasInGameBehavior : MenuCanvasController
{
    // Start is called before the first frame update
    void Start()
    {
        SetActivateSelectionCursor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchActive()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            SetActivateSelectionCursor();
        }
        
    }
}
