using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class StunEffect : NetworkBehaviour
{
    [SerializeField] private Hunter hunter;
    [SerializeField] private GameObject stunEffect;

    private void Start()
    {
        hunter.onStunned.AddListener(EnableStunEffect);
    }

    private void EnableStunEffect(float stunCoolDownDuration)
    {
        EnableStunEffectCommand(stunCoolDownDuration);
    }

    private IEnumerator StunEffectCoroutine(float duration)
    {
        stunEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        stunEffect.SetActive(false);
    }
    
    [Command]
    private void EnableStunEffectCommand(float stunCoolDownDuration)
    {
        RpcEnableStunEffect(stunCoolDownDuration);
    }

    [ClientRpc]
    private void RpcEnableStunEffect(float stunCoolDownDuration)
    {
        StartCoroutine(StunEffectCoroutine(stunCoolDownDuration));
    }
}
