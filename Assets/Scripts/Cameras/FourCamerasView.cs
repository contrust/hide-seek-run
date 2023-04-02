using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class FourCamerasView : MonoBehaviour
{
    private Camera camera;

    private IEnumerable<Camera> Cameras => FindObjectsOfType<Victim>().Select(v => v.GetComponentInChildren<Camera>());
    private bool isEnabled;

    private MatchSettings matchSettings;
    private Rect[] rects = {
        new Rect(0, 0, 0.5f, 0.5f),
        new Rect(0.5f, 0, 0.5f, 0.5f),
        new Rect(0, 0.5f, 0.5f, 0.5f),
        new Rect(0.5f, 0.5f, 0.5f, 0.5f),
    };

    private Hunter hunter;

    private void Start()
    {
        matchSettings = GameObject.FindObjectOfType<MatchSettings>();
        camera = Camera.main;
        hunter = GetComponent<Hunter>();
    }

    void Update()
    {
        /*
         * Added this method for testing purposes
         */
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isEnabled) DisableView();
            else EnableView();
        }
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
