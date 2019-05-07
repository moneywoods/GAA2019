using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalStarBehavior : LandStarController
{
    // シーン遷移までの時間
    private uint m_NextSceneTimer = 0;

    public GoalStarBehavior()
    {
        starType |= StarType.GoalStar;
        AddStat(LANDSTAR_STAT.STUCKED);
    }

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
            if(m_NextSceneTimer % 120 == 0)
            {// 時間がたったら次のシーンへ
//                PauseTheGame.SetTimeScale(0.0f);
                FadeManager.FadeOut("scene0315");
            }
            m_NextSceneTimer++;
        }   
    }
    
    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.PlayerCharacter)
        {
            GameObject.FindWithTag("Finish").transform.position = transform.position;

            m_NextSceneTimer++;
            GameMasterBehavior.InitiatingChapter = GameMasterBehavior.InitiatingChapter + 1;    
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        
    }
}
