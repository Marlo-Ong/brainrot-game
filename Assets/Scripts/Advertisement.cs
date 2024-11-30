using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Container for a Video and close-ad Button.
/// This object's Canvas should be a reference to
/// the Video canvas.
/// </summary>
public class Advertisement : Minigame
{
    public Button closeAdButton;
    public int secondsBeforeAllowingExit;
    public VideoClip[] clipsToPlay;
    public RenderTexture[] correspondingTextures;

    private Coroutine endAdCoroutine;

    protected override void OnSetup()
    {
        base.OnSetup();
        closeAdButton.gameObject.SetActive(false);
        closeAdButton.onClick.AddListener(this.CloseAd);
        StartCoroutine(WaitToAllowExit());
    }

    protected override void OnPlay()
    {
        base.OnPlay();

        // Place the ad in a random position on the screen.
        GameManager.PlaceRandomly(canvas.GetComponent<RectTransform>());

        var videoComponent = canvas.GetComponent<Video>();
        var imageComponent = videoComponent.gameObject.GetComponentInChildren<RawImage>();
        var sliderComponent = videoComponent.gameObject.GetComponentInChildren<Slider>();
        var newAd = videoComponent.gameObject.GetComponentInChildren<VideoPlayer>();

        // Make the ad block everything else.
        videoComponent.PlaceInFront();

        // Choose a random video clip.
        int i = Random.Range(0, clipsToPlay.Length);
        newAd.clip = clipsToPlay[i];

        // Lower the audio of the clip.
        newAd.SetDirectAudioVolume(0, 0.1f);

        // Set the image's texture to the video.
        imageComponent.texture = correspondingTextures[i];
        newAd.targetTexture = correspondingTextures[i];

        // Play the clip.
        canvas.gameObject.SetActive(true);
        newAd.Play();

        // Set a timer to end the ad, if not closed by the player first.
        if (endAdCoroutine != null)
            StopCoroutine(endAdCoroutine);
        endAdCoroutine = StartCoroutine(EndAd((float)newAd.length));

        // Update the progress of the slider.
        StartCoroutine(UpdateSlider(sliderComponent, newAd));
    }

    private IEnumerator WaitToAllowExit()
    {
        yield return new WaitForSeconds(this.secondsBeforeAllowingExit);
        closeAdButton.gameObject.SetActive(true);
    }

    private void CloseAd()
    {
        StopCoroutine(endAdCoroutine);
        this.Fail();
    }

    IEnumerator EndAd(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (canvas.isActiveAndEnabled)
            Win();
    }

    IEnumerator UpdateSlider(Slider slider, VideoPlayer ad)
    {
        while (ad != null && ad.time < ad.length)
        {
            slider.value = (float)(ad.time / ad.length);
            yield return null;
        }
    }
}
