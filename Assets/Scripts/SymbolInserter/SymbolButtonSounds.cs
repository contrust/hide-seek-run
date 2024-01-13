using Mirror;
using UnityEngine;


public class SymbolButtonSounds : NetworkBehaviour
{
    [SerializeField] private SymbolButton symbolButton;
    [SerializeField] private AudioSource buttonSound;
    

    private void Start()
    {
        symbolButton.Pressed.AddListener(PlayPressedButtonSound);
    }

    private void PlayPressedButtonSound()
    {
        PlayPressedButtonSoundRpc();
    }
    
    [Command]
    private void PlayPressedButtonSoundCommand()
    {
        PlayPressedButtonSoundRpc();
    }
    
    [ClientRpc]
    private void PlayPressedButtonSoundRpc()
    {
        buttonSound.Play();
    }
}