using System;
using System.Collections;
using System.Collections.Generic;
using HUD;
using Mirror;
using Phone;
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
    [SerializeField] private PhoneController phone;
    public bool IsPhoneActive => phone.isPhoneActive;

    [SyncVar]
    public string steamName;

    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;
    public LayerMask Render;

    //For test only
    public bool GetHit;

    private void Start()
    {
        onDamageTaken.AddListener(PlayDamageSound);
        if (isLocalPlayer)
        {
            overlayCamera.depth = 1000;
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
            Camera.main.cullingMask = Render;
            phone.gameObject.layer = LayerMask.NameToLayer("FirstPersonVictim");
            SetLayerAllChildren(phone.transform, LayerMask.NameToLayer("FirstPersonVictim"));
        }
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


    public void GetDamage(int damage)
    {
        Health -= damage;
        onDamageTaken.Invoke();
        if (Health <= 0)
        {
            onDeath.Invoke();
            Destroy(gameObject);
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