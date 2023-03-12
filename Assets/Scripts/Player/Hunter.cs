using System.Collections;
using System.Collections.Generic;
using Mirror;
using Transport;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    private CustomNetworkManager networkManager;
    [SerializeField] private GameObject victim;
    
    
    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
        networkManager.OnClientConnected += OnClientConnected;
        networkManager.playerPrefab = victim;
    }
    
    private void OnClientConnected()
    {
        Debug.Log("Client connected");
    }
}
