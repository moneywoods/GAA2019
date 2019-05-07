using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMenuCanvasBehavior : MenuCanvasBehavior
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchActive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            PauseTheGame.SetTimeScale(PauseTheGame.GetOldTime());
        }
        else
        {
            gameObject.SetActive(true);
//            SetActivateSelectionCursor();
            PauseTheGame.SetTimeScale(0.0f);
        }
    }
}
