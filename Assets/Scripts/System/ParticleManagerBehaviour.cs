using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class ParticleManagerBehaviour : SingletonPattern<ParticleManagerBehaviour>
{
    public enum ParticleIndex
    {
        MOVEGUIDE,
        MAX,
    }

    class MyParticle
    {
        public GameObject ParticleObj;
        public ParticleIndex index;
        public MyParticle(GameObject part, ParticleIndex index)
        {
            this.ParticleObj = part;
            this.index = index;
        }
    }

    List<MyParticle> ParticlePrefabList;


    // Prefab置き場
    [SerializeField] private GameObject m_ObjMoveGuide;

    // Start is called before the first frame update
    void Start()
    {
        ParticlePrefabList = new List<MyParticle>();
        ParticlePrefabList.Add(new MyParticle(m_ObjMoveGuide, ParticleIndex.MOVEGUIDE));

        Instantiate(m_ObjMoveGuide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetParticle(ParticleIndex index)
    {
        return ParticlePrefabList.Find(obj => obj.index == index).ParticleObj;
    }


}
