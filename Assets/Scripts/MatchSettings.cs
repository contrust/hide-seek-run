using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MatchSettings : MonoBehaviour
{
    [Header("Hunter Settings")]
    [Range(0, 1)]
    public float StartHunterBlindness = 1f;
    [Range(0, 1)]
    public float EndHunterBlindness = 0.1f;
    public float DurationHunterBlindnessSeconds = 100;

    [Header("Players")]
    public Hunter Hunter;
    public List<Victim> Victims;

    [Header("Match Settings")]
    public int CountCorrectSymbolsToWin = 3;
}
