using HUD.Effects;
using Mirror;
using UnityEngine;

namespace HUD
{
    public class HUDController : MonoBehaviour
    { 
        [SerializeReference]private HUDEffect blurEffect;
        [SerializeReference]private HUDEffect hitMarkerEffect;
        [SerializeField] private GameObject staticElements;
        
        public static HUDController instance;

        public void Start()
        {
            instance = this;
        }

        public void SetupEventHandlers()
        {
            var weapon = NetworkClient.localPlayer.GetComponent<Weapon>();
            if (weapon != null)
            {
                weapon.onEnemyHit.AddListener(instance.OnEnemyHitHandler);
            }
            else
            {
                var victim = NetworkClient.localPlayer.GetComponent<Victim>();
                victim.onDamageTaken.AddListener(instance.OnDamageTakenHandler);
            }
        }

        public void HideStaticElements()
        {
            staticElements.SetActive(false);
        }

        public void ShowStaticElements()
        {
            staticElements.SetActive(true);
        }
        
        public void OnEnemyHitHandler()
        {
            ShowEffect(hitMarkerEffect);
        }

        public void OnDamageTakenHandler()
        {
            ShowEffect(blurEffect);
        }

        public void ShowEffect(HUDEffect effect)
        {
            effect.Show();
        }
    }
}
