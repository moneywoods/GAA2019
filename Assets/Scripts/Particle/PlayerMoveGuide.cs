using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tako;

public class PlayerMoveGuide : MonoBehaviour
{
    private GameObject m_ObjPlayer;
    private TakoController m_TakoScript;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjPlayer = GameObject.FindWithTag("PlayerCharacter");
        m_TakoScript = m_ObjPlayer.GetComponent<TakoController>();
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ParticleStart();
        ParticleStop();
    }

    public void ParticleStart()
    {
        bool isIndexStar = (m_TakoScript.nextStar != null && m_TakoScript.CurrentState.Name == TakoController.StateName.Normal);
        if (m_TakoScript == null) return; 

        if (isIndexStar)
        {
            ThisRotation();
            MiddlePoint();
            IsPlay();
        }else
        {
            IsStop();
        }
    }

    private void ParticleStop()
    {
        if (m_TakoScript.nextStar == null)
        {
            return;
        }

        if(m_TakoScript.CurrentState.Name != TakoController.StateName.Normal) {
            IsStop();
        }
    }

    private void MiddlePoint()
    {
        Vector3 nextStarPos = m_TakoScript.nextStar.transform.position;
        Vector3 pos = m_ObjPlayer.transform.position;

        // 中点を取得
        Vector3 middlePoint = new Vector3(0f, 0f, 0f);
        middlePoint.x = (pos.x + nextStarPos.x) * 0.5f;
        middlePoint.z = (pos.z + nextStarPos.z) * 0.5f;

        transform.position = middlePoint;

        ThisRadius(middlePoint, pos);
    }

    private void ThisRotation()
    {
        Vector3 pos = m_ObjPlayer.transform.position;
        Vector3 target = m_TakoScript.nextStar.transform.position;

        float dx = pos.x - target.x;
        float dz = pos.z - target.z;
        float radian = Mathf.Atan2(dz, dx);
        float deg = radian * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(-deg, new Vector3(0, 1, 0));
    }
    private void ThisRadius(Vector3 midPt, Vector3 other)
    {
        ParticleSystem.ShapeModule shape = GetComponent<ParticleSystem>().shape;
        float radius = 0f;
        float x = 0f;
        float z = 0f;

        x = midPt.x - other.x;
        z = midPt.z - other.z;
        radius = x * x + z * z;
        
        shape.radius = Mathf.Sqrt(radius);
    }

    private void IsPlay()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            GetComponent<ParticleSystem>().Play();
        }
    }

    private void IsStop()
    {
        if (!GetComponent<ParticleSystem>().isStopped)
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
