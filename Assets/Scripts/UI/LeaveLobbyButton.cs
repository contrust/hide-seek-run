using Transport;
using UnityEngine;

namespace UI
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