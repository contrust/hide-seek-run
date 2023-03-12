using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Victim : NetworkBehaviour
{
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
}
