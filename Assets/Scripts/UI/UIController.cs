using DefaultNamespace;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltip;
        [SerializeField] private VictoryUIScreen victoryUI;
        [SerializeField] public UIScreen activeScreen = null;
        [SerializeField] private UIScreen pauseUI;
        public static UIController instance;
        
        public bool isPause { get; private set; }

        private void Start()
        {
            SymbolManager.OnVictimsVictory.AddListener(ShowVictimsVictoryScreen);
            AliveVictimsCounter.onHunterVictory.AddListener(ShowHunterVictoryScreen);
            GameTimer.OnTimeIsOver.AddListener(ShowVictimsVictoryScreen);
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
            if(activeScreen == victoryUI) return;
            if (isPause)
            {
                HideUIScreen(pauseUI);
                CursorController.ForcedHideCursor();
            }
            else
            {
                ShowUIScreen(pauseUI);
                CursorController.ForcedShowCursor();
            }
            isPause = !isPause;
        }

        private void ShowVictimsVictoryScreen()
        {
            if(activeScreen == victoryUI) return;
            ShowUIScreen(victoryUI);
            victoryUI.SetWinner(Winner.Victims);
            CursorController.ForcedShowCursor();
        }

        private void ShowHunterVictoryScreen()
        {
            if(activeScreen == victoryUI) return;
            ShowUIScreen(victoryUI);
            victoryUI.SetWinner(Winner.Hunter);
            CursorController.ForcedShowCursor();
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
            if(screen is null) return;
            screen.SetActive(false);
            if (screen == activeScreen)
            {
                activeScreen = null;
            }
        }
    }
}
