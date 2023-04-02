using System.Collections;
using UnityEngine;

namespace HUD.Effects
{
    public class HitMarkerEffect: HUDEffect
    {
        [SerializeField]private GameObject hitMarker;
        
        public override void Show()
        {
            StartCoroutine(ShowHitMarker());
        }
        
        private IEnumerator ShowHitMarker()
        {
            hitMarker.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            hitMarker.SetActive(false);
        }
    }
}