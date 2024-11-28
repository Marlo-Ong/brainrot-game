using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JohnPork : MonoBehaviour
{
    public Minigame minigame;
    public Slider slider;

    [Tooltip("Minimum number of seconds before John Pork calls again")]
    public int frequencyMin;
    [Tooltip("Maximum number of seconds before John Pork calls again")]
    public int frequencyMax;
    [Tooltip("How many seconds do you have to answer John Pork?")]
    public int timeout;

    private int callTimer;
    private float startTime = 0.0f;
    private float lastCallTime = 0.0f;

    void Start()
    {
        callTimer = Random.Range(frequencyMin, frequencyMax);
    }

    void Update()
    {
        if (minigame.isActiveAndEnabled && minigame.isPlaying)
        {
            // Fail if you don't pick up the phone in time
            if (Time.time - startTime >= timeout)
            {
                minigame.Fail();
                lastCallTime = Time.time;
                slider.value = 0;
            }
        }

        // Wait until John Pork calls
        else if (Time.time - lastCallTime >= callTimer)
        {
            minigame.Play();
            startTime = Time.time;
            callTimer = Random.Range(frequencyMin, frequencyMax);
        }
    }

    public void OnPointerUp()
    {
        if (!minigame.isActiveAndEnabled || !minigame.isPlaying)
            return;

        // Win if you successfully slide to answer
        if (slider.value == slider.maxValue)
        {
            minigame.Win();
            lastCallTime = Time.time;
        }
        slider.value = 0;
    }
}
