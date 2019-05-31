using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalStarBehavior : LandStarController
{
    [SerializeField]
    private GameObject m_StageClearEvent;

    Tako.TakoController m_TakoControllerScript;
    GameObject effect;

    public GoalStarBehavior()
    {
        starType |= StarType.GoalStar;
        AddStat(LANDSTAR_STAT.STUCKED);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject objTako = GameObject.FindWithTag("PlayerCharacter");
        m_TakoControllerScript = objTako.GetComponent<Tako.TakoController>();

        effect = Instantiate(ParticleManagerBehaviour.Instance.GetParticle(ParticleManagerBehaviour.ParticleIndex.KINETICEFFECT), transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.PlayerCharacter)
        {
            Instantiate(m_StageClearEvent);
            GameMasterBehavior.InitiatingChapter = GameMasterBehavior.InitiatingChapter + 1;


        }
    }
    private void OnTriggerExit(Collider collision)
    {
        
    }


}
