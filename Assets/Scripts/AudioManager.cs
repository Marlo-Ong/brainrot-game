using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager singleton;
    private AudioSource source;

    public AudioClip globalFailSound;
    public AudioClip globalWinSound;

    void Awake()
    {
        Debug.Assert(singleton == null);
        singleton = this;

        this.source = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        singleton.source.PlayOneShot(clip);
    }

    public static void PlayFailSound() => singleton.source.PlayOneShot(singleton.globalFailSound);
    public static void PlayWinSound() => singleton.source.PlayOneShot(singleton.globalWinSound);

}
