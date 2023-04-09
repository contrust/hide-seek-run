using HUD.Effects;
using Mirror;
using UnityEngine;

namespace HUD
{
    public class HUDController : MonoBehaviour
    { 
        [SerializeField] private HUDEffect blurEffect;
        [SerializeField] private HUDEffect hitMarkerEffect;
        [SerializeField] private HUDEffect reloadEffect;
        [SerializeField] private HUDEffect symbolInsertedEffect;
        
        [SerializeField]private GameObject staticElements;
        
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
                (reloadEffect as ReloadEffect).reloadTime = weapon.TimeReload; //TODO: нормально получать время перезарядки
                weapon.onEnemyHit.AddListener(instance.OnEnemyHitHandler);
                weapon.onShot.AddListener(instance.OnShotHandler);
                SymbolManager.onSymbolInserted.AddListener(instance.OnSymbolInsertedEffect);
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
        
        public void ShowEffect(HUDEffect effect)
        {
            effect.Show();
        }

        public void OnShotHandler()
        {
            ShowEffect(reloadEffect);
        }
        
        public void OnEnemyHitHandler()
        {
            ShowEffect(hitMarkerEffect);
        }

        public void OnDamageTakenHandler()
        {
            ShowEffect(blurEffect);
        }

        public void OnSymbolInsertedEffect()
        {
            ShowEffect(symbolInsertedEffect);
        }
    }
}
