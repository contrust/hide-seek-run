using HUD.Effects;
using Mirror;
using Player;
using Player.Weapons;
using UnityEngine;

namespace HUD
{
    public class HUDController : MonoBehaviour
    { 
        [SerializeField] private HUDEffect blurEffect;
        [SerializeField] private HUDEffect hitMarkerEffect;
        [SerializeField] private HUDEffect reloadEffect;
        [SerializeField] private HUDEffect symbolInsertedEffect;
        [SerializeField] private HUDEffect slapEffect;
        [SerializeField] private HUDEffect camNumbers;
        [SerializeField] private GameObject staticElements;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private HealthManager healthManager;
        
        public static HUDController instance;

        public void Start()
        {
            instance = this;
        }

        public void SetupHUD()
        {
            var weapon = NetworkClient.localPlayer.GetComponent<Weapon>();
            var shotgun = NetworkClient.localPlayer.GetComponent<Shotgun>(); //TODO: Сделать интерфейс IWeapon и получать его
            var fourCams = NetworkClient.localPlayer.GetComponent<FourCamerasView>();
            var isHunter = weapon != null && fourCams;
            if (isHunter)
            {
                (reloadEffect as ReloadEffect).reloadTime = weapon.TimeReload; //TODO: нормально получать время перезарядки
                weapon.onEnemyHit.AddListener(instance.OnEnemyHitHandler);
                shotgun.onEnemyHit.AddListener(instance.OnEnemyHitHandler); 
                //weapon.onShot.AddListener(instance.OnShotHandler);
                SymbolManager.OnSymbolInserted.AddListener(instance.OnSymbolInsertedEffect);
                fourCams.onFourCamModeChange.AddListener(instance.OnFourCamerasHandler);
            }
            else
            {
                var victim = NetworkClient.localPlayer.GetComponent<Victim>();
                victim.onDamageTaken.AddListener(instance.OnDamageTakenHandler);
                var slap = NetworkClient.localPlayer.GetComponent<Slap>();
                slap.onSlap.AddListener(instance.OnSlapHandler);
                healthManager.gameObject.SetActive(true);
                healthManager.Setup();
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

        public void OnSymbolInsertedEffect(bool _)
        {
            ShowEffect(symbolInsertedEffect);
        }

        public void OnSlapHandler()
        {
            ShowEffect(slapEffect);
        }

        public void OnFourCamerasHandler()
        {
            ShowEffect(camNumbers);
        }
    }
}
