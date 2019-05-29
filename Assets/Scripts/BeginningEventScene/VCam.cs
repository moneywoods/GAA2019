﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCam : MonoBehaviour
{
    private CinemachineVirtualCamera m_VCam;
    private CinemachineTrackedDolly m_Dolly;

    private float m_Radian;

    [SerializeField]
    private float m_CameraSpeed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objOcto = GameObject.FindWithTag("PlayerCharacter");
        m_VCam = GetComponent<CinemachineVirtualCamera>();
        

        m_VCam.LookAt = objOcto.transform;
//        m_VCam.Follow = objOcto.transform;

        m_Dolly = m_VCam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public bool MoveVCam()
    {
        m_Dolly.m_PathPosition += m_CameraSpeed * Time.deltaTime;

        bool isEndScene = m_Dolly.m_PathPosition >= m_Dolly.m_Path.MaxPos;
        
        return (isEndScene) ? true : false;
    }

}

