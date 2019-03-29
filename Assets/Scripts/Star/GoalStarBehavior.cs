using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalStarBehavior : LandStarController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == "PlayerCharacter")
        {
            Debug.Log("Congratulations! Player Win. ");
            GameObject.FindWithTag("Finish").transform.position = transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
