using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializerBehavior : MonoBehaviour
{
    public GameObject m_StarMakerPrefab = null;

    private void Awake()
    {
        // 世界を作る.
        GameObject starMaker = Instantiate(m_StarMakerPrefab);
        starMaker.GetComponent<StarMaker>().MakeWorld();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
