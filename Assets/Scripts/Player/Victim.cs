using DefaultNamespace;
using HUD;
using Mirror;
using Phone;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Victim : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetHealth))] public int Health;
    public AudioSource DamageSound;

    [SerializeField] private Material skybox;
    [SerializeField] private GameObject view;

    [SerializeField] private int ignoreCameraLayer = 8;
    [SerializeField] private Camera overlayCamera;
    private const float ClippingPlaneDistance = 0.15f;
    private const int OverlayCameraDepth = 1000;
    private PlayerCamera playerCamera;
    
    [SerializeField] private PhoneController phone;
    public bool IsPhoneActive => phone.isPhoneActive;

    [SyncVar]
    public string steamName;

    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;
    public LayerMask Render;
    private AnimationHelper animationHelper;

    //For test only
    public bool GetHit;

    private void Start()
    {
        animationHelper = GetComponent<AnimationHelper>();
        onDamageTaken.AddListener(PlayDamageSound);
        playerCamera = GetComponent<PlayerCamera>();
        if (isLocalPlayer)
        {
            overlayCamera.depth = OverlayCameraDepth;
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
            Camera.main.cullingMask = Render;
            Camera.main.nearClipPlane = ClippingPlaneDistance;
            phone.gameObject.layer = LayerMask.NameToLayer("FirstPersonVictim");
            SetLayerAllChildren(phone.transform, LayerMask.NameToLayer("FirstPersonVictim"));
        }
        onDeath.AddListener(AliveVictimsCounter.instance.OnVictimDeathHandler);
    }
    
    private void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    private void Update()
    {
        if (GetHit)
        {
            GetHit = false;
            onDamageTaken.Invoke();
        }
    }

    private void SetHealth(int oldValue, int newValue)
    {
        Health = newValue;
        if (isLocalPlayer) 
            onDamageTaken.Invoke();
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            view.layer = ignoreCameraLayer;
            HUDController.instance.ShowStaticElements();
            HUDController.instance.SetupEventHandlers();
        }
    }


    public void GetDamage(int damage, Transform hunterCamera)
    {
        Health -= damage;
        onDamageTaken.Invoke();
        if (Health <= 0)
        {
            var hitAngle = Vector3.Angle(hunterCamera.forward * -1, overlayCamera.transform.forward);
            Die(hunterCamera.position, hitAngle);
            view.layer = LayerMask.NameToLayer("Default");
            animationHelper.TriggerDead(hitAngle);
            onDeath.Invoke();
        }
    }

    [ClientRpc]
    private void Die(Vector3 lookAt, float hitAngle)
    {
        if (isLocalPlayer)
        {
            Camera cam = Camera.main;
            cam.GetUniversalAdditionalCameraData().cameraStack.Remove(overlayCamera);
            playerCamera.SetControl(false);
            transform.LookAt(lookAt, Vector3.up);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + (hitAngle < 90 ? 0 : 180), 0);
            view.layer = LayerMask.NameToLayer("Default");
            animationHelper.TriggerDead(hitAngle, cam.GetComponent<Spectator>());
        }
    }

    private void PlayDamageSound()
    {
        PlayDamageSoundCommand();
    }

    [Command]
    private void PlayDamageSoundCommand()
    {
        RpcPlayDamageSound();
    }

    [ClientRpc]
    private void RpcPlayDamageSound()
    {
        DamageSound.Play();
    }
}