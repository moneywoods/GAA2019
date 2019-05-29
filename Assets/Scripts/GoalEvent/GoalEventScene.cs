using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEventScene : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ObjTako;

    [SerializeField]
    private GameObject m_ObjGoalStar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GoalStarBehavior goalScript = m_ObjGoalStar.GetComponent<GoalStarBehavior>();
        goalScript.EventMove();
    }
}
