using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billborad : MonoBehaviour
{

    private Camera m_TargetCamera;
    // Start is called before the first frame update
    void Start()
    {
        GameObject objCamera = GameObject.FindWithTag("MainCamera");
        //対象のカメラが指定されない場合にはMainCameraを対象とします。
        if (this.m_TargetCamera == null)
            m_TargetCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //カメラの方向を向くようにする。
        this.transform.LookAt(this.m_TargetCamera.transform.position);
    }
}
