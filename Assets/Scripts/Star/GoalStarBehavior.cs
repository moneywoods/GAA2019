using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalStarBehavior : LandStarController
{
    // シーン遷移までの時間
    private float m_NextSceneTimer = 0;
    [SerializeField]
    private GameObject m_StageClearEvent;

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
    }
    
    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.PlayerCharacter)
        {

            GameMasterBehavior.InitiatingChapter = GameMasterBehavior.InitiatingChapter + 1;
            Instantiate(m_StageClearEvent);

//            FadeManager.FadeOut("scene0315");
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        
    }


}
