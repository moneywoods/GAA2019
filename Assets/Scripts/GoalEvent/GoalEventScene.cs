using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEventScene : MonoBehaviour
{
    private GameObject m_ObjTako;

    private GameObject m_ObjGoalStar;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjTako = GameObject.FindWithTag("PlayerCharacter");
        m_ObjGoalStar = GameObject.FindWithTag("GoalStar");
        
    }

    // Update is called once per frame
    void Update()
    {
        GoalStarBehavior goalScript = m_ObjGoalStar.GetComponent<GoalStarBehavior>();
//        goalScript.EventMove();
    }
}
