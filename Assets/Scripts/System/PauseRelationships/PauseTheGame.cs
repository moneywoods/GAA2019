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

    // 時間の速度を設定(０f～１f)
    public static void SetTimeScale(float time)
    {
        bool minTime = time >= 0f;
        bool maxTime = time <= 1f;

        if(minTime && maxTime)
        {
            Time.timeScale = time;
        }
    }
}
