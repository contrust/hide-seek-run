using System;
using System.Collections;
using System.Collections.Generic;
using HUD;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    public LayerMask Render;

    private float blindness;
    private Coroutine blindnessCoroutine;
    private CustomNetworkManager networkManager;
    private MatchSettings matchSettings;
    private bool paused;
    
    [SerializeField] private GameObject victim;
    [SerializeField] private Material darkSkybox;
    [SerializeField] private Material lightSkybox;
    [SerializeField] private Camera overlayCamera;
    [SerializeField] private Color fogColor;
    
    public MeshRenderer SymbolMeshRenderer;

    private void Start()
    {
        networkManager = GameObject.Find("NetworkRoomManager (1)").GetComponent<CustomNetworkManager>();
        matchSettings = FindObjectOfType<MatchSettings>();
        if (isLocalPlayer) Init();
    }

    private void Init()
    {
        networkManager.playerPrefab = victim;
        blindness = 1;
        SetDark();
        blindnessCoroutine = StartCoroutine(BlindnessCoroutine());
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
        Camera.main.cullingMask = Render;
        HUDController.instance.ShowStaticElements();
        HUDController.instance.SetupEventHandlers();
    }

    public void SetLight()
    {
        RenderSettings.fogDensity = 0.025f;
        RenderSettings.fogColor = fogColor;
        RenderSettings.skybox = lightSkybox;
        paused = true;
    }

    public void SetDark()
    {
        RenderSettings.fogDensity = blindness;
        RenderSettings.fogColor = Color.black;
        RenderSettings.skybox = darkSkybox;
        paused = false;
    }

    private IEnumerator BlindnessCoroutine()
    {
        var startTime = Time.time;
        while (Math.Abs(Blindness - matchSettings.EndHunterBlindness) < 1e5)
        {
            if (paused)
            {
                yield return null;
                continue;
            }
            var time = (Time.time - startTime) / matchSettings.DurationHunterBlindnessSeconds;
            Blindness = Mathf.Lerp(matchSettings.StartHunterBlindness, matchSettings.EndHunterBlindness, time);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
