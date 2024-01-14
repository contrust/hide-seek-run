using Mirror;
using UnityEngine;

namespace Player.HunterAbilities.Trap
{
    public class Trap: NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var victim = other.gameObject.GetComponent<Victim>();
            if (victim != null)
            {
                HitVictim(victim);
            }
        }

        private void HitVictim(Victim victim)
        {
            Debug.Log("Trigger trap");
            victim.GetStun();
            NetworkServer.Destroy(gameObject);
        }
    }
}