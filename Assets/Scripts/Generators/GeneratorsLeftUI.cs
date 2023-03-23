using System;
using TMPro;
using UnityEngine;

namespace Generators
{
    public class GeneratorsLeftUI : MonoBehaviour
    {
        private GeneratorsManager generatorsManager;
        private TextMeshProUGUI countText;

        private void Awake()
        {
            generatorsManager = FindObjectOfType<GeneratorsManager>();
            countText = GetComponent<TextMeshProUGUI>();
            UpdateCount();
        }


        // Update is called once per frame
        void Update()
        {
        
        }

        public void TaskFinishedHandler()
        {
            UpdateCount();
        }

        private void UpdateCount()
        {
            countText.text = $"Generators left: {generatorsManager.GetTasksCount()}";
        }
    }
}
