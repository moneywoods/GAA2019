using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // カメラ情報をメインカメラ（親）と同期
        // fov
        GetComponent<Camera>().fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView;   
    }
}
