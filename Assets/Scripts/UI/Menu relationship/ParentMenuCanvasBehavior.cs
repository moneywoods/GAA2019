using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMenuCanvasBehavior : MenuCanvasBehavior
{
    private readonly int STAGE_SELECT = 2;
    private readonly int CANVAS_MENU = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(this.name == "ParentMenuCanvasInTitle")
        {
            GetComponent<AudioSource>().Play();
        }

        if(this.name == "ParentMenuCanvasInGame")
        {
            GetComponent<AudioSource>().Play();
        }
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

            GameObject stageSelect = transform.GetChild(STAGE_SELECT).gameObject;
            GameObject canvasMenu = transform.GetChild(CANVAS_MENU).gameObject;
            stageSelect.SetActive(false);
            canvasMenu.SetActive(true);

            PauseTheGame.SetTimeScale(0.0f);
        }
    }
}
