using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerBehaviour : SingletonPattern<SoundManagerBehaviour>
{
    public enum AudioIndex // ここに必ず追加してください
    {
        Fanfare,
        BGM_Stage1,
        BGM_Stage2,
        BGM_Stage3,
        BGM_Title
    }

    [SerializeField] AudioClip[] clipList; // インデックスとの整合性を保ってください

    List<AudioSource> playingList; // 再生しているAudio これの数だけ同時に音を鳴らせるらしい

    bool isInited = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!isInited)
        {
            Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = playingList.Count -1 ; i <= 0; i--)
        {
            if(i < 0)
            {
                break;
            }

            var audiosrc = playingList[i];

            if (!audiosrc.isPlaying)
            {
                playingList.Remove(audiosrc);
                Destroy(audiosrc);
            }
        }
    }

    // isAdditiveをTrueにすると同じSEがなっていても追加で鳴らし始めます
    public AudioSource Play(AudioIndex index, bool isLoop, bool isAdditive = true)
    {
        if (!isInited)
        {
            Init();
        }

        if (!isAdditive)
        {
            var target = playingList.Find(audio => audio.clip == clipList[(int)index]);
            if ( target != null)
            {
                return target;
            }
        }
        var audioSource = gameObject.AddComponent<AudioSource>();
        playingList.Add(audioSource);
        
        playingList[playingList.Count - 1].clip = clipList[(int)index];
        playingList[playingList.Count - 1].loop = isLoop;
        playingList[playingList.Count - 1].Play();
        return playingList[playingList.Count - 1];
    }

    void Init()
    {
        playingList = new List<AudioSource>();
    }
}
