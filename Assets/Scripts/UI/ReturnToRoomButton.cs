﻿using Mirror;
using Player;
using UnityEngine;

namespace UI
{
    public class ReturnToRoomButton: MonoBehaviour
    {
        private UIController uiController;
        
        public void OnClick()
        {
            var networkManager = FindObjectOfType<NetworkRoomManager>();
            networkManager.ServerChangeScene(networkManager.RoomScene);
            if (uiController is null)
            {
                uiController = UIController.instance;
            }
            uiController.HideUIScreen(uiController.activeScreen);
            uiController.TooltipSetActive(false);
        }
    }
}