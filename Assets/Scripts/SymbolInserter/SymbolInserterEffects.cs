using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class SymbolInserterEffects : NetworkBehaviour
{
    [SerializeField] private SymbolInserter symbolInserter;
    [SerializeField] private GameObject correctAnswerEffect;
    [SerializeField] private float correctAnswerEffectDestroyDelayInSeconds;

    private void Start()
    {
        symbolInserter.onCorrectSymbol.AddListener(InstantiateCorrectAnswerEffect);
    }

    private void InstantiateCorrectAnswerEffect()
    {
        var effect = Instantiate(correctAnswerEffect, transform);
        effect.SetActive(true);
        Destroy(effect, correctAnswerEffectDestroyDelayInSeconds);
    }
}