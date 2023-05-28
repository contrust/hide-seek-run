using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.Examples.NetworkRoom;
using Steamworks;
using Transport;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CustomNetworkManager : NetworkRoomManager
{
    public static event Action OnSceneLoadedForPlayer = delegate {  };

    private Hunter hunter;
    private List<Victim> victims;
    public CSteamID lobbyID;
    private UIController uiController;

    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
        uiController = FindObjectOfType<UIController>();
    }

    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
        uiController = FindObjectOfType<UIController>();
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        var victim = player.GetComponent<Victim>();
        if (victim)
            victim.steamName = roomPlayer.GetComponent<NetworkRoomPlayerExt>().steamName;
        return player;
    }
    
    

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        OnSceneLoadedForPlayer?.Invoke();
        uiController.LobbyUISetActive(false);
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

    public override void OnRoomClientDisconnect()
    {
        base.OnRoomClientDisconnect();
        uiController.LobbyEnterUISetActive(false);
    }

    public override void OnRoomClientSceneChanged()
    {
        base.OnRoomClientSceneChanged();
        if (networkSceneName == GameplayScene)
            uiController.LobbyUISetActive(false);
    }
}
