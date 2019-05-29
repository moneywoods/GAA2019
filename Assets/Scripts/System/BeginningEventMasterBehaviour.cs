using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BeginningEventMasterBehaviour : MonoBehaviour
{
    // ゲームオブジェクトの生成用
    [SerializeField]
    private GameObject m_ObjOcto;
    [SerializeField]
    private GameObject m_ObjVirtualCamera;
    [SerializeField]
    private GameObject m_ObjEventRelation;
    
    private OctoStartMove m_OctoScript;     // オクトちゃんのスクリプト
    private VCam m_VCamScript;              // VirtualCameraのスクリプト

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameMasterBehavior.InitiatingStage.Stage == 1 || GameMasterBehavior.InitiatingStage.Stage == 0)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
        }

        // ゲームオブジェクト生成
        m_ObjOcto.transform.position = new Vector3(0f, 0f, 0f);
        Instantiate(m_ObjOcto);

        GameObject m_VCam = Instantiate(m_ObjVirtualCamera);

        Instantiate(m_ObjEventRelation);

    }
}
