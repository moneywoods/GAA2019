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


    // 次のステージへの切り替え方修正してほちぃ
    float m_Timer = 0f;
    readonly float TIME_LIMIT = 8f;

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
        // オクトちゃんぐるぐる回転
        m_ObjTako.transform.Rotate(new Vector3(0, 1, 0), 4);


        m_Timer += Time.deltaTime;
        if (m_Timer >= TIME_LIMIT)
        {
            FadeManager.SceneOut("Scene0315");
        }
    }
    
}
