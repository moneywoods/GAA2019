using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    //　出現させるエフェクト
    [SerializeField]
    private GameObject effectObject;
    //　エフェクトを消す秒数
    [SerializeField]
    private float deleteTime;

    LandStarController Star;

    // Start is called before the first frame update
    void Start()
    {
        Star = GetComponent<LandStarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Star.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING))
        {
            var instantiateEffect = GameObject.Instantiate(effectObject, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
            Destroy(instantiateEffect, deleteTime);
        }
    }
}
