using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JohnPork : Minigame
{
    public Slider slider;

    public override void OnStop()
    {
        base.OnStop();
        slider.value = 0;
    }

    // Called by Slider OnPointerUp
    public void CheckSliderValue()
    {
        if (slider.value == slider.maxValue)
            this.Win();
        slider.value = 0;
    }
}
