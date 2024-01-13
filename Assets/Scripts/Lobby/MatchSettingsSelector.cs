using Assets.Scripts.Lobby.WeaponSelection;
using Network;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Lobby
{
	public class MatchSettingsSelector : MonoBehaviour
	{
		[SerializeField] private SceneAsset[] maps;
		[SerializeField] private TMP_Dropdown weaponSelector;
		[SerializeField] private TMP_Dropdown mapSelector;
		[SerializeField] private GameObject[] enableObjectsForHost;
		
		private Hunter hunter;
		private CustomNetworkRoomManager networkRoomManager;

		public void SelectWeapon()
		{
			hunter.SelectWeapon((WeaponType)weaponSelector.value);
		}

		public void SelectMap() => networkRoomManager.GameplayScene = maps[mapSelector.value].name;

		private void Start()
		{
			networkRoomManager = FindObjectOfType<CustomNetworkRoomManager>();
			hunter = networkRoomManager.hunterPrefab.GetComponent<Hunter>();
			Debug.Log(hunter);
			//if (hunter.isServer)
			{
				InitHostUI();
			}
		}

		private void InitHostUI()
		{
			foreach (GameObject obj in enableObjectsForHost)
			{
				obj.SetActive(true);
			}
		}
	}
}
