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
        CHOSENCELL
    }

    [System.Serializable]
    class MyParticle
    {
        public GameObject ParticleObj;
        public bool isInstatiateOnStart;
        public ParticleIndex index;
        public MyParticle(GameObject part, ParticleIndex index)
        {
            this.ParticleObj = part;
            this.index = index;
        }
    }

    [SerializeField]
    List<MyParticle> ParticlePrefabList;


    // Prefab置き場
    [SerializeField]
    private GameObject m_ObjMoveGuide;

    // Start is called before the first frame update
    void Start()
    {
        // ParticlePrefabList = new List<MyParticle>();
        // ParticlePrefabList.Add(new MyParticle(m_ObjMoveGuide, ParticleIndex.MOVEGUIDE));
        Instantiate(ParticlePrefabList[(int)ParticleIndex.MOVEGUIDE].ParticleObj);
        Instantiate(ParticlePrefabList[(int)ParticleIndex.CHOSENCELL].ParticleObj);
        // Instantiate(m_ObjMoveGuide);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetParticle(ParticleIndex index)
    {
        return ParticlePrefabList[(int)index].ParticleObj;
    }


}
