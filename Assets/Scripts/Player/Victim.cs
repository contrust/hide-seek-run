using System;
using System.Collections;
using System.Collections.Generic;
using HUD;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Victim : NetworkBehaviour
{
    [SyncVar] public int Health;
    public AudioSource DamageSound;
    [SerializeField] private Material skybox;
    private MatchSettings matchSettings;
    [SerializeField] private GameObject view;
    [SerializeField] private int ignoreCameraLayer = 8;

    public UnityEvent onDamageTaken;
    
    //For test only
    public bool GetHit;

    private void Start()
    {
        matchSettings = FindObjectOfType<MatchSettings>();
    }

    private void Update()
    {
        if (GetHit)
        {
            GetHit = false;
            onDamageTaken.Invoke();
        }
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
        PlayDamageSound();
        Health -= damage;
        onDamageTaken.Invoke();
        if (Health <= 0)
        {
            matchSettings.Victims.Remove(this);
            Destroy(gameObject);
        }
    }

    private void PlayDamageSound()
    {
        CmdSendServerDamageSound();
    }
    
    [Command]
    private void CmdSendServerDamageSound()
    {
        RpcSendDamageSoundToClients();
    }

    [ClientRpc]
    private void RpcSendDamageSoundToClients()
    {
        DamageSound.Play();
    }
}
