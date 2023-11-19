using System;
using System.Collections;
using HUD;
using Mirror;
using Network;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
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
    private FirstPersonController firstPersonController;
    private bool paused;
    
    [SerializeField] private GameObject victim;
    [SerializeField] private Material darkSkybox;
    [SerializeField] private Material lightSkybox;
    [SerializeField] private Camera overlayCamera;
    [SerializeField] private Color fogColor;
    [SerializeField] private float slapCoolDown;
    private float slapCoolDownTimeLeft;
    [SerializeField] private Transform rotationX;
    [SerializeField] private Transform rotationY;
    
    public UnityEvent<float> onStunned;

    [SerializeField] private MeshRenderer symbolMeshRendererFront;
    [SerializeField] private MeshRenderer symbolMeshRendererBack;

    private void Start()
    {
        networkManager = GameObject.Find("NetworkRoomManager (1)").GetComponent<CustomNetworkRoomManager>();
        //networkManager = GameObject.Find("KcpNetworkManager").GetComponent<KcpNetworkManager>();
        matchSettings = FindObjectOfType<MatchSettings>();
        firstPersonController = GetComponent<FirstPersonController>();
        if (isLocalPlayer) Init();
    }

    private void Update()
    {
        if (slapCoolDownTimeLeft > 0)
            slapCoolDownTimeLeft -= Time.deltaTime;
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
        HUDController.instance.SetupHUD();
        SymbolManager.OnSymbolInserted.AddListener(OnSymbolInsertedHandler);
    }

    public void SetLight()
    {
        RenderSettings.fogDensity = 0.015f;
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
        if (slapCoolDownTimeLeft <= 0)
        {
            onStunned.Invoke(slapCoolDown);
            slapCoolDownTimeLeft = slapCoolDown;
            var currentRotation = new Vector3(rotationX.rotation.x, rotationY.rotation.y, 0);
            var newRotation = new Vector3(Random.Range(-89, 89), Random.Range(0, 360), 0);

            while (Vector3.Angle(currentRotation, newRotation) < 60)
                newRotation = new Vector3(Random.Range(-89, 89), Random.Range(0, 360), 0);

            rotationY.eulerAngles = new Vector3(0, newRotation.y, 0);
            rotationX.localEulerAngles = new Vector3(newRotation.x, 0, 0);
            firstPersonController.cinemachineTargetPitch =
                newRotation.x; // Без этого любое движение мыши возвращает вертикальное положение камеры в исходное
        }
    }

    public void SetSymbol(Material material)
    {
        symbolMeshRendererFront.material = material;
        symbolMeshRendererBack.material = material;
    }
}
