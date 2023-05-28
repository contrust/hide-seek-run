using UnityEngine;

namespace UI
{
    public class UIScreen: MonoBehaviour
    {
        public virtual void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}