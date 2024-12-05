using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum IdleAnimation
{
    Rotate,
    PulseScale,
    FadeAndRaise,
}

public class IdleTextAnimation : MonoBehaviour
{
    public IdleAnimation anim;
    public AnimationCurve _idleCurve;
    public bool _playOnEnable = true;
    public float _duration;
    public bool _loop;
    public bool _fadeOut;
    public float _raiseMagnitude;
    private Vector3 _startingPos;

    void Start()
    {
        _startingPos = transform.localPosition;
    }

    void OnEnable()
    {
        if (this._playOnEnable)
            StartAnimation();
    }

    public void StartAnimation()
    {
        var gameObj = this.gameObject;
        if (gameObj == null || gameObj.activeInHierarchy == false)
            return;

        switch (anim)
        {
            case IdleAnimation.Rotate:
                StartCoroutine(AnimateRotation());
                break;
            case IdleAnimation.PulseScale:
                StartCoroutine(AnimatePulseScale());
                break;
            case IdleAnimation.FadeAndRaise:
                StartCoroutine(AnimateFadeAndRaise());
                break;
        }
    }

    private IEnumerator AnimateRotation()
    {
        float elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            Quaternion newRotation = transform.localRotation;
            newRotation.z = _idleCurve.Evaluate(elapsedTime / _duration);
            transform.localRotation = newRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Quaternion finalRotation = transform.localRotation;
        finalRotation.z = _idleCurve.keys[^1].value;
        transform.localRotation = finalRotation;

        if (_loop) StartAnimation();
        else if (_fadeOut) StartCoroutine(AnimateFadeOut());
    }

    private IEnumerator AnimatePulseScale()
    {
        float elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            float scale = _idleCurve.Evaluate(elapsedTime / _duration);
            transform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float finalScale = _idleCurve.keys[^1].value;
        transform.localScale = new Vector3(finalScale, finalScale, finalScale);

        if (_loop) StartAnimation();
        else if (_fadeOut) StartCoroutine(AnimateFadeOut());
    }

    private IEnumerator AnimateFadeAndRaise()
    {
        float elapsedTime = 0;
        var text = GetComponent<TMP_Text>();
        while (elapsedTime < _duration)
        {
            text.alpha = 1 - (elapsedTime / _duration);
            transform.localPosition = new(transform.localPosition.x, transform.localPosition.y + _raiseMagnitude);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _startingPos;
        gameObject.SetActive(false);
        if (_loop) StartAnimation();
    }

    private IEnumerator AnimateFadeOut()
    {
        float elapsedTime = 0;
        var text = GetComponent<TMP_Text>();
        while (elapsedTime < _duration)
        {
            text.alpha = 1 - (elapsedTime / _duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _startingPos;
        gameObject.SetActive(false);
        text.alpha = 1;
    }
}