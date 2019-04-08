using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SingletonPattern<T> : MonoBehaviour where T : MonoBehaviour
{
    // インスタンス
    private static volatile T instance;

    // 同期オブジェクト(マルチスレッドで同時にインスタンスを生成するのを避けるため.
    private static object synObj = new object();

    // getter
    public static T Instance
    {

        get
        {
            // インスタンスがない場合探す。
            if(instance == null)
            {
                instance = FindObjectOfType<T>() as T;
                
                // 複数インスタンスがあった場合、instanceを戻して関数終了。
                if(1 < FindObjectsOfType<T>().Length)
                {
                    return instance;
                }

            }
            if(instance == null)
            {
                lock(synObj)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString() + "(singleton)";
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj); // シーン変更時に破棄させない。

                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    // アプリケーションが終了しているかどうか。
    static bool applicationIsQuitting = false;

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    // コンストラクタをprotectedにすることでインスタンスを生成できなくする。
    protected SingletonPattern()
    {

    }
}