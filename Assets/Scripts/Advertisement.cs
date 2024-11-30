using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Advertisement : MonoBehaviour
{
    public int auraPointsToWin;
    public int auraPointsToLose;

    public VideoClip[] clipsToPlay;
    public RenderTexture[] correspondingTextures;
    public GameObject playerPrefab;
    public RectTransform canvasRectTransform;

    [Tooltip("Minimum number of seconds before ad can play")]
    public int frequencyMin;
    [Tooltip("Maximum number of seconds before ad can play")]
    public int frequencyMax;

    public AudioClip OnStartSound;
    public AudioClip OnSuccessSound;

    void Start()
    {
        StartNewAd();
    }


    private void PlaceRandomly(RectTransform adTransform)
    {
        // Get the Canvas dimensions (sizeDelta is in local space for the RectTransform)
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // Get the size of the UI element
        Vector2 uiElementSize = adTransform.sizeDelta;

        // Calculate the random position bounds, ensuring the UI element stays fully within the Canvas
        float xMin = -canvasSize.x / 2 + uiElementSize.x / 2;
        float xMax = canvasSize.x / 2 - uiElementSize.x / 2;

        float yMin = -canvasSize.y / 2 + uiElementSize.y / 2;
        float yMax = canvasSize.y / 2 - uiElementSize.y / 2;

        // Generate random positions within the adjusted bounds
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        // Set the UI element's anchored position
        adTransform.anchoredPosition = new Vector2(randomX, randomY);
    }

    public void StartNewAd()
    {
        AudioManager.PlaySound(this.OnStartSound);

        // Create a new ad
        var newAdObject = Instantiate(playerPrefab);
        newAdObject.transform.SetParent(this.transform);
        this.PlaceRandomly(newAdObject.GetComponent<RectTransform>());

        var videoComponent = newAdObject.GetComponent<Video>();
        videoComponent.PlaceInFront();

        var imageComponent = videoComponent.gameObject.GetComponentInChildren<RawImage>();
        var sliderComponent = videoComponent.gameObject.GetComponentInChildren<Slider>();
        var newAd = videoComponent.gameObject.GetComponentInChildren<VideoPlayer>();

        int i = Random.Range(0, clipsToPlay.Length);
        newAd.clip = clipsToPlay[i];
        imageComponent.texture = correspondingTextures[i];
        newAd.targetTexture = correspondingTextures[i];

        newAd.Play();
        StartCoroutine(DestroyAd((float)newAd.length, newAdObject));
        StartCoroutine(UpdateSlider(sliderComponent, newAd));
    }

    IEnumerator DestroyAd(float timer, GameObject ad)
    {
        yield return new WaitForSeconds(timer);
        Destroy(ad);

        yield return new WaitForSeconds(Random.Range(frequencyMin, frequencyMax));
        StartNewAd();
    }

    IEnumerator UpdateSlider(Slider slider, VideoPlayer ad)
    {
        while (ad != null && ad.time < ad.length)
        {
            slider.value = (float)(ad.time / ad.length);
            yield return null;
        }
    }

    public virtual void Win()
    {
        AudioManager.PlaySound(this.OnSuccessSound);
        GameManager.AddAuraPoints(this.auraPointsToWin);
    }
}
