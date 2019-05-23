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

    GameObject Land;
    LandStarController Star;

    // Start is called before the first frame update
    void Start()
    {
        Land = GameObject.FindWithTag("Land");
        Star = Land.GetComponent<LandStarController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Star.CheckFlag(LandStarController.LANDSTAR_STAT.MOVING_LEFT))
        {
            transform.position = Land.transform.position;
            GetComponent<ParticleSystem>().Play();

           // var instantiateEffect = Instantiate(effectObject, transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
          //  Destroy(instantiateEffect, deleteTime);
        }
        */
    }


}
