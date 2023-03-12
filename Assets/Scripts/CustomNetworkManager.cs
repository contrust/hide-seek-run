using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomNetworkManager : NetworkManager
{
    public event Action OnClientConnected = delegate { };
    [FormerlySerializedAs("MatchController")]
    public MatchSettings MatchSettings;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);

        
        var isHunter = player.GetComponent<Hunter>() is not null;
        if (isHunter) 
            MatchSettings.Hunter = player.GetComponent<Hunter>();
        else 
            MatchSettings.Victims.Add(player.GetComponent<Victim>());

        OnClientConnected.Invoke();
    }
}
