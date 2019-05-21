using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalStarBehavior : LandStarController
{
    // シーン遷移までの時間
    private float m_NextSceneTimer = 0;

    Tako.TakoController m_TakoControllerScript;

    public GoalStarBehavior()
    {
        starType |= StarType.GoalStar;
        AddStat(LANDSTAR_STAT.STUCKED);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_NextSceneTimer = 0;
        GameObject objTako = GameObject.FindWithTag("PlayerCharacter");
        m_TakoControllerScript = objTako.GetComponent<Tako.TakoController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_NextSceneTimer != 0)
        {
        float TIME_OVER = 3f;
        m_NextSceneTimer += Time.deltaTime;
            if(m_NextSceneTimer > TIME_OVER)
            {// 時間がたったら次のシーンへ
                m_NextSceneTimer = 0;
                FadeManager.NextColor = Color.blue;
                FadeManager.AddState(FadeManager.State.A_TO_ONE);
                FadeManager.SceneOut("scene0315");
            }
        }
    }
    
    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.PlayerCharacter)
        {
            GameObject.FindWithTag("Finish").transform.position = transform.position;

            if(m_NextSceneTimer == 0) m_NextSceneTimer = Time.deltaTime;

            GameMasterBehavior.InitiatingChapter = GameMasterBehavior.InitiatingChapter + 1;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        
    }
}
