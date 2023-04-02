using System.Collections;
using UnityEngine;

namespace HUD.Effects
{
    public class BlurEffect: HUDEffect
    {
        [SerializeField]private GameObject hitBlur;
        
        public override void Show()
        {
            StartCoroutine(ShowHitBlur());
        }
        
        private IEnumerator ShowHitBlur()
        {
            hitBlur.SetActive(true);
            yield return new WaitForSeconds(1f);
            hitBlur.SetActive(false);
        }
    }
}