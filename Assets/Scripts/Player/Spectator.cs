using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class Spectator : MonoBehaviour
	{
		private StarterAssetsInputs input;
		private int currentCameraIndex = -1;
		private Vector3 target = Vector3.zero;
		private TextMeshProUGUI spectatorNickname;
		
		private List<Victim> victims => FindObjectsOfType<Victim>().ToList();
		private List<Camera> cameras => victims.Select(v => v.GetComponentInChildren<Camera>()).ToList();

		private void Start()
		{
			var mainCamera = GetComponent<Camera>();
			mainCamera.transform.SetParent(null);
			mainCamera.enabled = false;
			input = GetComponent<StarterAssetsInputs>();
			input.enabled = true;
			spectatorNickname = GameObject.FindGameObjectWithTag("SpectatorNickname").GetComponent<TextMeshProUGUI>();
			GetComponent<PlayerInput>().enabled = true;
			NextCamera();
		}

		private void Update()
		{
			if (input.nextCamera)
			{
				NextCamera();
				input.nextCamera = false;
			}
			if (input.previousCamera)
			{
				PreviousCamera();
				input.previousCamera = false;
			}
			transform.position = target;
		}

		private void NextCamera()
		{
			var _cameras = cameras;
			if (_cameras.Count == 0)
				return;
			currentCameraIndex = (int)Mathf.Repeat(currentCameraIndex + 1, _cameras.Count);
			EnableCamera(_cameras[currentCameraIndex], currentCameraIndex);
		}

		private void PreviousCamera()
		{
			var _cameras = cameras;

			if (_cameras.Count == 0)
				return;
			currentCameraIndex = (int)Mathf.Repeat(currentCameraIndex - 1, _cameras.Count);
			EnableCamera(_cameras[currentCameraIndex], currentCameraIndex);
		}

		private void EnableCamera(Camera curCamera, int index)
		{
			var nickname = victims[index].steamName;
			foreach (Camera cam in cameras) 
				cam.enabled = false;
			curCamera.enabled = true;
			target = curCamera.transform.position;
			spectatorNickname.text = nickname;
		}
	}
}
