using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generators
{
    public class GeneratorsManager : MonoBehaviour
    {
        private List<Generator> generators;
        [SerializeField]
        private int tasksLeft;

        void Start()
        {
            generators = FindObjectsOfType<Generator>().ToList();
            tasksLeft = generators.Count;
        }

        public void TaskFinishedHandler()
        {
            --tasksLeft;
            if (tasksLeft <= 0)
            {
                FinishGame();
            }
        }

        public int GetTasksCount()
        {
            return tasksLeft;
        }

        private void FinishGame(){}
    }
}
