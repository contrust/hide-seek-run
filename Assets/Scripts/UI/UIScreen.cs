using UnityEngine;

namespace UI
{
    public class UIScreen: MonoBehaviour
    {
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}