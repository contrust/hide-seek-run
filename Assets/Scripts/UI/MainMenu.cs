using UnityEngine;

namespace UI
{
    public class MainMenu:MonoBehaviour
    {
        [SerializeField] private UIScreen mainMenuUI;
        [SerializeField] private UIScreen settingsUI;
        [SerializeField] private UIScreen activeScreen;
        
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
        
        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}