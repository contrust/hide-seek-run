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

    private void PlayStunSound(float duration)
    {
        PlayStunSoundCommand(duration);
    }
    
    [Command]
    private void PlayStunSoundCommand(float duration)
    {
        PlayStunSoundRpc(duration);
    }

    [ClientRpc]
    private void PlayStunSoundRpc(float duration)
    {
        stunSound.Play();
    }
}