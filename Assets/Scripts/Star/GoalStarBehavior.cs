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
            GameObject.FindWithTag("Finish").transform.position = transform.position;

            GameMasterBehavior.InitiatingChapter = GameMasterBehavior.InitiatingChapter + 1;
            Instantiate(m_StageClearEvent);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        
    }

    public void EventMove()
    {
        Vector3 pos = transform.position;
        pos = new Vector3(0f, 1f, 0f) * 1f;
        transform.position -= pos;
    }
}
