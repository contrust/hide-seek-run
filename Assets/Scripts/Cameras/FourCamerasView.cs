using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class FourCamerasView : MonoBehaviour
{
    private bool isEnabled;
    public Camera[] cameras = new Camera[4];

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
        isEnabled = true;
    }

    public void DisableView()
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
        isEnabled = false;
    }
}
