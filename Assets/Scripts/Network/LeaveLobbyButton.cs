using Transport;
using UnityEngine;

namespace Network
{
    public class LeaveLobbyButton: MonoBehaviour
    {
        public void OnClick()
        {
            var lobby = FindObjectOfType<SteamLobby>();
            lobby.LeaveLobby();
        }
    }
}