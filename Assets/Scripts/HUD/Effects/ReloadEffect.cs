using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HUD.Effects
{
    public class ReloadEffect: HUDEffect
    {
        [SerializeField]public float reloadTime;
        [SerializeField] private float remainingTime;
        [SerializeField]private Image reloadCircle;
        
        public override void Show()
        {
            reloadCircle.gameObject.SetActive(true);
            remainingTime = reloadTime;
        }

        public void Update()
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                reloadCircle.fillAmount = remainingTime / reloadTime;
            }
            else
            {
                reloadCircle.gameObject.SetActive(false);
            }
        }
    }
}