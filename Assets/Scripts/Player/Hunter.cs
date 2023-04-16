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

    [SerializeField] private float blindness;
    private Coroutine blindnessCoroutine;
    [SerializeField] private float victimsProgress = 0;
    private const float VictimsProgressStep = 0.2f;
    private NetworkManager networkManager;
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
        #if UNITY_EDITOR
            networkManager = GameObject.Find("KcpNetworkManager").GetComponent<KcpNetworkManager>();      //Для локальных тестов
        #else
            networkManager = GameObject.Find("NetworkRoomManager (1)").GetComponent<CustomNetworkManager>(); 
        #endif
        matchSettings = FindObjectOfType<MatchSettings>();
        if (isLocalPlayer) Init();
    }

    private void Init()
    {
        if (networkManager is null)
        {
            Debug.Log("networkManager is null");
        }
        networkManager.playerPrefab = victim;
        blindness = 1;
        SetDark();
        blindnessCoroutine = StartCoroutine(BlindnessCoroutine());
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
        Camera.main.cullingMask = Render;
        HUDController.instance.ShowStaticElements();
        HUDController.instance.SetupEventHandlers();
        SymbolManager.OnSymbolInserted.AddListener(OnSymbolInsertedHandler);
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
            Blindness = Mathf.Lerp(matchSettings.StartHunterBlindness, matchSettings.EndHunterBlindness, Math.Min(1, time+victimsProgress));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnSymbolInsertedHandler(bool isCorrect)
    {
        if (isCorrect)
        {
            victimsProgress += VictimsProgressStep;
        }
    }
}
