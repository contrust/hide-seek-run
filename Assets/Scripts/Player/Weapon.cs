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
    private float lastTimeShot;
    private Camera mainCamera;
    public UnityEvent onShot;
    public UnityEvent onEnemyHit;

    [SerializeField] private Transform bulletPos;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private ParticleSystem shootingSystem;
    private float bulletSpeed = 50;


    private void Start()
    {
        mainCamera = Camera.main;
        onShot.AddListener(PlayShootingSound);
        input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Time.time - lastTimeShot > TimeReload && input.shot)
        {
            Transform cameraTransform = mainCamera.transform;
            shootingSystem.Play();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, 100.0f,
                    Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hitInfo.distance, Color.yellow, 10,
                    false);
                var victim = hitInfo.collider.GetComponent<Victim>();
                if (victim)
                {
                    victim.GetDamage(Damage, cameraTransform);
                    onEnemyHit.Invoke();
                }
                ShootLaserHit(hitInfo.point);
            }
            else 
                ShootLaserNotHit(mainCamera.transform.position + mainCamera.transform.forward * 100f);
            onShot.Invoke();
            lastTimeShot = Time.time;
            Flash.SetActive(true);
        }

        input.shot = false;
        
        if(Time.time - lastTimeShot > 0.1)
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
    
    [Command]
    private void ShootLaserNotHit(Vector3 point)
    {
        RPCShootLaserNotHit(point);
    }

    [ClientRpc]
    private void RPCShootLaserNotHit(Vector3 point)
    {
        var trail = Instantiate(bulletTrail, bulletPos.position, Quaternion.identity);
        trail.AddPosition(bulletPos.position);
        trail.transform.position = point;
    }
}