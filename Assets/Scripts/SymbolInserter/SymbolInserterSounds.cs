using Mirror;
using UnityEngine;

public class SymbolInserterSounds : NetworkBehaviour
{
    [SerializeField] private SymbolInserter symbolInserter;
    [SerializeField] private AudioSource wrongAnswerSound;
    [SerializeField] private AudioSource correctAnswerSound;

    private void Start()
    {
        symbolInserter.onCorrectSymbol.AddListener(PlayCorrectAnswerSound);
        symbolInserter.onWrongSymbol.AddListener(PlayWrongAnswerSound);
    }

    private void PlayCorrectAnswerSound()
    {
        PlayCorrectAnswerSoundRpc();
    }

    private void PlayWrongAnswerSound()
    {
        PlayWrongAnswerSoundRpc();
    }

    [Command]
    private void PlayCorrectAnswerSoundCommand()
    {
        PlayCorrectAnswerSoundRpc();
    }
    
    [Command]
    private void PlayWrongAnswerSoundCommand()
    {
        PlayWrongAnswerSoundRpc();
    }

    [ClientRpc]
    private void PlayCorrectAnswerSoundRpc()
    {
        correctAnswerSound.Play();
    }
    
    [ClientRpc]
    private void PlayWrongAnswerSoundRpc()
    {
        wrongAnswerSound.Play();
    }
}