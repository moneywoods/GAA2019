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
        if (m_Timer >= TIME_LIMIT && !FadeManager.CheckIsFade())
        {
            // 一時的にFadeの表現をこれにしてます
            FadeManager.BeginSetting();
            FadeManager.NextColor = Color.black;
            FadeManager.SetUnmaskImage(FadeManager.ImageIndex.STAR);
            FadeManager.AddState(FadeManager.State.UNMASK);
            FadeManager.AddState(FadeManager.State.UNMASK_BIGGER);
            FadeManager.UnmaskSize_Start = new Vector2(Screen.width * 10, Screen.height * 10);
            FadeManager.UnmaskSize_End = new Vector2(0.01f, 0.01f);
            GameMasterBehavior.isInitiationEvent = true;
            // ------ ここまで
            FadeManager.SceneOut("Scene0315");
        }
    }
    
}
