using Mirror;
using Symbols.Inserter;
using UnityEngine;

namespace Symbols.Effects
{
    public class SymbolInserterEffects : NetworkBehaviour
    {
        [SerializeField] private SymbolInserter symbolInserter;
        [SerializeField] private GameObject correctAnswerEffect;
        [SerializeField] private Transform correctAnswerEffectTransform;
        [SerializeField] private float correctAnswerEffectDestroyDelayInSeconds;

        private void Start()
        {
            symbolInserter.onCorrectSymbol.AddListener(InstantiateCorrectAnswerEffectCommand);
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
}