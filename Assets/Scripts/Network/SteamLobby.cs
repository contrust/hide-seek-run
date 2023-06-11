using Mirror;
using Network;
using Steamworks;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Transport
{
    public class SteamLobby : MonoBehaviour
    {
        //Callbacks
        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequested;
        protected Callback<LobbyEnter_t> LobbyEntered;
        
        
        private CustomNetworkRoomManager networkRoomManager;
        private const string HostAddressKey = "HostAddress";
        private string pchValue;
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject slider;
        private UIController uiController;
        private readonly UnityEvent onHostLobby = new ();
        private readonly UnityEvent onLeaveLobby = new ();
        private readonly UnityEvent onEnterLobby = new ();

        public CSteamID LobbyId { get; private set; }
        
        private void Start()
        {
            uiController = FindObjectOfType<UIController>();
            networkRoomManager = GetComponent<CustomNetworkRoomManager>();
            if (!SteamManager.Initialized) return;
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            AddListeners();
        }

        private void AddListeners()
        {
            onLeaveLobby.AddListener(uiController.OnLeaveLobbyHandler);
            onLeaveLobby.AddListener(() =>
            {
                SceneManager.LoadScene("OfflineScene");
            });
            onHostLobby.AddListener(uiController.OnHostLobbyHandler);
            onEnterLobby.AddListener(uiController.OnEnterLobbyHandler);
        }

        public void HostLobby()
        {
            Debug.Log("hosted");
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkRoomManager.maxConnections);
            onHostLobby.Invoke();
        }

        public void LeaveLobby()
        {
            SteamMatchmaking.LeaveLobby(LobbyId);
            if (NetworkClient.activeHost)
                NetworkServer.Shutdown();
            NetworkClient.Shutdown();
            onLeaveLobby.Invoke();
        }


        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            LobbyId = new CSteamID(callback.m_ulSteamIDLobby);
            networkRoomManager.StartHost();
            pchValue = SteamUser.GetSteamID().ToString();
            SteamMatchmaking.SetLobbyData(LobbyId, HostAddressKey,
                pchValue);
            networkRoomManager.lobbyID = LobbyId;
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active) return;
            networkRoomManager.networkAddress =
                SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            networkRoomManager.StartClient();
            onEnterLobby.Invoke();
        }
    }
}