using UnityEngine;

namespace DefaultNamespace
{
    public class GameState: MonoBehaviour
    {
        public static GameState instance;
        public bool isVictory;

        private void Start()
        {
            instance = this;
            isVictory = false;
        }
    }
}