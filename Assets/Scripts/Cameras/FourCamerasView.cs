using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class FourCamerasView : MonoBehaviour
{ 
    private new Camera camera;
    private List<Camera> Cameras => FindObjectsOfType<Victim>().Select(v => v.GetComponentInChildren<Camera>()).ToList();
    private StarterAssetsInputs input;
    private FixedCameraView fixedCamView;
    private bool isEnabled;

    private Rect[] rects = {
        new Rect(0, 0, 0.5f, 0.5f),
        new Rect(0.5f, 0, 0.5f, 0.5f),
        new Rect(0, 0.5f, 0.5f, 0.5f),
        new Rect(0.5f, 0.5f, 0.5f, 0.5f),
    };

    private Hunter hunter;

    private void Start()
    {
        camera = Camera.main;
        hunter = GetComponent<Hunter>();
        fixedCamView = hunter.GetComponent<FixedCameraView>();
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        /*
         * Added this method for testing purposes
         */
        /*if (input.GetKeyDown(KeyCode.E))
        {
            if (isEnabled) DisableView();
            else EnableView();
        }*/
        UpdateCameraMode();
    }

    private void UpdateCameraMode()

    {
        if (input.changeCameraMode)
        {
            if (isEnabled) 
                DisableView();
            else
                EnableView();
            input.changeCameraMode = false;
        }

        if (isEnabled)
        {
            if (input.fixedCamNum == 1)
            {
                SetFixedCam(0);
                input.fixedCamNum = 0;
            }

            if (input.fixedCamNum == 2)
            {
                SetFixedCam(1);
                input.fixedCamNum = 0;
            }

            if (input.fixedCamNum == 3)
            {
                SetFixedCam(2);
                input.fixedCamNum = 0;
            }

            if (input.fixedCamNum == 4)
            {
                SetFixedCam(3);
                input.fixedCamNum = 0;
            }
        }
        else
            input.fixedCamNum = 0;
        // if (!input.changeCameraMode) return;
        // if (isEnabled) DisableView();
        // else EnableView();
        // input.changeCameraMode = false;
    }

    private void EnableView()
    {
        for (var i = 0; i < Cameras.Count; i++)
        {
            Cameras[i].enabled = true;
            Cameras[i].rect = rects[i];
        }
        camera.enabled = false;
        isEnabled = true;
        hunter.SetLight();
    }

    private void DisableView()
    {
        foreach (var cam in Cameras)
        {
            cam.enabled = false;
        }
        isEnabled = false;
        camera.enabled = true;
        hunter.SetDark();
    }

    private void SetFixedCam(int camNumber)
    {
        if (Cameras.Count < camNumber + 1)
            return;
        Debug.Log("Got it");
        DisableView();
        fixedCamView.DisableFixedCam();
        fixedCamView.SetFixedCam(Cameras[camNumber]);
    }
}
