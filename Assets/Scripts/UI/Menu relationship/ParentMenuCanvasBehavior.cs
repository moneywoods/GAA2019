using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMenuCanvasBehavior : MenuCanvasBehavior
{
    float m_OldTime;        // 時間を止める前の時間を保持

    // Start is called before the first frame update
    void Start()
    {
        m_OldTime = 1.0f;
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
            PauseTheGame.SetTimeScale(m_OldTime);
        }
        else
        {
            gameObject.SetActive(true);
//            SetActivateSelectionCursor();
            m_OldTime = Time.timeScale;
            PauseTheGame.SetTimeScale(0.0f);
        }
    }
}
