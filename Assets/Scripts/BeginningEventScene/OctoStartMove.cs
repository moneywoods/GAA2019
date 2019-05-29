using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoStartMove : MonoBehaviour
{
        // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 20f, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void OctoAdmission(float speed)
    {
        transform.position -= transform.up * speed * Time.deltaTime;
    }
}
