using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager singleton;
    public static Canvas MainCanvas => singleton.mainCanvas;

    [Header("Set In Inspector")]
    public AuraData auraData;
    public Canvas mainCanvas;
    public GameObject gameParent;
    public GameObject winScreen;
    public TMP_Text playTimer;
    public Slider auraProgressBar;
    public IdleTextAnimation middleTextObject;
    public TMP_Text auraTitleSmall;
    public TMP_Text auraTitleLarge;
    public TMP_Text auraPointsText;
    public GameObject[] auraTextPool;
    public AudioClip auraTitleSound;

    [Header("Do Not Set In Inspector")]
    public int auraPoints;
    public string auraTitle;
    public int indexOfCurrentTitle = 0;

    private Coroutine progressBarAnimation;
    private bool isPlaying;

    void Awake()
    {
        Debug.Assert(singleton == null);
        singleton = this;
    }

    public void StartGame()
    {
        // Give lowest title.
        this.auraTitle = this.auraData.titles[0];
        this.auraTitleLarge.text = this.auraTitle;
        this.auraTitleSmall.text = this.auraTitle;

        this.auraPoints = 0;
        this.auraProgressBar.value = 0;
        this.auraPointsText.text = "AURA: 0";
        this.indexOfCurrentTitle = 0;

        this.isPlaying = true;

        this.gameParent.SetActive(true);
    }

    public void EndGame()
    {
        this.gameParent.SetActive(false);

        float currentTime = Time.unscaledTime;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        playTimer.text = $"{minutes:D2}:{seconds:D2} \nminutes";
        winScreen.SetActive(true);

        this.isPlaying = false;
    }

    public void Update()
    {
        if (!this.isPlaying)
            return;

        // Reached highest title
        if (indexOfCurrentTitle == this.auraData.titles.Length - 1)
            EndGame();

        // Reached next threshold
        else if (this.auraPoints >= this.auraData.thresholds[indexOfCurrentTitle + 1])
        {
            UpgrateTitle();
        }
    }

    public void UpgrateTitle()
    {
        this.auraTitle = this.auraData.titles[++indexOfCurrentTitle];

        // Update visuals.
        this.auraTitleLarge.text = this.auraTitle;
        this.auraTitleSmall.text = this.auraTitle;
        this.middleTextObject.StartAnimation();

        // Play sound.
        StartCoroutine(SayAuraTitle());

        // Activate a GameObject for this title, if specified.
        GameObject upgradeObject = this.auraData.objectToActivateOnUpgrade[indexOfCurrentTitle];
        if (upgradeObject != null)
        {
            var newObj = Instantiate(upgradeObject);
            newObj.transform.SetParent(gameParent.transform, false);
        }
    }

    public static void AddAuraPoints(int pointWeight)
    {
        if (pointWeight == 0)
            return;

        var data = singleton.auraData;
        int i = singleton.indexOfCurrentTitle + 1;
        int pts = (int)(pointWeight * ((float)data.thresholds[i] / data.minigamesUntilTitle[i]));

        // Lose only half points.
        if (pts < 0)
            pts /= 2;

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
                PlaceRandomly(txt.GetComponent<RectTransform>());
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
        singleton.auraPointsText.text = $"AURA: {singleton.auraPoints}";
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
            this.auraProgressBar.value = to - 1;
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

    public static void PlaceRandomly(RectTransform transform)
    {
        // Get the Canvas dimensions (sizeDelta is in local space for the RectTransform)
        Vector2 canvasSize = MainCanvas.GetComponent<RectTransform>().sizeDelta;

        // Get the size of the UI element
        Vector2 uiElementSize = transform.sizeDelta;

        // Calculate the random position bounds, ensuring the UI element stays fully within the Canvas
        float xMin = -canvasSize.x / 2 + uiElementSize.x / 2;
        float xMax = canvasSize.x / 2 - uiElementSize.x / 2;

        float yMin = -canvasSize.y / 2 + uiElementSize.y / 2;
        float yMax = canvasSize.y / 2 - uiElementSize.y / 2;

        // Generate random positions within the adjusted bounds
        float randomX = UnityEngine.Random.Range(xMin, xMax);
        float randomY = UnityEngine.Random.Range(yMin, yMax);

        // Set the UI element's anchored position
        transform.anchoredPosition = new Vector2(randomX, randomY);
    }
}
