using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Generators
{
    public class DummyGeneratorTask: MonoBehaviour, IGeneratorTask
    {
        public UnityEvent onTaskFinished = new UnityEvent();
        public UnityEvent onTaskAborted = new UnityEvent();
        [SerializeField] private int taskTimeInSeconds = 10;
        [SerializeField] private Slider progressBar;
        [SerializeField] private GameObject taskPanel;
        private bool taskInProgress;

        private void Awake()
        {
            progressBar.maxValue = taskTimeInSeconds;
        }
        
        public void ShowTaskWindow()
        {
            taskPanel.SetActive(true);
        }
        
        public void HideTaskWindow()
        {
            taskPanel.SetActive(false);
        }

        public void StartTask()
        {
            if(taskInProgress) return;
            StartCoroutine(TaskProgress());
            taskInProgress = true;
        }

        private void FinishTask()
        {
            onTaskFinished.Invoke();
        }

        public void AbortTask()
        {
            taskInProgress = false;
            progressBar.value = 0;
            StopAllCoroutines();
            onTaskAborted.Invoke();
        }

        private IEnumerator TaskProgress()
        {
            for (var i = 0; i < taskTimeInSeconds; i++)
            {
                yield return new WaitForSeconds(1);
                ++progressBar.value;
            }
            FinishTask();
        }
    }
}