using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    public Canvas canvas;
    public bool playOnStart = true;
    protected bool isPlaying = false;

    void OnEnable()
    {
        if (playOnStart)
            this.Play();
    }

    void OnDisable()
    {
        this.Stop();
    }

    public abstract void Play();

    public abstract void Pause();

    public abstract void Stop();
}
