using Mirror;
using StarterAssets;
using UnityEngine;

namespace Phone
{
    public class PhoneController: MonoBehaviour
    {
        [SerializeField] private Transform phoneDefaultPosition;
        [SerializeField] private Transform phoneActivePosition;
        [SerializeField] private GameObject phone;
        [SerializeField] private bool isPhoneActive;
        [SerializeField] private HunterDetector hunterDetector;
        [SerializeField] private PhoneExpirationIndicator expirationIndicator;

        [SerializeField] private NetworkIdentity playerNetworkIdentity;

        [SerializeField] private StarterAssetsInputs input;

        private void Start()
        {
            input = GetComponentInParent<StarterAssetsInputs>();
            if (IsLocalPlayer())
            {
                phone.SetActive(true);
                hunterDetector.TurnOn();
            }
        }

        private void Update()
        {
            if(!IsLocalPlayer()) return;
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
            hunterDetector.TurnOn();
        }
        
        private void HidePhone()
        {
            phone.transform.position = phoneDefaultPosition.position;
            phone.transform.rotation = phoneDefaultPosition.rotation;
            isPhoneActive = false;
            hunterDetector.TurnOff();
        }

        private bool IsLocalPlayer()
        {
            return !(playerNetworkIdentity is null || !playerNetworkIdentity.isLocalPlayer);
        }
    }
}