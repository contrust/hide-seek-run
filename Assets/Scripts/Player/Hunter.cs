using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Transport;
using UnityEngine;

public class Hunter : NetworkBehaviour
{
    public float Blindness
    {
        get => blindness;
        set
        {
            if (value is > 1 or < 0)
                throw new InvalidOperationException();
            blindness = value;
            if (isLocalPlayer)
                RenderSettings.fogDensity = value;
        }
    }

    private float blindness;
    private Coroutine blindnessCoroutine;
    private CustomNetworkManager networkManager;
    private MatchSettings matchSettings;
    
    [SerializeField] private GameObject victim;
    [SerializeField] private Material skybox;


    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
        networkManager.OnClientConnected += OnClientConnected;
        matchSettings = networkManager.MatchSettings;
        if (isLocalPlayer) Init();
    }

    private void Init()
    {
        networkManager.playerPrefab = victim;
        Blindness = 1;
        RenderSettings.fogColor = Color.black;
        RenderSettings.skybox = skybox;
        blindnessCoroutine = StartCoroutine(BlindnessCoroutine());
    }

    private void OnClientConnected()
    {
        Debug.Log("Client connected");
    }
    
    private IEnumerator BlindnessCoroutine()
    {
        var startTime = Time.time;
        while (Math.Abs(Blindness - matchSettings.EndHunterBlindness) < 1e5)
        {
            var time = (Time.time - startTime) / matchSettings.DurationHunterBlindnessSeconds;
            Blindness = Mathf.Lerp(matchSettings.StartHunterBlindness, matchSettings.EndHunterBlindness, time);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
