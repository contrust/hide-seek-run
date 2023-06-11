using System;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class Spectator : MonoBehaviour
	{
		private StarterAssetsInputs input;
		private List<Camera> Cameras => FindObjectsOfType<Victim>().Select(v => v.GetComponentInChildren<Camera>()).ToList();
		private int currentCameraIndex = -1;
		private Vector3 target = Vector3.zero;

		private void Start()
		{
			var mainCamera = GetComponent<Camera>();
			mainCamera.transform.SetParent(null);
			mainCamera.enabled = false;
			input = GetComponent<StarterAssetsInputs>();
			input.enabled = true;
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
			if (Cameras.Count == 0)
				return;
			currentCameraIndex = (int)Mathf.Repeat(currentCameraIndex + 1, Cameras.Count);
			EnableCamera(Cameras[currentCameraIndex]);
		}

		private void PreviousCamera()
		{
			if (Cameras.Count == 0)
				return;
			currentCameraIndex = (int)Mathf.Repeat(currentCameraIndex - 1, Cameras.Count);
			EnableCamera(Cameras[currentCameraIndex]);
		}

		private void EnableCamera(Camera curCamera)
		{
			foreach (Camera cam in Cameras) 
				cam.enabled = false;
			curCamera.enabled = true;
			target = curCamera.transform.position;
		}
	}
}
