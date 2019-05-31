using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GoalEventScene : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainVCam;
    [SerializeField]
    private GameObject m_EventVCam;

    private GameObject m_ObjTako;


    bool CheckFlag = false;
    // 次のステージへの切り替え方修正してほちぃ
    float m_Timer = 0f;
    [SerializeField]
    private float TIME_TO_SCENE_TRANSITION = 8f;

    private GameObject m_CloneMainVCam;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjTako = GameObject.FindWithTag("PlayerCharacter");
        Instantiate(m_MainVCam);
        Instantiate(m_EventVCam);

        CheckFlag = false;
        m_CloneMainVCam = GameObject.FindWithTag("MainVCam");

    }

    // Update is called once per frame
    void Update()
    {
        // オクトちゃんぐるぐる回転
        m_ObjTako.transform.Rotate(new Vector3(0, 1, 0), 4);

        SceneChange();
    }
    bool IsCheckStageChange()
    {
        if (GameMasterBehavior.InitiatingChapter == 1)
        {
            return true;
        }
        return false;
    }    

    void SceneChange()
    {
        if (!m_CloneMainVCam.activeSelf)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= TIME_TO_SCENE_TRANSITION && CheckFlag != true)
            {
                if (IsCheckStageChange())
                {
                    FadeManager.BeginSetting();
                    FadeManager.NextColor = Color.black;
                    FadeManager.NextColor.a = 0f;
                    FadeManager.AddState(FadeManager.State.A_TO_ONE);
                    FadeManager.SceneOut("BeginingEventScene");
                    CheckFlag = true;
                }
                else
                {
                    FadeManager.BeginSetting();
                    FadeManager.NextColor = Color.black;
                    FadeManager.NextColor.a = 0f;
                    FadeManager.AddState(FadeManager.State.A_TO_ONE);
                    FadeManager.SceneOut("Scene0315");
                    CheckFlag = true;
                }
            }
        }
    }
}
