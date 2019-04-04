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

    //=======================
    // ゲームを停止
    //=======================
    public static void GameStop()
    {
        Time.timeScale = 0f;
    }

    //=======================
    // ゲームを開始
    //=======================
    public static void GameReStart()
    {
        Time.timeScale = 1f;
    }

    //=======================
    // ゲームを一時停止のオンオフ
    //=======================
    public static void GamePauseSwitch()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        } else if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
    }
}
