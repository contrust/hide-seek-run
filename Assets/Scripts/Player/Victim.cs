using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Victim : NetworkBehaviour
{
    [SyncVar] public int Health;
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
        Health -= damage;
        if (Health <= 0)
        {
            matchSettings.Victims.Remove(this);
            Destroy(gameObject);
        }
    }
}
