using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI survivorsVictoryText;
        [SerializeField] private TextMeshProUGUI tooltip;
        [SerializeField] private UIScreen mainMenuUI;
        [SerializeField] private UIScreen lobbyUI;
        [SerializeField] private UIScreen settingsUI;
        [SerializeField] private UIScreen victoryUI;
        [SerializeField] private UIScreen activeScreen;
        public bool isPause { get; private set; }

        private void Start()
        {
            SymbolManager.OnVictimsVictory.AddListener(ShowVictimsVictoryScreen);
        }

        public void Pause()
        {
            isPause = !isPause;
            SettingsUISetActive(isPause);
        }

        private void SettingsUISetActive(bool setActive)
        {
            settingsUI.SetActive(setActive);
        }

        private void ShowVictimsVictoryScreen()
        {
            survivorsVictoryText.gameObject.SetActive(true);
        }

        public void TooltipSetActive(bool setActive)
        {
            tooltip.gameObject.SetActive(setActive);
        }

        private void MainMenuUISetActive(bool setActive)
        {
            mainMenuUI.SetActive(setActive);
        }

        public void LobbyUISetActive(bool setActive)
        {
            lobbyUI.SetActive(setActive);
        }

        public void LobbyEnterUISetActive(bool setActive)
        {
            MainMenuUISetActive(!setActive);
            LobbyUISetActive(setActive);
        }

        public void ShowUIScreen(UIScreen screen)
        {
            activeScreen?.SetActive(false);
            screen.SetActive(true);
            activeScreen = screen;
        }
    }
}
