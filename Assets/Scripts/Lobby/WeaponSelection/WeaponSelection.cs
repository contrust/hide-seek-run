using Assets.Scripts.Lobby.WeaponSelection;
using Network;
using System;
using TMPro;
using UnityEngine;

namespace Lobby.WeaponSelection
{
    internal class WeaponSelection : MonoBehaviour
    {
        public GameObject SelectionWindow;
        public GameObject SelectWeaponButton;
        public TextMeshProUGUI CurrentWeaponText;
        private string CurrentWeaponTemplate = "<mark=#>Текущее оружие: {weaponType}<mark>";
        private Hunter hunter;
        private WeaponType CurrentWeapon;

        public void ShowSelectionWindow()
        {
            SelectionWindow.SetActive(true);
        }

        public void HideSelectionWindow()
        {
            SelectionWindow.SetActive(false);
        }

        public void SelectRifle() => hunter.SelectWeapon(WeaponType.Rifle);
        public void SelectShotgun() => hunter.SelectWeapon(WeaponType.Shotgun);

        private void Start()
        {
            var networkRoomManager = FindObjectOfType<CustomNetworkRoomManager>();
            hunter = networkRoomManager.hunterPrefab.GetComponent<Hunter>();
            if (hunter.isServer)
            {
                InitHunterUI();
            }
        }

        private void Update()
        {
            UpdateCurrentWeaponText();
        }


        private void InitHunterUI()
        {
            SelectWeaponButton.SetActive(true);
        }

        private void UpdateCurrentWeaponText()
        {
            if(CurrentWeapon != hunter.CurrentWeapon) {
                CurrentWeapon = hunter.CurrentWeapon;
                SetCurrentWeapon(hunter.CurrentWeapon);
            }
        }

        private void SetCurrentWeapon(WeaponType weapon)
        {
            switch (weapon)
            {
                case WeaponType.Rifle: 
                    CurrentWeaponText.text = CurrentWeaponTemplate.Replace("{weaponType}", "Винтовка");
                    break;
                case WeaponType.Shotgun: 
                    CurrentWeaponText.text = CurrentWeaponTemplate.Replace("{weaponType}", "Дробовик"); 
                    break;
                default: throw new ArgumentException("Unknown weapon type");
            }
        }
    }
}