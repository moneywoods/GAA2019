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
        BackGroundLoad();

        // ゲームオブジェクト生成
        m_ObjOcto.transform.position = new Vector3(0f, 0f, 0f);
        Instantiate(m_ObjOcto);

        GameObject m_VCam = Instantiate(m_ObjVirtualCamera);

        Instantiate(m_ObjEventRelation);
    }

    void BackGroundLoad()
    {
        // 土屋君、ここの"ステージ == 数値"を増やしてほしいにゃぁ。
        if (GameMasterBehavior.InitiatingStage.Stage == 1 || GameMasterBehavior.InitiatingStage.Stage == 0)
        {
            // 背景用のシーン読込
            SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
        }
        if (GameMasterBehavior.InitiatingStage.Stage == 2)
        {
            // 背景用のシーン読込
//            SceneManager.LoadScene("シーンの名前", LoadSceneMode.Additive);
        }
        if (GameMasterBehavior.InitiatingStage.Stage == 3)
        {
            // 背景用のシーン読込
//            SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
        }
        if (GameMasterBehavior.InitiatingStage.Stage == 4)
        {
            // 背景用のシーン読込
//            SceneManager.LoadScene("GameBackGround", LoadSceneMode.Additive);
        }


    }
}
