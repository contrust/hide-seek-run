using Mirror;
using Steamworks;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Transport
{
    public class SteamLobby : MonoBehaviour
    {
        //Callbacks
        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequested;
        protected Callback<LobbyEnter_t> LobbyEntered;
        
        
        private CustomNetworkManager networkManager;
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
            networkManager = GetComponent<CustomNetworkManager>();
            if (!SteamManager.Initialized) return;
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            AddListeners();
        }

        private void AddListeners()
        {
            onLeaveLobby.AddListener(uiController.OnLeaveLobbyHandler);
            onHostLobby.AddListener(uiController.OnHostLobbyHandler);
            onEnterLobby.AddListener(uiController.OnEnterLobbyHandler);
        }

        public void HostLobby()
        {
            Debug.Log("hosted");
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
            onHostLobby.Invoke();
        }

        public void LeaveLobby()
        {
            SteamMatchmaking.LeaveLobby(LobbyId);
            if (NetworkClient.activeHost)
                NetworkServer.Shutdown();
            NetworkClient.Shutdown();
            onLeaveLobby.Invoke();
            networkManager.ServerChangeScene(networkManager.offlineScene);
        }


        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            LobbyId = new CSteamID(callback.m_ulSteamIDLobby);
            networkManager.StartHost();
            pchValue = SteamUser.GetSteamID().ToString();
            SteamMatchmaking.SetLobbyData(LobbyId, HostAddressKey,
                pchValue);
            networkManager.lobbyID = LobbyId;
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active) return;
            networkManager.networkAddress =
                SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            networkManager.StartClient();
            onEnterLobby.Invoke();
        }
    }
}