using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Victim : NetworkBehaviour
{
    [SyncVar] public int Health;
    [SerializeField] private Material skybox;
    private MatchSettings matchSettings;
    [SerializeField] private GameObject view;
    [SerializeField] private int ignoreCameraLayer = 8;

    private void Start()
    {
        matchSettings = FindObjectOfType<MatchSettings>();
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer) 
            view.layer = ignoreCameraLayer;
    }
    

    public void GetDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            matchSettings.Victims.Remove(this);
            Destroy(gameObject);
        }
    }
}
