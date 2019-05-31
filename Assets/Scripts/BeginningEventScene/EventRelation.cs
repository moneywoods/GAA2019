using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRelation : MonoBehaviour
{
    [SerializeField]
    private float m_TakoMoveSpeed = 3f;          // オクトちゃんの速度

    [SerializeField]
    private float m_CameraStartPos = 5f;    // タコが指定の座標に行ったら、カメラが動き出す

    [SerializeField]
    private float TIMELIMIT = 0f;               // カメラを動かしきってから待機時間が必要なら・・・
    private float m_Timer;

    private OctoStartMove m_OctoScript;     // オクトちゃんのスクリプト
    private VCam m_VCamScript;              // VirtualCameraのスクリプト

    // Start is called before the first frame update
    void Start()
    {
        // 生成されたオクトのクローンのスクリプト
        m_OctoScript = GameObject.FindWithTag("PlayerCharacter").GetComponent<OctoStartMove>();
        m_VCamScript = GameObject.FindWithTag("VCamera").GetComponentInChildren<VCam>();

    }

    // Update is called once per frame
    void Update()
    {
        OctoAdmissionScene();
    }

    private void OctoAdmissionScene()
    {
        IsTakoMoveLimit();
        IsCameraMoveStart();
        IsChangeScene();
    }


    private void IsTakoMoveLimit()
    {
        Vector3 octoPos = m_OctoScript.GetPos();

        if (octoPos.y >= 0f)
        {
            m_OctoScript.OctoAdmission(m_TakoMoveSpeed);
        }
    }

    private void IsCameraMoveStart()
    {
        Vector3 octoPos = m_OctoScript.GetPos();

        if (octoPos.y <= m_CameraStartPos) m_VCamScript.MoveVCam();
    }

    private void IsChangeScene()
    {
        if (m_VCamScript.VCamLimitPos())
        {
            if ((m_Timer += Time.deltaTime) >= TIMELIMIT && !FadeManager.CheckIsFade())
            {
                GameMasterBehavior.isInitiationEvent = true;
                FadeManager.BeginSetting();
                FadeManager.NextColor = Color.black;
                FadeManager.NextColor.a = 0.0f;
                FadeManager.AddState(FadeManager.State.A_TO_ONE);
                FadeManager.SceneOut("scene0315");
            }
        }
    }
}
