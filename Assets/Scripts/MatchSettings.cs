using UnityEngine;

public class MatchSettings : MonoBehaviour
{
    [Header("Hunter Settings")]
    [Range(0, 1)]
    public float StartHunterBlindness = 1f;
    [Range(0, 1)]
    public float EndHunterBlindness = 0.1f;
    public float DurationHunterBlindnessSeconds = 100;

    [Header("Match Settings")]
    public int CountCorrectSymbolsToWin = 3;
    public float timeChangeSymbol = 60f;
}
