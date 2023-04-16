using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.Examples.NetworkRoom;
using Steamworks;
using Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CustomNetworkManager : NetworkRoomManager
{
    public static event Action OnSceneLoadedForPlayer = delegate {  };

    private Hunter hunter;
    private List<Victim> victims;
    public CSteamID lobbyID;

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        return player;
    }

    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        OnSceneLoadedForPlayer?.Invoke();
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
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

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        var newRoomGameObject = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
        var networkRoomPlayer = newRoomGameObject.GetComponent<NetworkRoomPlayerExt>();
        var playerSteamId = SteamMatchmaking.GetLobbyMemberByIndex(lobbyID, numPlayers);
        networkRoomPlayer.steamName = SteamFriends.GetFriendPersonaName(playerSteamId);
        return newRoomGameObject;
    }
}
