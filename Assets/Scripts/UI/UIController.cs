using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltip;
        public UIScreen mainMenuUI;
        public UIScreen lobbyUI;
        public UIScreen settingsUI;
        public UIScreen victoryUI;
        public UIScreen activeScreen;
        public UIScreen pauseUI;
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
    }
}
