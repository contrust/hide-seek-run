using System;
using System.Collections;
using System.Collections.Generic;
using HUD;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField] private Transform rotationX;
    [SerializeField] private Transform rotationY;
    [SerializeField] private float minSlapRotationX;
    [SerializeField] private float maxSlapRotationX;
    [SerializeField] private float minSlapRotationY;
    [SerializeField] private float maxSlapRotationY;
    
    public MeshRenderer SymbolMeshRenderer;

    private void Start()
    {
        networkManager = GameObject.Find("NetworkRoomManager (1)").GetComponent<CustomNetworkManager>();
        //networkManager = GameObject.Find("KcpNetworkManager").GetComponent<KcpNetworkManager>();
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

    public void Slapped()
    {
        var currentRotationY = rotationY.rotation.x;
        if (currentRotationY > 90)
            currentRotationY -= 360;
        var newRotationY = currentRotationY;
        while (Mathf.Abs(currentRotationY - newRotationY) < minSlapRotationY)
            newRotationY = Random.Range(-89, 89);
        var currentRotationX = rotationX.rotation.y;
        var newRotationX = currentRotationX;
        while (Mathf.Abs(currentRotationX - newRotationX) < minSlapRotationX)
            newRotationX = Random.Range(-179, 179);
            // if (currentRotationY > 90)
        //     currentRotationY -= 360;
        // var randomRotationX = Random.Range(minSlapRotationX, maxSlapRotationX);
        // var randomRotationY = Random.Range(minSlapRotationY, maxSlapRotationY);
        // var yRotationDirection = Random.Range(0, 1);
        // if (yRotationDirection > 0.5)
        //     randomRotationY *= -1;
        // var newYRotation = currentRotationY + randomRotationY;
        // if (newYRotation > 90)
        //     newYRotation -= 180;
        // if (newYRotation < -90)
        //     newYRotation += 180;
        // rotationX.Rotate(new Vector3(0, 1), randomRotationX);
        // rotationY.Rotate(new Vector3(1, 0), newYRotation);
        rotationX.rotation = Quaternion.Euler(0, newRotationX, 0);
        rotationY.rotation = Quaternion.Euler(newRotationY, 0, 0);
    }
}
