using System.Collections;
using Player.Weapons;
using UnityEngine;

public class Recharge : MonoBehaviour
{
    private float reload;
    private int rechargeBars = 4;
    [SerializeField] private Transform recharger;
    void Start()
    {
        var weapon = GetComponent<Weapon>();
        reload = weapon.TimeReload;
        weapon.onShot.AddListener(StartRecharging);
    }

    private void StartRecharging()
    {
        foreach(Transform bar in recharger)
            bar.gameObject.SetActive(false);
        StartCoroutine(Recharging());

    }

    private IEnumerator Recharging()
    {
        var singleBarRecharge = reload / rechargeBars;
        foreach (Transform bar in recharger)
        {
            yield return new WaitForSeconds(singleBarRecharge);
            bar.gameObject.SetActive(true);
        }
    }
}
