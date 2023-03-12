using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Victim : NetworkBehaviour
{
    [SyncVar] public int Health;
    [SerializeField] private Material skybox;

    private void Start()
    {   
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
            Destroy(gameObject);
    }
}
