using System;
using System.Collections;
using System.Collections.Generic;
using HUD;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Victim : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetHealth))] public int Health;
    public AudioSource DamageSound;
    [SerializeField] private Material skybox;
    [SerializeField] private GameObject view;
    [SerializeField] private int ignoreCameraLayer = 8;

    [SyncVar]
    public string steamName;

    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;
    
    //For test only
    public bool GetHit;

    private void Start()
    {
        onDamageTaken.AddListener(PlayDamageSound);
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