using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomNetworkManager : NetworkRoomManager
{
    public event Action<GameObject> OnServerAddedPlayer = delegate { };
    public event Action OnClientConnected = delegate {  };

    [FormerlySerializedAs("MatchController")]
    public MatchSettings MatchSettings;

    // public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    // {
    //     Transform startPos = GetStartPosition();
    //     GameObject player = startPos != null
    //         ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
    //         : Instantiate(playerPrefab);
    //     player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
    //     NetworkServer.AddPlayerForConnection(conn, player);
    //     
    //     Debug.Log("Connected");
    //     
    //     var isHunter = player.GetComponent<Hunter>() is not null;
    //     if (isHunter) 
    //         MatchSettings.Hunter = player.GetComponent<Hunter>();
    //     else 
    //         MatchSettings.Victims.Add(player.GetComponent<Victim>());
    //
    //     OnServerAddedPlayer.Invoke(player);
    // }

    // public override void OnClientSceneChanged()
    // {
    //     base.OnClientSceneChanged();
    //     OnClientConnected.Invoke();
    // }
    //
    // public override void OnStartServer()
    // {
    //     base.OnStartServer();
    //     OnClientConnected = () => { };
    // }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        
        var isHunter = player.GetComponent<Hunter>() is not null;
        if (isHunter) 
            MatchSettings.Hunter = player.GetComponent<Hunter>();
        else 
            MatchSettings.Victims.Add(player.GetComponent<Victim>());
        return player;
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
            Debug.Log("start");
            // InitialSpawn();
    }

    private void InitialSpawn()
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        
        var isHunter = player.GetComponent<Hunter>() is not null;
        if (isHunter) 
            MatchSettings.Hunter = player.GetComponent<Hunter>();
        else 
            MatchSettings.Victims.Add(player.GetComponent<Victim>());

        OnServerAddedPlayer.Invoke(player);
    }
    
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        return true;
    }
    
    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();
    }

    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();
    }
    
    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
        showStartButton = true;
#endif
    }

    public override void OnGUI()
    {
        base.OnGUI();

        if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        {
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }
}
