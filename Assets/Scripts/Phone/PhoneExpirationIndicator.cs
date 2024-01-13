using System.Collections;
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
        [SerializeField] private float expireColorIntensity = 5;
        [SerializeField] private float blinkingTime = 5;
        [SerializeField] private float changeExpirationSignalTime = -1;
        private SymbolManager symbolManager;
        private MatchSettings matchSettings;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

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
                material.SetColor(EmissionColor, expireNotSoonColor * expireColorIntensity);
                yield return new WaitForSeconds(changeExpirationSignalTime);
                material.SetColor(EmissionColor, expireAfterSomeTimeColor * expireColorIntensity);
                yield return new WaitForSeconds(changeExpirationSignalTime);
                material.SetColor(EmissionColor, expireSoonColor * expireColorIntensity);
                yield return new WaitForSeconds(changeExpirationSignalTime - blinkingTime);
            
                for (var i = 0; i < blinkingTime; i++)
                {
                    material.SetColor(EmissionColor, expireBlack * expireColorIntensity);
                    yield return new WaitForSeconds(0.5f);
                    material.SetColor(EmissionColor, expireSoonColor * expireColorIntensity);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}