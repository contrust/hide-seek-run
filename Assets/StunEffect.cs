using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class StunEffect : NetworkBehaviour
{
    [SerializeField] private Hunter hunter;
    [SerializeField] private ParticleSystem stunEffect;

    private void Start()
    {
        hunter.onStunned.AddListener(EnableStunEffect);
    }

    private void EnableStunEffect(float stunCoolDownDuration)
    {
        EnableStunEffectCommand(stunCoolDownDuration);
    }

    [Command]
    private void EnableStunEffectCommand(float stunCoolDownDuration)
    {
        RpcEnableStunEffect(stunCoolDownDuration);
    }

    [ClientRpc]
    private void RpcEnableStunEffect(float stunCoolDownDuration)
    {
        stunEffect.Stop();
        var effectMain = stunEffect.main;
        effectMain.duration = stunCoolDownDuration/2;
        effectMain.startLifetime = stunCoolDownDuration / 2;
        stunEffect.Play();
    }
}
