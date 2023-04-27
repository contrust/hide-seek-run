using System;
using UnityEngine;
using UnityEngine.Events;

namespace Phone
{
    [RequireComponent(typeof(Collider))]
    public class Interact: MonoBehaviour
    {
        public UnityEvent onClicked;

        private void OnMouseDown()
        {
            Debug.Log("Button clicked");
            onClicked.Invoke();
        }
    }
}