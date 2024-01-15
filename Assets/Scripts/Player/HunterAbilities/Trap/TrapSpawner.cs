using Mirror;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

namespace Player.HunterAbilities.Trap
{
    public class TrapSpawner: NetworkBehaviour
    {
        public readonly float reloadTimeInSeconds = 10;

        [SerializeField] private GameObject trapPrefab;
        private StarterAssetsInputs input;
        public UnityEvent trapSpawned;
        public UnityEvent trapReady;

        private bool canSpawn;
        private float lastSpawnTime;
        private float timeFromLastSpawn => Time.time - lastSpawnTime;

        private void Start()
        {
            input = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if(!isLocalPlayer) return;

            if (timeFromLastSpawn > reloadTimeInSeconds)
            {
                if (!canSpawn)
                {
                    canSpawn = true;
                    trapReady.Invoke();
                }
                if (input.setTrap)
                {
                    SpawnTrap();
                }
            }
            input.setTrap = false;
        }

        private void SpawnTrap()
        {
            var trap = Instantiate(trapPrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(trap.gameObject);
            lastSpawnTime = Time.time;
            trapSpawned.Invoke();
            canSpawn = false;
        }
    }
}