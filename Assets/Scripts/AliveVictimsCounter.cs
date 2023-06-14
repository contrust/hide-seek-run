using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class AliveVictimsCounter: MonoBehaviour
    {
        [SerializeField] private int victimsCount;
        public static readonly UnityEvent onHunterVictory = new UnityEvent();
        public static AliveVictimsCounter instance;

        private void Awake()
        {
            if (instance is null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            StartCoroutine(GetVictimsCount());
        }

        private IEnumerator GetVictimsCount()
        {
            yield return new WaitForSeconds(3);
            victimsCount = FindObjectsOfType<Victim>().Length;

        }

        public void OnVictimDeathHandler()
        {
            --victimsCount;
            if (victimsCount == 0)
            {
                onHunterVictory.Invoke();
            }
        }
    }
}