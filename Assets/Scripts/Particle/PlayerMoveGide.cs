using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tako;

public class PlayerMoveGide : MonoBehaviour
{
    GameObject m_ObjPlayer;
    TakoController m_TakoScript;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjPlayer = GameObject.FindWithTag("PlayerCharacter");
        m_TakoScript = GetComponent<TakoController>();
    }

    // Update is called once per frame
    void Update()
    {
//        transform.position = m_ObjPlayer.transform.position;
        if (m_TakoScript.nextStar != null)
        {
            float speed = 10;
            Transform target = m_TakoScript.nextStar.transform;

            Vector3 targetDir = target.position - transform.position;
            targetDir.y = transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}
