using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEventScene : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainVCam;
    [SerializeField]
    private GameObject m_EventVCam;

    private GameObject m_ObjTako;


    // Start is called before the first frame update
    void Start()
    {
        m_ObjTako = GameObject.FindWithTag("PlayerCharacter");
        Instantiate(m_MainVCam);
        Instantiate(m_EventVCam);
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ObjTako.transform.Rotate(new Vector3(0, 1, 0), 4);
    }
    
}
