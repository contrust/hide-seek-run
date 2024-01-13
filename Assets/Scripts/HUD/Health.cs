using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float fromValue = 0.26f;
    [SerializeField] private float toValue = 0.71f;

    private Victim victim;
    private Image playerImage => GetComponent<Image>();

    public void SetVictim(Victim newVictim, Sprite playerSprite)
    {
        gameObject.SetActive(true);
        if (victim)
        {
            victim.onDamageTaken.RemoveListener(SetHealth);
            victim.onDeath.RemoveListener(Dead);
        }
        victim = newVictim;
        victim.onDamageTaken.AddListener(SetHealth);
        victim.onDeath.AddListener(Dead);
        playerImage.sprite = playerSprite;
    }

    private void SetHealth() => 
        healthBar.fillAmount = Mathf.Lerp(fromValue, toValue, (float)victim.Health / victim.MaxHealth);

    private void Dead() => playerImage.color = Color.gray;
}
