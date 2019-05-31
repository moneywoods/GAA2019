using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class MainVCam : MonoBehaviour
{

    float m_Timer = 0f;
    [SerializeField]
    private float TIME_UNTIL_CAMERA_MOVES = 0.5f;
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
        m_Timer += Time.deltaTime;
        if (m_Timer >= TIME_UNTIL_CAMERA_MOVES)
        {
            GameObject eventVCam = GameObject.FindWithTag("EventVCam");
            eventVCam.GetComponent<EventCamera>().CameraSetting();
            gameObject.SetActive(false);
        }        
    }
}
