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
        [SerializeField] public static bool forced;

        public static void ForcedShowCursor()
        {
            forced = true;
            ShowCursor();
        }
        
        public static void ForcedHideCursor()
        {
            forced = false;
            HideCursor();
        }
        
        public static void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void HideCursor()
        {
            if(forced) return;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

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
                ShowCursor();
            }

            if (!input.showCursor && Cursor.visible)
            {
                HideCursor();
            }
        }
        
        private void OnDisable()
        {
            if(!player.isLocalPlayer) return;
            forced = false;
            ShowCursor();
        }
    }
}