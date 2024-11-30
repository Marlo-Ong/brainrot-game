using System.Collections;
using TMPro;
using UnityEngine;

public class SpamClickGame : Minigame
{
    public TMP_Text clickCount;

    public int clicksMade;

    [Tooltip("Number of clicks needed to win. (set to -1 for infinite)")]
    public int clicksRequired;
    [Tooltip("Whether to count up from zero or count down to zero.")]
    public bool countsDown;

    [Tooltip("Removes clicks while not clicking.")]
    public bool removeStaleClicks;

    [Tooltip("How many clicks are decreased per second, if removeStaleClicks is on.")]
    public int stalingRate;

    [Tooltip("How many seconds after letting go of the button to begin counting down.")]
    public float staleClickCooldown;

    private bool isStaling;
    private Coroutine stalingCoroutine;

    protected override void OnPlay()
    {
        base.OnPlay();
        clicksMade = this.countsDown ? this.clicksRequired : 0;
        clickCount.text = clicksMade.ToString();
    }

    public void OnClick()
    {
        clicksMade += this.countsDown ? -1 : 1;

        clickCount.text = clicksMade.ToString();
        CheckWinCondition();

        // Reset staling.
        isStaling = false;
        if (stalingCoroutine != null)
        {
            StopCoroutine(stalingCoroutine);
            stalingCoroutine = null;
        }

        StartStaling();
    }

    // Called by Button OnPointerUp
    public void StartStaling()
    {
        // Start countdown timer if countDown is enabled
        if (clicksMade > 0 && removeStaleClicks)
            stalingCoroutine ??= StartCoroutine(ContinueStaling());
    }

    IEnumerator ContinueStaling()
    {
        // Wait x frames after letting go of button.
        yield return new WaitForSeconds(this.staleClickCooldown);
        isStaling = true;

        while (isStaling)
        {
            bool outOfBounds =
                (countsDown && clicksMade >= clicksRequired) ||
                (!countsDown && clicksMade <= 0);

            if (outOfBounds)
                break;

            // Gradually reverse the count by the staling rate.
            if (this.countsDown)
                clicksMade += this.stalingRate;
            else
                clicksMade -= this.stalingRate;

            clickCount.text = clicksMade.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        isStaling = false;
        clicksMade = 0;

        stalingCoroutine = null;
    }

    private void CheckWinCondition()
    {
        // Check win condition.
        bool enoughClicks = countsDown
            ? clicksMade <= 0
            : clicksMade >= clicksRequired;

        if (clicksRequired > 0 && enoughClicks)
        {
            this.Win();
        }
    }

    public override void OnTimeout()
    {
        CheckWinCondition();
    }
}
