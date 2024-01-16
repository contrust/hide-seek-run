using Mirror;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Player.HunterAbilities.Trap
{
    public class TrapSpawner: NetworkBehaviour
    {
        public float reloadTimeInSeconds = 10;

        [SerializeField] public GameObject trapPrefab;
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
            var spawnPosition = transform.position + Vector3.up;
            var spawnRotation = transform.rotation;
            if (trapPrefab.GetComponent<BirdSkill>() != null)
            {
                spawnPosition = transform.position + Vector3.up * 3;
                spawnRotation = quaternion.identity;
            }
            var trap = Instantiate(trapPrefab, spawnPosition,spawnRotation);
            NetworkServer.Spawn(trap.gameObject);
            lastSpawnTime = Time.time;
            trapSpawned.Invoke();
            canSpawn = false;
        }
    }
}