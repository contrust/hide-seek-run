using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class TrapInfo: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite trapReloadImage;
        [SerializeField] private Sprite trapReadyImage;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void ShowTrapReady()
        {
            image.sprite = trapReadyImage;
        }

        public void ShowTrapReload()
        {
            image.sprite = trapReloadImage;
        }
    }
}