using System.Collections;
using System.Collections.Generic;
using HUD.Effects;
using UnityEngine;

public class CamNumbers : HUDEffect
{
    [SerializeField]private GameObject camNumbers;
        
    public override void Show()
    {
        camNumbers.SetActive(!camNumbers.activeSelf);
    }
}
