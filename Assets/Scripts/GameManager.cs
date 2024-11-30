using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager singleton;

    [Header("Set In Inspector")]
    public AuraData auraData;
    public GameObject auraTitleObject;
    public Slider auraProgressBar;
    public TMP_Text auraPointsText;
    public GameObject[] auraTextPool;
    public AudioClip auraTitleSound;

    [Header("Do Not Set In Inspector")]
    public int auraPoints;
    public string auraTitle;
    public int indexOfCurrentTitle = 0;

    private Coroutine progressBarAnimation;

    void Awake()
    {
        Debug.Assert(singleton == null);
        singleton = this;

        // Give lowest title.
        this.auraTitle = this.auraData.titles[0];
    }

    public void Update()
    {
        if (indexOfCurrentTitle < this.auraData.titles.Length &&                  // haven't reached highest title
            this.auraPoints >= this.auraData.thresholds[indexOfCurrentTitle + 1]  // reached next threshold
        )
            UpgrateTitle();
    }

    public void UpgrateTitle()
    {
        if (indexOfCurrentTitle == this.auraData.titles.Length - 1)
            return;

        this.auraTitle = this.auraData.titles[++indexOfCurrentTitle];

        // Update visuals.
        this.auraTitleObject.GetComponent<TMP_Text>().text = this.auraTitle;
        this.auraTitleObject.GetComponent<IdleTextAnimation>().StartAnimation();

        // Play sound.
        StartCoroutine(SayAuraTitle());
    }

    public static void AddAuraPoints(int pts)
    {
        // Clamp new aura points to the smallest and largest thresholds.
        singleton.auraPoints = math.clamp(
            singleton.auraPoints + pts,
            singleton.auraData.thresholds[0],
            singleton.auraData.thresholds[^1]);

        // Set the first text in pool active.
        foreach (var txt in singleton.auraTextPool)
        {
            if (!txt.activeInHierarchy)
            {
                txt.GetComponent<TMP_Text>().text = $"{pts:+#;-#;0} AURA";
                txt.SetActive(true);
                break;
            }
        }

        // Update progress bar.
        var oldValue = singleton.auraProgressBar.value;
        var newValue = (float)singleton.auraPoints / singleton.auraData.thresholds[singleton.indexOfCurrentTitle + 1];

        // If upgrading, calculate offset of next bar.
        if (singleton.progressBarAnimation != null)
            singleton.StopCoroutine(singleton.progressBarAnimation);

        singleton.progressBarAnimation = singleton.StartCoroutine(singleton.AnimateProgressBar(oldValue, newValue));

        // Update total aura points text.
        singleton.auraPointsText.text = $"aura points: {singleton.auraPoints}";
    }

    private IEnumerator AnimateProgressBar(float from, float to)
    {
        float duration = 0f;
        while (duration < 1f)
        {
            this.auraProgressBar.value = Mathf.Lerp(from, to, duration);
            duration += Time.deltaTime;
            yield return null;
        }

        // Show progress animation even if upgrading.
        if (to >= 0.99f)
            this.auraProgressBar.value = 0;
        else
            this.auraProgressBar.value = to;

        this.progressBarAnimation = null;
    }

    private IEnumerator SayAuraTitle()
    {
        AudioManager.PlaySound(this.auraTitleSound);

        float startTime = Time.time;

        // Wait one second.
        while (Time.time - startTime < 0.75f)
            yield return null;

        AudioManager.PlaySound(this.auraData.sounds[indexOfCurrentTitle]);
    }
}
