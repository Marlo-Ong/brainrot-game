using System.Collections;
using UnityEngine;

public class Minigame : Screen
{
    [Tooltip("How many 'minigame's worth this game is")]
    public int pointWeight = 1;

    [Tooltip("Minimum number of seconds before minigame can restart")]
    public float frequencyMin;
    [Tooltip("Maximum number of seconds before minigame can restart")]
    public float frequencyMax;
    [Tooltip("How many seconds do you have to complete the minigame? (set to -1 for unlimited)")]
    public bool losePointsOnTimeout = true;
    public float timeout;

    public AudioClip OnStartSound;
    public AudioClip OnFailSound;
    public AudioClip OnSuccessSound;

    public override void Stop() => OnStop();
    public override void Play() => OnPlay();
    public override void Pause() => OnPause();
    public override void Setup() => OnSetup();

    private Coroutine cooldownCoroutine;
    private Coroutine timeoutCoroutine;

    protected virtual void OnSetup()
    {
        this.StartCooldown();
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
        if (this.placeRandomly)
            GameManager.PlaceRandomly(this.canvas.GetComponent<RectTransform>());
        this.Display();

        if (this.timeout > 0)
        {
            if (this.timeoutCoroutine != null)
            {
                StopCoroutine(this.timeoutCoroutine);
                this.timeoutCoroutine = null;
            }
            this.timeoutCoroutine = this.StartCoroutine(StartTimeout());
        }
    }

    protected virtual void OnStop()
    {
        this.isPlaying = false;
        this.Hide();
        this.PlaceInBack();
        this.StartCooldown();
    }

    public virtual void Win()
    {
        this.Stop();
        AudioManager.PlaySound(this.OnSuccessSound);
        AudioManager.PlayWinSound();
        GameManager.AddAuraPoints(this.pointWeight);
    }

    public virtual void Fail()
    {
        this.Stop();
        AudioManager.PlaySound(this.OnFailSound);
        AudioManager.PlayFailSound();
        GameManager.AddAuraPoints(-this.pointWeight);
    }

    public virtual void OnUpdate() { }
    public virtual void OnTimeout() { }

    public void StartCooldown()
    {
        // Start cooldown.
        if (this.cooldownCoroutine != null)
        {
            StopCoroutine(this.cooldownCoroutine);
            this.cooldownCoroutine = null;
        }
        this.cooldownCoroutine = StartCoroutine(ContinueCooldown());
    }

    private IEnumerator ContinueCooldown()
    {
        yield return new WaitForSeconds(Random.Range(frequencyMin, frequencyMax));
        if (!this.isPlaying)
            this.Play();
    }

    private IEnumerator StartTimeout()
    {
        yield return new WaitForSeconds(this.timeout);
        if (this.isPlaying)
        {
            if (this.losePointsOnTimeout)
                this.Fail();
            else
                this.Stop();
            this.OnTimeout();
        }
    }
}
