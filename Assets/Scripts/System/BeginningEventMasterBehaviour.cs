using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BeginningEventMasterBehaviour : MonoBehaviour
{
    public bool isSceneEnding = false;

    [SerializeField]
    private GameObject m_ObjOcto;
    [SerializeField]
    private GameObject m_ObjVirtualCamera;

    private VCam m_VCamScript;

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private  float TIMELIMIT;

    private OctoStartMove m_OctoScript;
    private float m_Timer;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(GameMasterBehavior.InitiatingStage.Stage == 1 || GameMasterBehavior.InitiatingStage.Stage == 0)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
        }

        // ゲームオブジェクト生成
        m_ObjOcto.transform.position = new Vector3(0f, 0f, 0f);
        Instantiate(m_ObjOcto);

        GameObject m_VCam = Instantiate(m_ObjVirtualCamera);
        m_VCamScript = m_VCam.GetComponentInChildren<VCam>();

        // 生成されたオクトのクローンのスクリプト
        m_OctoScript = GameObject.FindWithTag("PlayerCharacter").GetComponent<OctoStartMove>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        OctoAdmissionScene();
    }

    private void OctoAdmissionScene()
    {
        if (IsMoveMax())
        {
            isSceneEnding = true;
        }else
        {
            m_OctoScript.OctoAdmission(m_Speed);
        }

        if (isSceneEnding)
        {
            if (m_VCamScript.MoveVCam())
            {
                 FadeManager.FadeOut("scene0315");
            }
        }
    }


    private bool IsMoveMax()
    {
        Vector3 octoPos = m_OctoScript.GetPos();

        return octoPos.y <= 5f;
    }
    
}
