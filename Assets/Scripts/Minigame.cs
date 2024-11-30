using UnityEngine;

public class Minigame : Screen
{
    public int auraPointsToWin;
    public int auraPointsToLose;

    [Tooltip("Minimum number of seconds before minigame can restart")]
    public int frequencyMin;
    [Tooltip("Maximum number of seconds before minigame can restart")]
    public int frequencyMax;
    [Tooltip("How many seconds do you have to complete the minigame? (set to -1 for unlimited)")]
    public int timeout;

    public AudioClip OnStartSound;
    public AudioClip OnFailSound;
    public AudioClip OnSuccessSound;

    public override void Stop() => OnStop();
    public override void Play() => OnPlay();
    public override void Pause() => OnPause();

    private int callTimer;
    private float startTime = 0.0f;
    private float lastCallTime;

    public override void OnEnable()
    {
        base.OnEnable();
        this.lastCallTime = this.frequencyMin;
    }

    protected virtual void OnPause()
    {
        this.isPlaying = false;
    }

    protected virtual void OnPlay()
    {
        this.isPlaying = true;
        AudioManager.PlaySound(this.OnStartSound);
        this.PlaceInFront();
        this.Display();
        callTimer = Random.Range(frequencyMin, frequencyMax);
    }

    protected virtual void OnStop()
    {
        this.isPlaying = false;
        this.Hide();
        this.PlaceInBack();
        this.lastCallTime = Time.time;
    }

    public virtual void Win()
    {
        this.Stop();
        AudioManager.PlaySound(this.OnSuccessSound);
        AudioManager.PlayWinSound();
        GameManager.AddAuraPoints(this.auraPointsToWin);
    }

    public virtual void Fail()
    {
        this.Stop();
        AudioManager.PlaySound(this.OnFailSound);
        AudioManager.PlayFailSound();
        GameManager.AddAuraPoints(-this.auraPointsToLose);
    }

    public virtual void OnUpdate() { }

    void Update()
    {
        if (this.isPlaying
            && timeout > 0
            && Time.time - startTime >= timeout)
        {
            this.Fail();
        }

        else if (Time.time - lastCallTime >= callTimer)
        {
            this.Play();
            lastCallTime = Time.time;
            startTime = Time.time;
            callTimer = Random.Range(frequencyMin, frequencyMax);
        }

        if (this.isPlaying)
            OnUpdate();
    }
}
