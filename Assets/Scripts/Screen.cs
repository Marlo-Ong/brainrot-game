using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    public Canvas canvas;
    public bool isBlocking = false;
    public bool playOnStart = true;
    public bool isPlaying = false;

    const int BackgroundLayer = 0;
    private static int ForegroundLayer = 0;
    const int BlockingLayer = 100;
    const int HUDLayer = 101;


    public void OnEnable()
    {
        if (playOnStart)
            this.Play();
        this.Setup();
    }

    public void OnDisable()
    {
        this.Stop();
    }
    public abstract void Setup();
    public abstract void Play();

    public abstract void Pause();

    public abstract void Stop();

    public void PlaceInFront()
    {
        this.canvas.sortingOrder = isBlocking ? BlockingLayer : ForegroundLayer++;

        // Reset foreground layer if too many elements have been placed there
        if (ForegroundLayer == BlockingLayer - 1)
            ForegroundLayer = BackgroundLayer + 1;
    }
    public void PlaceInBack() => this.canvas.sortingOrder = BackgroundLayer;

    public void Display() => this.canvas.gameObject.SetActive(true);
    public void Hide() => this.canvas.gameObject.SetActive(false);
}
