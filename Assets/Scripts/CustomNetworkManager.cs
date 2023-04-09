using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CustomNetworkManager : NetworkRoomManager
{
    public event Action<GameObject> OnServerAddedPlayer = delegate { };
    public event Action OnClientConnected = delegate {  };

    private Hunter hunter;
    private List<Victim> victims;

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        // var player = base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        
        var isHunter = player.GetComponent<Hunter>() is not null;
        // if (isHunter) 
        //     hunter = player.GetComponent<Hunter>();
        // else 
        //     victims.Add(player.GetComponent<Victim>());
        return player;
    }

    // public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    // {
    //     var hunter = FindObjectOfType<Hunter>();
    //     var victims = FindObjectsOfType<Victim>();
    //     MatchSettings.Hunter = hunter;
    //     MatchSettings.Victims = victims.ToList();
    //     return true;
    // }
    
    
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
