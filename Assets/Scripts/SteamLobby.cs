using Mirror;
using Steamworks;
using UnityEngine;

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

        public CSteamID LobbyId { get; private set; }
        
        private void Start()
        {
            networkManager = GetComponent<CustomNetworkManager>();
            if (!SteamManager.Initialized) return;
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            Debug.Log("hosted");
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
            button.SetActive(false);
            // SensitivitySetActive(false);
        }

        public void LeaveLobby()
        {
            SteamMatchmaking.LeaveLobby(LobbyId);
            if (NetworkClient.activeHost)
                NetworkServer.Shutdown();
            NetworkClient.Shutdown();
            button.SetActive(true);
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
            if (button) 
                button.SetActive(false);
            // SensitivitySetActive(false);
        }

        // private void SensitivitySetActive(bool setActive)
        // {
        //     foreach(Transform child in slider.transform) 
        //         child.gameObject.SetActive(setActive);
        // }
    }
}