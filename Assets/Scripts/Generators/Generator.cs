using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Generators
{
    public class Generator : MonoBehaviour, IInteractable
    {
        public UnityEvent onStateChanged;
        
        private new Renderer renderer;
        private readonly Dictionary<GeneratorState, Color> stateColors = new Dictionary<GeneratorState, Color>
        {
            { GeneratorState.TaskFinished, Color.green },
            { GeneratorState.WorkInProgress, Color.yellow },
            { GeneratorState.TaskNotFinished, Color.black },
        };
        [SerializeField] private GeneratorState state;
        [SerializeField] private IGeneratorTask generatorTask;
        private GeneratorState State
        {
            get => state;
            set
            {
                state = value;
                onStateChanged.Invoke();
            }
        }
        private enum GeneratorState
        {
            TaskNotFinished,
            WorkInProgress,
            TaskFinished, 
        }
        
        private void Start()
        {
            renderer = GetComponent<Renderer>();
            generatorTask = GetComponentInChildren<IGeneratorTask>();
        }
        
        public void Interact()
        {
            if(State != GeneratorState.TaskNotFinished) return;
            State = GeneratorState.WorkInProgress;
            generatorTask.ShowTaskWindow();
        }

        public string GetDescription()
        {
            return State != GeneratorState.TaskNotFinished ? "" : "Press [F] to do task";
        }

        public void OnTaskFinishedHandler()
        {
            generatorTask.HideTaskWindow();
            State = GeneratorState.TaskFinished;
        }

        public void OnTaskAbortedHandler()
        {
            generatorTask.HideTaskWindow();
            State = GeneratorState.TaskNotFinished;
        }
        
        public void OnTaskStateChangedHandler()
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            renderer.material.color = stateColors[State];
        }
    }
}
