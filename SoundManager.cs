using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource bgmSource;

    public AudioClip[] audioClips;
    public AudioClip[] bgmClips;


    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    public void ChangeBGM(string BGMName)
    {
        for (int i = 0; i < bgmClips.Length; i++)
        {
            if (bgmClips[i].name == BGMName)
            {
                bgmSource.clip = bgmClips[i];
                bgmSource.Play();
            }
        }
    }

    public void PlaySound(string clipName)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == clipName)
            {
                audioSource.clip = audioClips[i];
                audioSource.Play();
            }
        }
    }
}
