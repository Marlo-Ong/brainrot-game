using UnityEngine.Video;

public class Video : Screen
{
    public VideoPlayer[] videosToPlay;

    public override void Pause()
    {
        this.isPlaying = false;

        foreach (var video in this.videosToPlay)
        {
            video.Pause();
        }
    }

    public override void Play()
    {
        if (this.videosToPlay == null)
            return;

        this.isPlaying = true;
        this.gameObject.SetActive(true);

        foreach (var video in this.videosToPlay)
        {
            video.Play();
        }
    }

    public override void Stop()
    {
        this.isPlaying = false;
        this.gameObject.SetActive(false);

        foreach (var video in this.videosToPlay)
        {
            video.Stop();
        }
    }
}
