using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpPlayerController : MonoBehaviour {

    private Rigidbody2D rb2D;
    public float speed;
    GameObject CurrentTarget;

    // Use this for initialization
    void Start () {
        CurrentTarget = GameObject.FindWithTag("UFO");
        rb2D = CurrentTarget.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        rb2D = CurrentTarget.GetComponent<Rigidbody2D>();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 vec = new Vector2(moveHorizontal, moveVertical);
        rb2D.AddForce(vec * speed);
    }

    void OnTriggerStay(Collider other)
    {
        CurrentTarget.GetComponent<Renderer>().material.color = Color.red;
    }
}
