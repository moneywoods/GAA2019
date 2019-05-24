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
        ParticleMoveGuideLine,
        ParticleMax
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
    [SerializeField]GameObject m_GuideLine;
    
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        ParticlePrefabList = new List<MyParticle>();
        ParticlePrefabList.Add(new MyParticle(m_GuideLine, ParticleIndex.ParticleMoveGuideLine));


        Instantiate(m_GuideLine);

    }

    public GameObject GetParticle(ParticleIndex index)
    {
        if(ParticlePrefabList == null) Initialize();

        return ParticlePrefabList.Find(obj => obj.index == index).ParticleObj;
    }


}
