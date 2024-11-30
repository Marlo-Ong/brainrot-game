using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdStarter : MonoBehaviour
{
    public GameObject advertisementObject;
    public Button button;
    public int secondsBeforeStartingAds;

    void Start()
    {
        advertisementObject.SetActive(false);
        button.gameObject.SetActive(false);
        button.onClick.AddListener(StartAdvertisements);
        StartCoroutine(WaitToShowButton());
    }

    private IEnumerator WaitToShowButton()
    {
        yield return new WaitForSeconds(this.secondsBeforeStartingAds);
        button.gameObject.SetActive(true);
    }

    private void StartAdvertisements()
    {
        advertisementObject.SetActive(true);
        button.gameObject.SetActive(false);
    }
}
