using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APBrainRotExam : Minigame
{
    protected override void OnStop()
    {
        CheckWinCondition();
        base.OnStop();
    }

    private void CheckWinCondition()
    {

    }
}
