using HUD.Effects;
using Mirror;
using Player.HunterAbilities.Trap;
using Player.Weapons;
using UnityEngine;

namespace HUD
{
    public class HUDController : MonoBehaviour
    { 
        [SerializeField] private HUDEffect hitBlurEffect;
        [SerializeField] private HUDEffect stunBlurEffect;
        [SerializeField] private GameObject stunnedText;
        [SerializeField] private HUDEffect hitMarkerEffect;
        [SerializeField] private HUDEffect reloadEffect;
        [SerializeField] private HUDEffect symbolInsertedEffect;
        [SerializeField] private HUDEffect slapEffect;
        [SerializeField] private HUDEffect camNumbers;
        [SerializeField] private GameObject staticElements;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private HealthManager healthManager;
        [SerializeField] private TrapInfo trapInfo;

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
                var trapSpawner = NetworkClient.localPlayer.GetComponent<TrapSpawner>();
                trapSpawner.trapReady.AddListener(OnTrapReadyHandler);
                trapSpawner.trapSpawned.AddListener(OnTrapSpawnedHandler);
                trapInfo.gameObject.SetActive(true);
            }
            else
            {
                var victim = NetworkClient.localPlayer.GetComponent<Victim>();
                victim.onDamageTaken.AddListener(instance.OnDamageTakenHandler);
                victim.onStartStun.AddListener(instance.OnStartStunHandler);
                victim.onEndStun.AddListener(instance.OnEndStunHandler);
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
            ShowEffect(hitBlurEffect);
        }

        public void OnStartStunHandler()
        {
            stunnedText.SetActive(true);
            ShowEffect(stunBlurEffect);
        }

        public void OnEndStunHandler()
        {
            stunnedText.SetActive(false);
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

        public void OnTrapReadyHandler()
        {
            trapInfo.ShowTrapReady();
        }

        public void OnTrapSpawnedHandler()
        {
            trapInfo.ShowTrapReload();
        }
    }
}
