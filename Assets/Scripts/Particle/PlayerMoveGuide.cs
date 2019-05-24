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

//        gameObject.GetComponent<ParticleSystem>().Stop();
        //Rotation();
        //MiddlePoint();

    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ParticleStart()
    {
        if (m_TakoScript.nextStar != null)
        {
            Rotation();
            MiddlePoint();
            gameObject.SetActive(true);
        }else
        {
            gameObject.SetActive(false);
        }
    }

    public void ParticleStop()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (m_ObjPlayer != null)
        {
            Rotation();
            MiddlePoint();
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
    }

    private void Rotation()
    {
        Vector3 pos = m_ObjPlayer.transform.position;
        Vector3 target = m_TakoScript.nextStar.transform.position;

        float dx = pos.x - target.x;
        float dz = pos.z - target.z;
        float radian = Mathf.Atan2(dz, dx);
        float deg = radian * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(-deg, new Vector3(0, 1, 0));
    }

}
