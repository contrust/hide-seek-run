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
        
        
        private NetworkManager networkManager;
        private const string HostAddressKey = "HostAddress";
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject slider;
        
        private void Start()
        {
            networkManager = GetComponent<NetworkManager>();
            if (!SteamManager.Initialized) return;
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            Debug.Log("hosted");
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
            Destroy(button);
            SensitivitySetActive(false);
        }


        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }
            networkManager.StartHost();
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
                SteamUser.GetSteamID().ToString());
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
                Destroy(button);
            SensitivitySetActive(false);
        }

        private void SensitivitySetActive(bool setActive)
        {
            foreach(Transform child in slider.transform) 
                child.gameObject.SetActive(setActive);
        }
    }
}