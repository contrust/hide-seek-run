using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Victim : NetworkBehaviour
{
    [SyncVar] public int Health;
    public AudioSource DamageSound;
    [SerializeField] private Material skybox;
    private MatchSettings matchSettings;

    private void Start()
    {
        matchSettings = FindObjectOfType<MatchSettings>();
        // if (isLocalPlayer) Init();
    }

    private void Init()
    {
        RenderSettings.skybox = skybox;
        RenderSettings.fogDensity = 0.025f;
        RenderSettings.fogColor = new Color(124, 177, 207, 255);
    }

    public void GetDamage(int damage)
    {
        PlayDamageSound();
        Health -= damage;
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
