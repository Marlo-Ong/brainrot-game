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
        this.gameObject.SetActive(true);
    }

    public override void Stop()
    {
        this.isPlaying = false;
        this.gameObject.SetActive(false);
    }

    public virtual void Win()
    {
        this.Stop();
        AudioManager.PlaySound(this.OnSuccessSound);
        GameManager.AddAuraPoints(this.auraPointsToWin);
    }

    public virtual void Fail()
    {
        this.Stop();
        AudioManager.PlaySound(this.OnFailSound);
        GameManager.AddAuraPoints(-this.auraPointsToLose);
    }
}
