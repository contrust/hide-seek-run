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
        var effectMain = stunEffect.main;
        effectMain.duration = stunCoolDownDuration;

    }

    // private IEnumerator StunEffectCoroutine(float duration)
    // {
    //     stunEffect.Play();
    //     stunEffect.SetActive(true);
    //     yield return new WaitForSeconds(duration);
    //     stunEffect.SetActive(false);
    // }
    
    [Command]
    private void EnableStunEffectCommand(float stunCoolDownDuration)
    {
        RpcEnableStunEffect(stunCoolDownDuration);
    }

    [ClientRpc]
    private void RpcEnableStunEffect(float stunCoolDownDuration)
    {
        // StartCoroutine(StunEffectCoroutine(stunCoolDownDuration));
        
        stunEffect.Play();
    }
}
