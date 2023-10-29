using System.Collections;
using Mirror;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : NetworkBehaviour
{
    public GameObject Flash;
    public float TimeReload;
    public int Damage;
    public AudioSource ShootingSound;

    private StarterAssetsInputs input;
    private float lastShotTime;
    private Camera mainCamera;
    public UnityEvent onShot;
    public UnityEvent onEnemyHit;

    [SerializeField] private Transform bulletPos;
    [SerializeField] private Transform POVbulletPos;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private TrailRenderer POVtrail;
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private ParticleSystem victimShooting;

    private const float ShootingDistance = 100;
    private const float FlashTime = 0.1f;

    private float TimeFromLastShot => Time.time - lastShotTime;
    private bool IsLoaded => TimeFromLastShot > TimeReload;
    private bool CanShoot => IsLoaded && !Cursor.visible;

    private void Start()
    {
        mainCamera = Camera.main;
        onShot.AddListener(PlayShootingSound);
        input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (CanShoot && input.shot)
        {
            Shoot();
            input.shot = false;
        }
    }

    private void Shoot()
    {
        var cameraTransform = mainCamera.transform;
        shootingSystem.Emit(1);
        victimShooting.Emit(1);

        var isHitSomethingFromCamera = TryHitWithRaycast(cameraTransform.position, cameraTransform.forward, out var cameraHitInfo);
        
        var hitPoint = isHitSomethingFromCamera
            ? cameraHitInfo.point
            : mainCamera.transform.position + mainCamera.transform.forward * ShootingDistance;
        
        if (isHitSomethingFromCamera)
        {
            Debug.DrawRay(
                cameraTransform.position,
                cameraTransform.forward * cameraHitInfo.distance,
                Color.yellow,
                10,
                false);

            DoDamage(cameraHitInfo, cameraTransform);
        }

        var isHitSomethingFromBulletPos = TryHitWithRaycast(bulletPos.position, hitPoint - bulletPos.position, out var bulletHitInfo);

        if (isHitSomethingFromBulletPos)
        {
            DoDamage(bulletHitInfo, cameraTransform);
        }
        
        DrawTrail(hitPoint);
        ShootLaserHit(hitPoint);

        onShot.Invoke();
        lastShotTime = Time.time;
        StartCoroutine(ShowFlash());
    }

    private bool TryHitWithRaycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
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
    private void DoDamage(RaycastHit hitInfo, Transform cameraTransform)
    {
        var victim = hitInfo.collider.GetComponent<Victim>();
        if (!victim) return;
        victim.GetDamage(Damage, cameraTransform); //Почему GetDamage принимает в параметрах позицию камеры охотника???????????????
        onEnemyHit.Invoke();
    }

    private void DrawTrail(Vector3 position)
    {
        var trail = Instantiate(POVtrail, POVbulletPos.position, Quaternion.identity);
        trail.AddPosition(POVbulletPos.position);
        trail.transform.position = position;
    }

    private IEnumerator ShowFlash()
    {
        Flash.SetActive(true);
        yield return new WaitForSeconds(FlashTime);
        Flash.SetActive(false);
    }

    private void PlayShootingSound()
    {
        PlayShootingSoundCommand();
    }

    [Command]
    private void PlayShootingSoundCommand()
    {
        RpcPlayShootingSound();
    }

    [ClientRpc]
    private void RpcPlayShootingSound()
    {
        ShootingSound.Play();
    }
    
    [Command]
    private void ShootLaserHit(Vector3 point)
    {
        RPCShootLaserHit(point);
    }

    [ClientRpc]
    private void RPCShootLaserHit(Vector3 point)
    {
        var trail = Instantiate(bulletTrail, bulletPos.position, Quaternion.identity);
        trail.AddPosition(bulletPos.position);
        trail.transform.position = point;
    }
}