using System;
using System.Collections.Generic;
using Mirror;
using Network.Lobby;
using Steamworks;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class CustomNetworkRoomManager : NetworkRoomManager
    {
        public static event Action OnSceneLoadedForPlayer = delegate {  };

        private Hunter hunter;
        private List<Victim> victims;
        public CSteamID lobbyID;
        private UIController uiController;
        private readonly UnityEvent onRoomClientDisconnectEvent = new ();
        private readonly UnityEvent onRoomClientSceneChangedGameplayScene = new ();
        private readonly UnityEvent onRoomServerSceneLoadedForPlayerEvent = new ();
        private readonly UnityEvent onRoomServerSceneChangedRoomScene = new ();
        [SerializeField] public GameObject hunterPrefab;
        [SerializeField] public GameObject victimPrefab;

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
            {
                var roomPlayerExt = roomPlayer.GetComponent<NetworkRoomPlayerExt>();
                victim.steamName = roomPlayerExt.steamName;
                roomPlayerExt.SetNewColor();
                victim.color = roomPlayerExt.PlayerColor;
            }
            return player;
        }
    
    

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            OnSceneLoadedForPlayer?.Invoke();
            onRoomServerSceneLoadedForPlayerEvent.Invoke();
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

            if (!NetworkServer.active)
            {
                showStartButton = false;
            }
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
            onRoomClientDisconnectEvent.Invoke();
        }

        public override void OnRoomClientSceneChanged()
        {
            base.OnRoomClientSceneChanged();
            if (networkSceneName == GameplayScene)
                onRoomClientSceneChangedGameplayScene.Invoke();
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            base.OnRoomServerSceneChanged(sceneName);
            if (networkSceneName == RoomScene)
            {
                playerPrefab = hunterPrefab;
            }
            onRoomServerSceneChangedRoomScene.Invoke();
        }

        public override void OnRoomClientExit()
        {
            base.OnRoomClientExit();
        }

        public void PlayAsVictim()
        {
            playerPrefab = victimPrefab;
        }
    }
}
