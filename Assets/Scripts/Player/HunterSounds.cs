using Mirror;
using UnityEngine;

public class HunterSounds : NetworkBehaviour
{
    [SerializeField] private Hunter hunter;
    [SerializeField] private AudioSource stunSound;

    private void Start()
    {
        hunter.onStunned.AddListener(PlayStunSound);
    }

    [ClientRpc]
    private void PlayStunSound(float duration)
    {
        stunSound.Play();
    }
}