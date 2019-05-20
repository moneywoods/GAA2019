using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tako;

public class PlayerMoveGide : MonoBehaviour
{
    private GameObject m_ObjPlayer;
    private TakoController m_TakoScript;
    private bool m_IsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjPlayer = GameObject.FindWithTag("PlayerCharacter");
        this.gameObject.SetActive(true);
        m_IsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_ObjPlayer.transform.position;
//        if (m_TakoScript.nextStar != null)
//        {
//            float speed = 10;
//            Transform target = m_TakoScript.nextStar.transform;
//
//            Vector3 targetDir = target.position - transform.position;
//            targetDir.y = transform.position.y; //targetと高さが異なると体ごと上下を向いてしまうので制御
//            float step = speed * Time.deltaTime;
//            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
//            transform.rotation = Quaternion.LookRotation(newDir);
//        }
    }

    public void ParticleStart()
    {
        if(!m_IsActive)
        {
            gameObject.SetActive(true);
            m_IsActive = true;
        }
    }

    public void ParticleStop()
    {
        if (m_IsActive)
        {
            if(m_TakoScript == null) m_TakoScript = m_ObjPlayer.GetComponent<TakoController>();

            gameObject.SetActive(false);
            m_IsActive = false;
            GameObject a = m_TakoScript.nextStar;
        }
    }
}
