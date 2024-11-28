using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager singleton;
    private AudioSource source;

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
}
