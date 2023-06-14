using Transport;
using UnityEngine;

namespace Network
{
    public class HostLobbyButton: MonoBehaviour
    {
        public void OnClick()
        {
            var lobby = FindObjectOfType<SteamLobby>();
            lobby.HostLobby();
        }
    }
}