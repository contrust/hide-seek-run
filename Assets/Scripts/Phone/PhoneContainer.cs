using System;
using Mirror;
using StarterAssets;
using UnityEngine;
using UnityEngine.XR;

namespace Phone
{
    public class PhoneContainer: MonoBehaviour
    {
        [SerializeField] private Transform phoneDefaultPosition;
        [SerializeField] private Transform phoneActivePosition;
        [SerializeField] private GameObject phone;
        [SerializeField] private bool isPhoneActive;

        [SerializeField] private NetworkIdentity playerNetworkIdentity;

        [SerializeField] private StarterAssetsInputs input;

        private void Start()
        {
            input = GetComponentInParent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if(playerNetworkIdentity is null || !playerNetworkIdentity.isLocalPlayer) return;
            HandleInput();
        }

        private void HandleInput()
        {
            if (input.showPhone)
            {
                if (isPhoneActive)
                {
                    HidePhone();
                }
                else
                {
                    ShowPhone();
                }
            }
            input.showPhone = false;
        }

        private void ShowPhone()
        {
            phone.transform.position = phoneActivePosition.position;
            phone.transform.rotation = phoneActivePosition.rotation;
            isPhoneActive = true;
        }
        
        private void HidePhone()
        {
            phone.transform.position = phoneDefaultPosition.position;
            phone.transform.rotation = phoneDefaultPosition.rotation;
            isPhoneActive = false;
        }
    }
}