using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EventCamera : MonoBehaviour
{
    CinemachineVirtualCamera m_VCam;
    // Start is called before the first frame update
    void Awake()
    {
        // MainCameraの場所に元々あったかのように配置するだけです。

        var vCam = GetComponent<CinemachineVirtualCamera>();
        GameObject objOcto = GameObject.FindWithTag(ObjectTag.PlayerCharacter);

        GameObject mainCam = GameObject.FindWithTag(ObjectTag.MainCamera);
        transform.position = mainCam.transform.position;

        vCam.LookAt = objOcto.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraSetting()
    {
        GameObject objCam = GameObject.FindWithTag(ObjectTag.MainCamera);
        GameObject objOcto = GameObject.FindWithTag(ObjectTag.PlayerCharacter);

        m_VCam = GetComponent<CinemachineVirtualCamera>();
        m_VCam.LookAt = objOcto.transform;      // オクトちゃんを見つめる

        transform.position = objOcto.transform.position;

        Vector3 pos = transform.position;
        pos.z -= 5f;
        pos.y += 3f;
        transform.position = pos;       // オクトの目の前にカメラ移動
    }

    
}
