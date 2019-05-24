using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    GameObject m_ObjTako;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjTako = GameObject.FindWithTag("PlayerCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_ObjTako.transform.position;
        transform.Rotate(new Vector3(0, 0, 1));
    }
}
