using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]private GameObject hitMarker;
        public static UIController instance;

        public void Start()
        {
            instance = this;
        }
        
        public void OnEnemyHitHandler()
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
