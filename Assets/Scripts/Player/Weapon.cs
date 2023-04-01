using System.Collections;
using System.Collections.Generic;
using Mirror;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Weapon : NetworkBehaviour
{
    public GameObject Flash;
    public float TimeReload;
    public int Damage;

    private float lastTimeShot;
    private Camera mainCamera;
    public UnityEvent onEnemyHit;


    private void Start()
    {
        mainCamera = Camera.main;
        onEnemyHit.AddListener(UIController.instance.OnEnemyHitHandler);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Time.time - lastTimeShot > TimeReload && Input.GetKey(KeyCode.Mouse0))
        {
            Transform cameraTransform = mainCamera.transform;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, 100.0f))
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hitInfo.distance, Color.yellow, 10, false);
                var victim = hitInfo.collider.GetComponent<Victim>();
                if (victim)
                {
                    victim.GetDamage(Damage);
                    onEnemyHit.Invoke();
                }
            }
            lastTimeShot = Time.time;
            Flash.SetActive(true);
        }
        
        if(Time.time - lastTimeShot > 0.1)
            Flash.SetActive(false);
    }
}
