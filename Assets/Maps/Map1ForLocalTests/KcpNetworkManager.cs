using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class KcpNetworkManager : NetworkManager
{
    public event Action<GameObject> OnServerAddedPlayer = delegate { };
    public event Action OnClientConnected = delegate {  };
    

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
        
        Debug.Log("Connected");

        OnServerAddedPlayer.Invoke(player);
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        OnClientConnected.Invoke();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        OnClientConnected = () => { };
    }
}

