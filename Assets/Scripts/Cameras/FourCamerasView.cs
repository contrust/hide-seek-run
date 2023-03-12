using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class FourCamerasView : MonoBehaviour
{
    private bool _isEnabled;
    public Camera[] cameras = new Camera[4];

    void Update()
    {
        /*
         * Added this method for testing purposes
         */
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_isEnabled) DisableView();
            else EnableView();
        }
    }

    public void EnableView()
    {
        foreach (var cam in cameras)
        {
            cam.enabled = true;
        }
        cameras[0].rect = new Rect(0, 0, 0.5f, 0.5f);
        cameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
        cameras[2].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        cameras[3].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        _isEnabled = true;
    }

    public void DisableView()
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
        _isEnabled = false;
    }
}
