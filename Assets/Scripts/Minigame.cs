using System;
using UnityEngine;

public class Minigame : Screen
{
    public int auraPointsToWin;
    public int auraPointsToLose;

    public AudioClip OnStartSound;
    public AudioClip OnFailSound;
    public AudioClip OnSuccessSound;

    public override void Pause()
    {
        this.isPlaying = false;
    }

    public override void Play()
    {
        this.isPlaying = true;
        AudioManager.PlaySound(this.OnStartSound);
        this.canvas.gameObject.SetActive(true);
    }

    public override void Stop()
    {
        this.isPlaying = false;
        this.canvas.gameObject.SetActive(false);
    }

    public virtual void Win()
    {
        AudioManager.PlaySound(this.OnSuccessSound);
    }

    public virtual void Fail()
    {
        AudioManager.PlaySound(this.OnFailSound);
    }
}
