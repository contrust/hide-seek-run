using Mirror;
using StarterAssets;
using UnityEngine;

namespace Player.HunterAbilities.Trap
{
    public class TrapSpawner: NetworkBehaviour
    {
        public readonly float reloadTimeInSeconds = 10;

        [SerializeField] private Trap trapPrefab;
        private StarterAssetsInputs input;
        private bool canSpawn => timeFromLastSpawn > reloadTimeInSeconds;
        private float lastSpawnTime;
        private float timeFromLastSpawn => Time.time - lastSpawnTime;

        private void Start()
        {
            input = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if(!isLocalPlayer) return;
            
            if (canSpawn && input.setTrap)
            {
                SpawnTrap();
            }

            input.setTrap = false;
        }

        private void SpawnTrap()
        {
            var trap = Instantiate(trapPrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(trap.gameObject);
            lastSpawnTime = Time.time;
        }
    }
}