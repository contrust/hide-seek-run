using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltip;
        [SerializeField] private UIScreen mainMenuUI;
        [SerializeField] private UIScreen lobbyUI;
        [SerializeField] private UIScreen settingsUI;
        [SerializeField] private UIScreen victoryUI;
        [SerializeField] private UIScreen activeScreen;
        [SerializeField] private UIScreen pauseUI;
        public static UIController instance;
        
        public bool isPause { get; private set; }

        private void Start()
        {
            SymbolManager.OnVictimsVictory.AddListener(ShowVictimsVictoryScreen);
            if (instance is null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void Pause()
        {
            if (isPause)
            {
                HideUIScreen(pauseUI);
            }
            else
            {
                ShowUIScreen(pauseUI);
            }
            isPause = !isPause;
        }

        private void ShowVictimsVictoryScreen()
        {
            victoryUI.SetActive(true);
        }

        public void TooltipSetActive(bool setActive)
        {
            tooltip.gameObject.SetActive(setActive);
        }

        public void ShowUIScreen(UIScreen screen)
        {
            activeScreen?.SetActive(false);
            screen.SetActive(true);
            activeScreen = screen;
        }

        public void HideUIScreen(UIScreen screen)
        {
            screen.SetActive(false);
            if (screen == activeScreen)
            {
                activeScreen = null;
            }
        }

        public void OnLeaveLobbyHandler()
        {
            ShowUIScreen(mainMenuUI);
        }

        public void OnHostLobbyHandler()
        {
            ShowUIScreen(lobbyUI);
        }

        public void OnEnterLobbyHandler()
        {
            ShowUIScreen(lobbyUI);
        }

        public void OnRoomServerSceneChangedRoomSceneHandler()
        {
            ShowUIScreen(lobbyUI);
        }

        public void OnRoomClientDisconnectEventHandler()
        {
            ShowUIScreen(mainMenuUI);
        }

        public void OnRoomClientSceneChangedToGameplaySceneHandler()
        {
            HideUIScreen(lobbyUI);
        }

        public void OnRoomServerSceneLoadedForPlayerHandler()
        {
            HideUIScreen(lobbyUI);
        }
    }
}
