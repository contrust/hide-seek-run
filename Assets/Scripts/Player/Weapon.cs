using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : NetworkBehaviour
{
    public GameObject Flash;
    public float TimeReload;
    public int Damage;
    public AudioSource ShootingSound;

    private float lastTimeShot;
    private Camera mainCamera;
    public UnityEvent onShot;
    public UnityEvent onEnemyHit;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Time.time - lastTimeShot > TimeReload && Input.GetKey(KeyCode.Mouse0))
        {
            Transform cameraTransform = mainCamera.transform;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, 100.0f))
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hitInfo.distance, Color.yellow, 10, false);
                var victim = hitInfo.collider.GetComponent<Victim>();
                if (victim)
                {
                    victim.GetDamage(Damage);
                    onEnemyHit.Invoke();
                }
            }
            onShot.Invoke();
            lastTimeShot = Time.time;
            Flash.SetActive(true);
            PlayShootingSound();
        }
        
        if(Time.time - lastTimeShot > 0.1)
            Flash.SetActive(false);
    }

    private void PlayShootingSound()
    {
        CmdSendServerShootingSound();
    }

    [Command]
    private void CmdSendServerShootingSound()
    {
        RpcSendShootingSoundToClients();
    }

    [ClientRpc]
    private void RpcSendShootingSoundToClients()
    {
        ShootingSound.Play();
    }
}
