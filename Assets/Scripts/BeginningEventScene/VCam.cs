using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCam : MonoBehaviour
{
    private ICinemachineCamera m_VCam;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objOcto = GameObject.FindWithTag("PlayerCharacter");
        m_VCam = GetComponent<ICinemachineCamera>();

        m_VCam.Follow = objOcto.transform;
        m_VCam.LookAt = objOcto.transform;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
