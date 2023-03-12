using System.Collections;
using System.Collections.Generic;
using Mirror;
using Transport;
using UnityEngine;

public class Hunter : NetworkBehaviour
{
    [Range(0, 1)]
    public float Blindness = 1f;
    
    private CustomNetworkManager networkManager;
    [SerializeField] private GameObject victim;
    [SerializeField] private Material skybox;


    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
        networkManager.OnClientConnected += OnClientConnected;
        if (isLocalPlayer) Init();
    }

    private void Init()
    {
        networkManager.playerPrefab = victim;
        RenderSettings.fogDensity = 1;
        RenderSettings.fogColor = Color.black;
        RenderSettings.skybox = skybox;
    }

    private void OnClientConnected()
    {
        Debug.Log("Client connected");
    }
}
