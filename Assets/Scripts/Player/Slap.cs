using System.Collections;
using System.Collections.Generic;
using Mirror;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Slap : NetworkBehaviour
{
    [SerializeField] private float slapCooldown;
    private float slapReload;
    [SerializeField] private float slapRadius;
    private Camera mainCamera;
    private bool parentIsVictim;
    public UnityEvent onSlap;
    private StarterAssetsInputs input;
    private Victim player;
    

    private void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        mainCamera = Camera.main;
        player = GetComponent<Victim>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        if (slapReload > 0)
            slapReload -= Time.deltaTime;
        if (input.slap && slapReload <= 0 && !player.isPhoneActive)
        {
            onSlap.Invoke();
            slapReload = slapCooldown;
            var hunter = TryFindHunter();
            if (hunter is not null)
                SlapHunter(hunter);
        }
        input.slap = false;
    }

    [Command]
    private void SlapHunter(Hunter hunter)
    {
        hunter.Slapped();
    }

    private Hunter TryFindHunter()
    {
        var cameraTransform = mainCamera.transform;
        Hunter hunter = null;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, slapRadius))
        {
            if (hitInfo.transform.parent != null)
                hunter = hitInfo.transform.parent.GetComponent<Hunter>();
        }


        return hunter;
    }
}
