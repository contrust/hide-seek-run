using System;
using TMPro;
using UnityEngine;

namespace Interactions
{
    public class VictimInteraction: MonoBehaviour
    {
        public Camera mainCam;
        public float interactionDistance = 2f;

        public GameObject interactionUI;
        public TextMeshProUGUI interactionText;

        private void Update()
        {
            InteractionRay();
        }

        private void Awake()
        {
            interactionUI = GameObject.Find("InteractionUI");
            interactionText = interactionUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void InteractionRay()
        {
            var ray = mainCam.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hit;

            bool hitSomething = false;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    hitSomething = true;
                    interactionText.text = interactable.GetDescription();
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        interactable.Interact();
                    }
                }
            }

            interactionUI.SetActive(hitSomething);
        }
    }
}