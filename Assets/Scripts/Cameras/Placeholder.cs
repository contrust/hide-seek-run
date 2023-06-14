using UnityEngine;

namespace Cameras
{
    public class Placeholder: MonoBehaviour
    {
        [SerializeField] private GameObject img;
        
        public void Show()
        {
            img.SetActive(true);
        }

        public void Hide()
        {
            img.SetActive(false);
        }
    }
}