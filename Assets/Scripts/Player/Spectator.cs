﻿using System;
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
		}

		private void NextCamera()
		{
			currentCameraIndex = Math.Clamp(currentCameraIndex + 1, 0, Cameras.Count - 1);
			EnableCamera(Cameras[currentCameraIndex]);
		}

		private void PreviousCamera()
		{
			currentCameraIndex = Math.Clamp(currentCameraIndex - 1, 0, Cameras.Count - 1);
			EnableCamera(Cameras[currentCameraIndex]);
		}

		private void EnableCamera(Camera curCamera)
		{
			foreach (Camera cam in Cameras) 
				cam.enabled = false;
			curCamera.enabled = true;
			transform.SetParent(curCamera.transform);
		}
	}
}
