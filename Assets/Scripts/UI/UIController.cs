using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltip;
        [SerializeField] private UIScreen victoryUI;
        [SerializeField] public UIScreen activeScreen = null;
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
