using Transport;
using UnityEngine;

namespace UI
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