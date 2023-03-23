using System;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Generators
{
    public class Generator : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private bool taskFinished;

        private new Renderer renderer;
        public UnityEvent onTaskFinished;

        private void Start()
        {
            renderer = GetComponent<Renderer>();
        }

        private void FinishTask()
        {
            if(taskFinished) return;
            taskFinished = true;
            renderer.material.color = Color.green;
            onTaskFinished.Invoke();
        }

        public void Interact()
        {
            FinishTask();
        }

        public string GetDescription()
        {
            if (taskFinished) return "";
            return "Press [F] to do task";
        }
    }
}
