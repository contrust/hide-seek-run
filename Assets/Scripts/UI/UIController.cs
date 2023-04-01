using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]private GameObject hitMarker;
        [SerializeField] private GameObject hitBlur;
        public static UIController instance;

        public void Start()
        {
            instance = this;
        }
        
        public void OnEnemyHitHandler()
        {
            StartCoroutine(ShowHitMarker());
        }

        public void OnDamageTakenHandler()
        {
            StartCoroutine(ShowHitBlur());
        }

        private IEnumerator ShowHitMarker()
        {
            hitMarker.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            hitMarker.SetActive(false);
        }

        private IEnumerator ShowHitBlur()
        {
            hitBlur.SetActive(true);
            yield return new WaitForSeconds(1f);
            hitBlur.SetActive(false);
        }
    }
}
