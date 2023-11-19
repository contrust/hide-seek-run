using Mirror;
using StarterAssets;
using Symbols;
using UnityEngine;
using UnityEngine.Events;

namespace Phone
{
    public class PhoneController: MonoBehaviour
    {
        [SerializeField] private Transform phoneDefaultPosition;
        [SerializeField] private Transform phoneActivePosition;
        [SerializeField] private GameObject phone;
        [SerializeField] private GameObject thirdPersonPhoneView;
        public bool isPhoneActive { get; private set; }
        [SerializeField] private HunterDetector hunterDetector;

        [SerializeField] private NetworkIdentity playerNetworkIdentity;

        [SerializeField] private StarterAssetsInputs input;
        [SerializeField] private SymbolExpirationIndicator symbolExpirationIndicator;

        public UnityEvent onShowPhone = new();
        public UnityEvent onHidePhone = new();

        private void Start()
        {
            input = GetComponentInParent<StarterAssetsInputs>();
            if (IsLocalPlayer())
            {
                phone.SetActive(true);
                thirdPersonPhoneView.SetActive(false);
            }
        }

        public void Init()
        {
            onShowPhone.AddListener(PutPhoneInActivePosition);
            onShowPhone.AddListener(hunterDetector.TurnOn);
            onShowPhone.AddListener(ActivatePhone);
            onHidePhone.AddListener(PutPhoneInDefaultPosition);
            onHidePhone.AddListener(hunterDetector.TurnOff);
            onHidePhone.AddListener(DisablePhone);
            StartCoroutine(symbolExpirationIndicator.ChangeExpirationSignalColors());
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
                    onHidePhone.Invoke();
                }
                else
                {
                    onShowPhone.Invoke();
                }
            }
            input.showPhone = false;
        }

        private void PutPhoneInActivePosition()
        {
            phone.transform.position = phoneActivePosition.position;
            phone.transform.rotation = phoneActivePosition.rotation;
        }

        private void ActivatePhone()
        {
            isPhoneActive = true;
        }
        
        private void PutPhoneInDefaultPosition()
        {
            phone.transform.position = phoneDefaultPosition.position;
            phone.transform.rotation = phoneDefaultPosition.rotation;
        }

        private void DisablePhone()
        {
            isPhoneActive = false;
        }

        private bool IsLocalPlayer()
        {
            return !(playerNetworkIdentity is null || !playerNetworkIdentity.isLocalPlayer);
        }
    }
}