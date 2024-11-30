using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdCloser : MonoBehaviour
{
    public Button button;
    public int secondsBeforeAllowingExit;
    public int auraPointsToLose;

    void OnEnable()
    {
        button.gameObject.SetActive(false);
        button.onClick.AddListener(CloseAd);
        StartCoroutine(WaitToAllowExit());
    }

    private IEnumerator WaitToAllowExit()
    {
        yield return new WaitForSeconds(this.secondsBeforeAllowingExit);
        button.gameObject.SetActive(true);
    }

    private void CloseAd()
    {
        GameManager.AddAuraPoints(-this.auraPointsToLose);
        Destroy(this.gameObject);
    }
}
