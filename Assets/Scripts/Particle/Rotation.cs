﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    GameObject m_ObjTako;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(m_ObjTako == null) m_ObjTako = GameObject.FindWithTag("PlayerCharacter");

        Vector3 pos = gameObject.transform.position = m_ObjTako.transform.position;
        pos.y = 3f;
        transform.position = pos;
    }
}
