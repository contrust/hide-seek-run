using System.Collections;
using Mirror;
using UnityEngine;

namespace Player.HunterAbilities.Trap
{
    [RequireComponent(typeof(Animator))]
    public class Trap: NetworkBehaviour
    {
        private Animator animator;
        private static readonly int CatchHash = Animator.StringToHash("Catch");
        private bool isActive = true;
        [SerializeField] private float stunTimeInSeconds = 5; 

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isActive) return;
            var victim = other.gameObject.GetComponent<Victim>();
            if (victim != null)
            {
                HitVictim(victim);
            }
        }

        private void HitVictim(Victim victim)
        {
            Debug.Log("Trigger trap");
            RunAnimation();
            victim.GetStun(stunTimeInSeconds);
            isActive = false;
            StartCoroutine(DestroyCoroutine());
        }

        [Command(requiresAuthority = false)]
        private void RunAnimation()
        {
            animator.SetTrigger(CatchHash);
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(stunTimeInSeconds);
            NetworkServer.Destroy(gameObject);
        }
    }
}