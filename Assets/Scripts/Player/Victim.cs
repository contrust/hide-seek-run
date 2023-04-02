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
    [SerializeField] private Material skybox;
    private MatchSettings matchSettings;
    public UnityEvent onDamageTaken;
    
    //For test only
    public bool GetHit;

    private void Start()
    {
        matchSettings = FindObjectOfType<MatchSettings>();
        if (isLocalPlayer) Init();
    }

    private void Update()
    {
        if (GetHit)
        {
            GetHit = false;
            onDamageTaken.Invoke();
        }
    }

    private void Init()
    {
        /*RenderSettings.skybox = skybox;
        RenderSettings.fogDensity = 0.025f;
        RenderSettings.fogColor = new Color(124, 177, 207, 255);*/
        HUDController.instance.ShowStaticElements();
        HUDController.instance.SetupEventHandlers();
    }
    
    

    public void GetDamage(int damage)
    {
        Health -= damage;
        onDamageTaken.Invoke();
        if (Health <= 0)
        {
            matchSettings.Victims.Remove(this);
            Destroy(gameObject);
        }
    }
}
