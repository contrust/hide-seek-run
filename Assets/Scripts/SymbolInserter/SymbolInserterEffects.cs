using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class SymbolInserterEffects : NetworkBehaviour
{
    [SerializeField] private SymbolInserter symbolInserter;
    [SerializeField] private GameObject correctAnswerEffect;
    [SerializeField] private Transform correctAnswerEffectTransform;
    [SerializeField] private float correctAnswerEffectDestroyDelayInSeconds;

    private void Start()
    {
        symbolInserter.onCorrectSymbol.AddListener(InstantiateCorrectAnswerEffect);
    }

    private void InstantiateCorrectAnswerEffect()
    {
        InstantiateCorrectAnswerEffectCommand();
    }

    [Command]
    private void InstantiateCorrectAnswerEffectCommand()
    {
        InstantiateCorrectAnswerEffectRpc();
    }

    [ClientRpc]
    private void InstantiateCorrectAnswerEffectRpc()
    {
        var effect = Instantiate(correctAnswerEffect, correctAnswerEffectTransform);
        effect.SetActive(true);
        Destroy(effect, correctAnswerEffectDestroyDelayInSeconds);
    }
}