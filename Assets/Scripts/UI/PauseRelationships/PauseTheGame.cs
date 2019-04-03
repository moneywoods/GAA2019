using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTheGame : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void PauseSwitching()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }else if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
}
