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
    private IEnumerable<Camera> Cameras => FindObjectsOfType<Victim>().Select(v => v.GetComponentInChildren<Camera>());
    private StarterAssetsInputs input;
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
        if (!input.changeCameraMode) return;
        if (isEnabled) DisableView();
        else EnableView();
        input.changeCameraMode = false;
    }

    public void EnableView()
    {
        var i = 0;
        foreach (Camera cam in Cameras)
        {
            cam.enabled = true;
            cam.rect = rects[i];
            i++;
        }
        camera.enabled = false;
        isEnabled = true;
        hunter.SetLight();
    }

    public void DisableView()
    {
        foreach (var cam in Cameras)
        {
            cam.enabled = false;
        }
        isEnabled = false;
        camera.enabled = true;
        hunter.SetDark();
    }
}
