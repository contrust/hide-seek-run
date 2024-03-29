using System.Collections;
using DefaultNamespace;
using HUD;
using Mirror;
using Phone;
using Player;
using StarterAssets;
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
    private FirstPersonController movementController;
    
    [SerializeField] private PhoneController phone;
    public bool IsPhoneActive => phone.isPhoneActive;
    public bool IsStunned { get; private set; }

    [SyncVar]
    public string steamName;

    public UnityEvent onDamageTaken;
    public UnityEvent onStartStun;
    public UnityEvent onEndStun;
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
        movementController = gameObject.GetComponent<FirstPersonController>();
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
            CmdOnDamageTaken();
        }
    }

    private void SetHealth(int oldValue, int newValue)
    {
        if (isLocalPlayer && newValue < Health)
        {
            CmdOnDamageTaken();
            if (Health <= 0)
                CmdOnDeath();
        }
        Health = newValue;
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
        CmdOnDamageTaken();
        if (Health <= 0)
        {
            var hitAngle = Vector3.Angle(hunterCamera.forward * -1, overlayCamera.transform.forward);
            CmdDie(hunterCamera.position, hitAngle);
            view.layer = LayerMask.NameToLayer("Default");
            animationHelper.TriggerDead(hitAngle);
        }
    }

    public void GetStun(float timeInSeconds)
    {
        if(IsStunned) return;
        StartCoroutine(StunCoroutine(timeInSeconds));
    }

    private IEnumerator StunCoroutine(float timeInSeconds)
    {
        onStartStun.Invoke();
        IsStunned = true;
        var tmpSpeed = movementController.MoveSpeed;
        var tmpSprintSpeed = movementController.SprintSpeed;
        movementController.MoveSpeed = 0;
        movementController.SprintSpeed = 0;
        movementController.CanJump = false;
        yield return new WaitForSeconds(timeInSeconds);
        movementController.MoveSpeed = tmpSpeed;
        movementController.SprintSpeed = tmpSprintSpeed;
        movementController.CanJump = true;
        IsStunned = false;
        onEndStun.Invoke();
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

    [Command(requiresAuthority = false)]
    private void CmdDie(Vector3 lookAt, float hitAngle)
    {
        Die(lookAt, hitAngle);
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

    [Command(requiresAuthority = false)]
    private void PlayDamageSoundCommand()
    {
        RpcPlayDamageSound();
    }

    [ClientRpc]
    private void RpcPlayDamageSound()
    {
        DamageSound.Play();
    }

    [Command(requiresAuthority = false)]
    private void CmdOnDamageTaken()
    {
        RpcOnDamageTaken();
    }
    
    [ClientRpc]
    private void RpcOnDamageTaken()
    {
        onDamageTaken.Invoke();
    }

    [Command(requiresAuthority = false)]
    private void CmdOnDeath()
    {
        RpcOnDeath();
    }

    [ClientRpc]
    private void RpcOnDeath()
    {
        onDeath.Invoke();
    }
}