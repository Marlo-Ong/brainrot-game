using UnityEngine;

[CreateAssetMenu(fileName = "NewAuraData", menuName = "ScriptableObjects/AuraData", order = 1)]
public class AuraData : ScriptableObject
{
    [Tooltip("Name of the title.")]
    public string[] titles;

    [Tooltip("Aura points needed to reach this title. Must be higher than the previous entry.")]
    public int[] thresholds;
    [Tooltip("How many minigames must be won (after the previous title) to reach the this title")]
    public int[] minigamesUntilTitle;
    [Tooltip("Activate a GameObject once this title has been reached. (optional)")]
    public GameObject[] objectToActivateOnUpgrade;

    [Tooltip("Sound to play when upgrading to this title. Can be null, but must match index of above.")]
    public AudioClip[] sounds;
}
