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
    
    [ClientRpc]
    private void PlayPressedButtonSound()
    {
        buttonSound.Play();
    }
}