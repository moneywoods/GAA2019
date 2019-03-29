﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalStarBehavior : LandStarController
{
    // シーン遷移までの時間
    private uint m_NextSceneTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_NextSceneTimer = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        if(m_NextSceneTimer != 0)
        {
            if(m_NextSceneTimer % 60 == 0)
            {// 時間がたったら次のシーンへ
                FadeManager.FadeOut("01");
            }
            m_NextSceneTimer++;
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == "PlayerCharacter")
        {
            Debug.Log("Congratulations! Player Win. ");
            GameObject.FindWithTag("Finish").transform.position = transform.position;

            m_NextSceneTimer++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
