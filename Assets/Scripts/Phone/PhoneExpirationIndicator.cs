using System.Collections;
using Symbols;
using UnityEngine;

namespace Phone
{
    public class PhoneExpirationIndicator: MonoBehaviour
    {
        [SerializeField] private MeshRenderer expirationSignal;
        [SerializeField] private Color expireSoonColor;
        [SerializeField] private Color expireAfterSomeTimeColor;
        [SerializeField] private Color expireNotSoonColor;
        [SerializeField] private Color expireBlack;
        [SerializeField] private float blinkingTime = 5;
        [SerializeField] private float changeExpirationSignalTime = -1;
        private SymbolManager symbolManager;
        private MatchSettings matchSettings;

        private void Start()
        {
            matchSettings =  FindObjectOfType<MatchSettings>();
            changeExpirationSignalTime = matchSettings.timeChangeSymbol / 3;
            StartCoroutine(ChangeExpirationSignalColors());
        }

        private IEnumerator ChangeExpirationSignalColors()
        {
            var material = expirationSignal.material;
            while (true)
            {
                material.color = expireNotSoonColor;
                yield return new WaitForSeconds(changeExpirationSignalTime);
                material.color = expireAfterSomeTimeColor;
                yield return new WaitForSeconds(changeExpirationSignalTime);
                material.color = expireSoonColor;
                yield return new WaitForSeconds(changeExpirationSignalTime - blinkingTime);
            
                for (var i = 0; i < blinkingTime; i++)
                {
                    material.color = expireBlack;
                    yield return new WaitForSeconds(0.5f);
                    material.color = expireSoonColor;
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}