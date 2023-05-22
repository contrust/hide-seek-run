using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunEffect : MonoBehaviour
{
    [SerializeField] private Hunter hunter;
    [SerializeField] private GameObject stunEffect;

    private void Start()
    {
        hunter.onStunned.AddListener(EnableStunEffect);
    }

    private void EnableStunEffect(float stunCoolDownDuration)
    {
        StartCoroutine(StunEffectCoroutine(stunCoolDownDuration));
    }

    private IEnumerator StunEffectCoroutine(float duration)
    {
        stunEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        stunEffect.SetActive(false);
    }
}
