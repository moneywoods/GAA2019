﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginningEventMasterBehaviour : MonoBehaviour
{
    public bool isSceneEnding = false;

    [SerializeField]
    private GameObject m_ObjOcto;
    [SerializeField]
    private GameObject m_ObjVirtualCamera;

    private GameObject m_MainCam;
    

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
        Instantiate(m_ObjVirtualCamera);

        // 生成されたオクトのクローンのスクリプト
        m_OctoScript = GameObject.FindWithTag("PlayerCharacter").GetComponent<OctoStartMove>();

        

        m_MainCam = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        OctoAdmissionScene();
    }

    private void OctoAdmissionScene()
    {
        if (IsMove())
        {
            isSceneEnding = true;
        }else
        {
            m_OctoScript.OctoAdmission(m_Speed);
        }

        if (isSceneEnding)
        {
            m_Timer += Time.deltaTime;
            Vector3 octoPos = m_OctoScript.GetPos();

        }
        if (TIMELIMIT < m_Timer)
        {
            FadeManager.FadeOut("scene0315");
        }
    }


    private bool IsMove()
    {
        Vector3 octoPos = m_OctoScript.GetPos();

        return octoPos.y <= 5f;
    }
    
}
