using System;
using Mirror;
using StarterAssets;
using UnityEngine;

namespace Player
{
    public class CursorController: MonoBehaviour
    {
        private StarterAssetsInputs input;
        [SerializeField] private NetworkBehaviour player;

        private void Start()
        {
            input = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (!player.isLocalPlayer) return;
            UpdateCursor();
        }

        private void UpdateCursor()
        {
            if (input.showCursor && !Cursor.visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (!input.showCursor && Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}