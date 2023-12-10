using System.Collections;
using Mirror;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Weapons
{
    public abstract class WeaponBase : NetworkBehaviour
    {
        protected abstract float ShootingDistance { get; set; }
        protected const float FlashTime = 0.1f;
        public GameObject Flash;
        public float TimeReload;
        public int Damage;
        public AudioSource ShootingSound;
        public UnityEvent onShot;
        public UnityEvent onEnemyHit;

        [SerializeField] protected Transform bulletPos;
        [SerializeField] protected Transform POVbulletPos;
        [SerializeField] protected TrailRenderer bulletTrail;
        [SerializeField] protected TrailRenderer POVtrail;
        [SerializeField] protected ParticleSystem shootingSystem;
        [SerializeField] protected ParticleSystem victimShooting;

        protected StarterAssetsInputs input;
        protected float lastShotTime;
        protected Camera mainCamera;

        protected float TimeFromLastShot => Time.time - lastShotTime;
        protected bool IsLoaded => TimeFromLastShot > TimeReload;
        protected bool CanShoot => IsLoaded && !Cursor.visible;

        protected void Start()
        {
            mainCamera = Camera.main;
            onShot.AddListener(PlayShootingSound);
            input = GetComponent<StarterAssetsInputs>();
        }

        protected void Update()
        {
            if (!isLocalPlayer) return;

            if (CanShoot && input.shot) Shoot();
            input.shot = false;
        }

        protected abstract void Shoot();

        protected bool TryHitWithRaycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
        {
            return Physics.Raycast(
                origin,
                direction,
                out hitInfo,
                ShootingDistance,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);
        }

        //TODO сделать интерфейс IDamageable и получать его в GetComponent вместо Victim
        protected bool TryDoDamage(RaycastHit hitInfo, Transform cameraTransform)
        {
            var victim = hitInfo.collider.GetComponent<Victim>();
            if (!victim) return false;
            victim.GetDamage(Damage,
                cameraTransform); //Почему GetDamage принимает в параметрах позицию камеры охотника???????????????
            onEnemyHit.Invoke();
            return true;
        }

        protected void DrawTrail(Vector3 position)
        {
            var trail = Instantiate(POVtrail, POVbulletPos.position, Quaternion.identity);
            trail.AddPosition(POVbulletPos.position);
            trail.transform.position = position;
        }

        protected IEnumerator ShowFlash()
        {
            Flash.SetActive(true);
            yield return new WaitForSeconds(FlashTime);
            Flash.SetActive(false);
        }

        protected void PlayShootingSound()
        {
            PlayShootingSoundCommand();
        }

        [Command]
        protected void PlayShootingSoundCommand()
        {
            RpcPlayShootingSound();
        }

        [ClientRpc]
        protected void RpcPlayShootingSound()
        {
            ShootingSound.Play();
        }

        [Command]
        protected void ShootLaserHit(Vector3 point)
        {
            RPCShootLaserHit(point);
        }

        [ClientRpc]
        protected void RPCShootLaserHit(Vector3 point)
        {
            var trail = Instantiate(bulletTrail, bulletPos.position, Quaternion.identity);
            trail.AddPosition(bulletPos.position);
            trail.transform.position = point;
        }
    }
}