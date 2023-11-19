using Mirror;
using Symbols.Inserter;
using UI;
using UnityEngine;

namespace Symbols
{
    public class SymbolInteract : NetworkBehaviour
    {
        private const float SymbolInserterRadius = 2f;
        private UIController uiController;
        private Camera mainCamera;
        private bool parentIsVictim;
    

        private void Start()
        {
            mainCamera = Camera.main;
            uiController = FindObjectOfType<UIController>(true);
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            var inserterButton = FindSymbolInserterButton();
            if (inserterButton is null)
            {
                uiController.TooltipSetActive(false);
                return;   
            }
            if (isLocalPlayer)
                uiController.TooltipSetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                PressButton(inserterButton);
            }
        }

        [Command]
        private void PressButton(SymbolButton button)
        {
            button.Pressed?.Invoke();
        }

        private SymbolButton FindSymbolInserterButton()
        {
            var cameraTransform = mainCamera.transform;
        
            SymbolButton button = null;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, SymbolInserterRadius))
                button = hitInfo.collider.GetComponent<SymbolButton>();
        
            return button;
        }
    }
}
