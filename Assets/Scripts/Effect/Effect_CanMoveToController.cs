using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_CanMoveToController : MonoBehaviour
{
    private float m_Cnt = 0.0f;
    public Vector3 m_ScaleUpperLimit;
    private Vector3 m_ScaleLowerLimit;

    // Start is called before the first frame update
    void Start()
    {
        m_ScaleLowerLimit = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Mathf.Approximately(Time.timeScale, 0f))
        //{
        //    return;
        //}

        Vector3 newScale = new Vector3();
        newScale.x = m_ScaleLowerLimit.x + (m_ScaleUpperLimit.x - m_ScaleLowerLimit.x) * Mathf.Sin(Mathf.Deg2Rad * (m_Cnt * Time.deltaTime));
        newScale.y = m_ScaleLowerLimit.y + (m_ScaleUpperLimit.y - m_ScaleLowerLimit.y) * Mathf.Sin(Mathf.Deg2Rad * (m_Cnt * Time.deltaTime));
        newScale.z = 1.0f;
        transform.localScale = newScale;
        m_Cnt += 5.0f;
    }
}
