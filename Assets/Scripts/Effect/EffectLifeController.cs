using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeController : MonoBehaviour
{
    // private ParticleSystem m_PerticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        // m_PerticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!m_PerticleSystem.isEmitting) // パーティクルが終了したら自身を削除する.
        //{
        //    Destroy(gameObject);
        //}
    }

    void OnParticleSystemStopped()
    {

    }
}
