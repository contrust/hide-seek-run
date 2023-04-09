using System.Collections;
using TMPro;
using UnityEngine;

namespace HUD.Effects
{
    public class SymbolInsertedEffect: HUDEffect
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float alertTime = 4f;
        
        public override void Show()
        {
            StartCoroutine(ShowText());
        }

        private IEnumerator ShowText()
        {
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(alertTime);
            text.gameObject.SetActive(false);
        }
    }
}