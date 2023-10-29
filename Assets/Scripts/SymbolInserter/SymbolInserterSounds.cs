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

    [ClientRpc]
    private void PlayCorrectAnswerSound()
    {
        correctAnswerSound.Play();
    }
    
    [ClientRpc]
    private void PlayWrongAnswerSound()
    {
        wrongAnswerSound.Play();
    }
}