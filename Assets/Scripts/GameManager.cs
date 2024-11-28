using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager singleton;

    [Header("Set In Inspector")]
    public AuraData auraData;

    [Header("Do Not Set In Inspector")]
    public int auraPoints;
    public string auraTitle;
    public int indexOfCurrentTitle = 0;

    void Awake()
    {
        Debug.Assert(singleton == null);
        singleton = this;

        // Give lowest title.
        this.auraTitle = this.auraData.titles[0];
    }

    public void Update()
    {
        if (indexOfCurrentTitle < this.auraData.titles.Length &&                  // haven't reached highest title
            this.auraPoints >= this.auraData.thresholds[indexOfCurrentTitle + 1]  // reached next threshold
        )
            UpgrateTitle();
    }

    public void UpgrateTitle()
    {
        this.auraTitle = this.auraData.titles[++indexOfCurrentTitle];


    }

    public static void AddAuraPoints(int pts)
    {
        // Clamp new aura points to the smallest and largest thresholds.
        singleton.auraPoints = math.clamp(
            singleton.auraPoints + pts,
            singleton.auraData.thresholds[0],
            singleton.auraData.thresholds[^1]);
    }
}
