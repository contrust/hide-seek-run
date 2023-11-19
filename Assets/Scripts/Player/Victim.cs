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
    [SyncVar(hook = nameof(SetColor))] public ColorPlayerEnum color;
    [SerializeField] private Material[] hatTextures;
    [SerializeField] private GameObject hat;
    public int MaxHealth = 100;
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
            InitCamera();
            SetupLayers();
        }
        onDeath.AddListener(AliveVictimsCounter.instance.OnVictimDeathHandler);
    }
    
    private void Update()
    {
        if (GetHit)
        {
            GetHit = false;
            GetDamage(1, null);
            onDamageTaken.Invoke();
        }
    }

    private void SetHealth(int oldValue, int newValue)
    {
        Health = newValue;
        if (isLocalPlayer)
        {
            onDamageTaken.Invoke();
        }
    }

    private void SetColor(ColorPlayerEnum _, ColorPlayerEnum newValue)
    {
        color = newValue;
        var newMaterials = hat.GetComponent<SkinnedMeshRenderer>().materials;
        newMaterials[3] = hatTextures[(int)newValue];
        hat.GetComponent<SkinnedMeshRenderer>().materials = newMaterials;
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            view.layer = ignoreCameraLayer;
            HUDController.instance.ShowStaticElements();
            HUDController.instance.SetupHUD();
        }
    }


    public void GetDamage(int damage, Transform hunterCamera)
    {
        if(Health <=0)
            return;
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

    private void InitCamera()
    {
        overlayCamera.depth = OverlayCameraDepth;
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
        Camera.main.cullingMask = Render;
        Camera.main.nearClipPlane = ClippingPlaneDistance;
    }

    private void SetupLayers()
    {
        phone.gameObject.layer = LayerMask.NameToLayer("FirstPersonVictim");
        SetLayerAllChildren(phone.transform, LayerMask.NameToLayer("FirstPersonVictim"));
    }

    private void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
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