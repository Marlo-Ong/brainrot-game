using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager singleton;

    [Header("Set In Inspector")]
    public AuraData auraTitles;

    [Header("Do Not Set In Inspector")]
    public int auraPoints;
    public string auraTitle;
    public int indexOfCurrentTitle = 0;

    void Awake()
    {
        Debug.Assert(singleton == null);
        singleton = this;

        // Give lowest title.
        this.auraTitle = this.auraTitles.titles[0];
    }

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("touched!");
        }

        if (indexOfCurrentTitle < this.auraTitles.titles.Length &&                  // haven't reached highest title
            this.auraPoints >= this.auraTitles.thresholds[indexOfCurrentTitle + 1]  // reached next threshold
        )
            UpgrateTitle();
    }

    public void UpgrateTitle()
    {
        this.auraTitle = this.auraTitles.titles[++indexOfCurrentTitle];


    }
}
