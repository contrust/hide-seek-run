using System;
using Mirror;
using UnityEngine;

namespace Phone
{
    public class PhoneContainer: MonoBehaviour
    {
        [SerializeField] private Transform phoneDefaultPosition;
        [SerializeField] private Transform phoneActivePosition;
        [SerializeField] private GameObject phone;
        [SerializeField] private KeyCode showPhoneKey = KeyCode.Q;
        [SerializeField] private bool isPhoneActive;

        [SerializeField] private NetworkIdentity playerNetworkIdentity;

        private void Update()
        {
            if(playerNetworkIdentity is null || !playerNetworkIdentity.isLocalPlayer) return;
            if (Input.GetKeyDown(showPhoneKey))
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