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

    [Tooltip("Minimum number of seconds before ad can play")]
    public int frequencyMin;
    [Tooltip("Maximum number of seconds before ad can play")]
    public int frequencyMax;

    public AudioClip OnStartSound;
    public AudioClip OnSuccessSound;

    void OnEnable()
    {
        StartNewAd();
    }




    public void StartNewAd()
    {
        AudioManager.PlaySound(this.OnStartSound);

        // Create a new ad
        var newAdObject = Instantiate(playerPrefab);
        newAdObject.transform.SetParent(this.transform);
        GameManager.PlaceRandomly(newAdObject.GetComponent<RectTransform>());
        newAdObject.SetActive(true);

        var videoComponent = newAdObject.GetComponent<Video>();
        videoComponent.PlaceInFront();

        var imageComponent = videoComponent.gameObject.GetComponentInChildren<RawImage>();
        var sliderComponent = videoComponent.gameObject.GetComponentInChildren<Slider>();
        var newAd = videoComponent.gameObject.GetComponentInChildren<VideoPlayer>();

        int i = Random.Range(0, clipsToPlay.Length);
        newAd.clip = clipsToPlay[i];
        newAd.SetDirectAudioVolume(0, 0.1f);
        imageComponent.texture = correspondingTextures[i];
        newAd.targetTexture = correspondingTextures[i];

        newAd.Play();
        StartCoroutine(DestroyAd((float)newAd.length, newAdObject));
        StartCoroutine(UpdateSlider(sliderComponent, newAd));
    }

    IEnumerator DestroyAd(float timer, GameObject ad)
    {
        yield return new WaitForSeconds(timer);
        Win();
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
