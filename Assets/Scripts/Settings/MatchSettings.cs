using UnityEngine;

public class MatchSettings : MonoBehaviour
{
    public MatchSettingsObj matchSettings;
    public float StartHunterBlindness => matchSettings.StartHunterBlindness;
    public float EndHunterBlindness => matchSettings.EndHunterBlindness;
    public float DurationHunterBlindnessSeconds => matchSettings.DurationHunterBlindnessSeconds;
    public int CountCorrectSymbolsToWin => matchSettings.CountCorrectSymbolsToWin;
    public float timeChangeSymbol => matchSettings.timeChangeSymbol;
}
