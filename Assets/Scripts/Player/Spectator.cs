using System;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

namespace Player
{
	public class Spectator : MonoBehaviour
	{
		private StarterAssetsInputs input;
		private List<Camera> Cameras => FindObjectsOfType<Victim>().Select(v => v.GetComponentInChildren<Camera>()).ToList();
		private int currentCameraIndex = -1;

		private void Start()
		{
			input = GetComponent<StarterAssetsInputs>();
			input.enabled = true;
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
		}

		private void NextCamera()
		{
			currentCameraIndex = Math.Clamp(currentCameraIndex + 1, 0, Cameras.Count);
			EnableCamera(Cameras[currentCameraIndex]);
		}

		private void PreviousCamera()
		{
			currentCameraIndex = Math.Clamp(currentCameraIndex - 1, 0, Cameras.Count);
			EnableCamera(Cameras[currentCameraIndex]);
		}

		private void EnableCamera(Camera curCamera)
		{
			foreach (Camera cam in Cameras) 
				cam.enabled = false;
			curCamera.enabled = true;
			Camera.main.transform.SetParent(curCamera.transform);
		}
	}
}
