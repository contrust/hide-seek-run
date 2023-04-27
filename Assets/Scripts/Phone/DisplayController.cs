using UnityEngine;

namespace Phone
{
    public class DisplayController: MonoBehaviour
    {
        [SerializeField] private GameObject messagesScreen;
        [SerializeField] private GameObject symbolsScreen;

        public void ShowSymbolsScreen()
        {
            messagesScreen.SetActive(false);
            symbolsScreen.SetActive(true);
        }
        
        public void ShowMessagesScreen()
        {
            messagesScreen.SetActive(true);
            symbolsScreen.SetActive(false);
        }
    }
}