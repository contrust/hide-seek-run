using System.Collections;
using System.Collections.Generic;
using HUD.Effects;
using UnityEngine;
using UnityEngine.Serialization;

public class SlapEffect : HUDEffect
{
    [SerializeField]private GameObject slap;
        
    public override void Show()
    {
        StartCoroutine(ShowSlapEffect());
    }
        
    private IEnumerator ShowSlapEffect()
    {
        slap.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        slap.SetActive(false);
    }
}
