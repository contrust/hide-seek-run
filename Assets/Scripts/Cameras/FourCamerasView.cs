using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cameras;
using DefaultNamespace;
using StarterAssets;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    public UnityEvent onFourCamModeChange;
    [SerializeField] private Placeholder placeholder;
    private Camera weaponCamera;
    public static FourCamerasView instance;

    private Rect[] rects = {
        new Rect(0, 0.5f, 0.5f, 0.5f),
        new Rect(0.5f, 0.5f, 0.5f, 0.5f),
        new Rect(0, 0, 0.5f, 0.5f),
        new Rect(0.5f, 0, 0.5f, 0.5f),
    };

    private Hunter hunter;

    private void Start()
    {
        camera = Camera.main;
        hunter = GetComponent<Hunter>();
        fixedCamView = hunter.GetComponent<FixedCameraView>();
        input = GetComponent<StarterAssetsInputs>();
        placeholder = GameObject.FindGameObjectWithTag("CameraPlaceholder").GetComponent<Placeholder>();
        weaponCamera = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>();
        instance = this;
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
            if(!GameState.instance.isVictory && !UIController.instance.isPause)
            {
                if (isEnabled) 
                    DisableView();
                else 
                    StartCoroutine(EnableView());
            } 
            input.changeCameraMode = false;
        }

        // if (isEnabled)
        // {
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
        // }
        // else
        //     input.fixedCamNum = 0;
        // if (!input.changeCameraMode) return;
        // if (isEnabled) DisableView();
        // else EnableView();
        // input.changeCameraMode = false;
    }

    private IEnumerator EnableView()
    {
        Debug.Log("EnableView");
        placeholder.Show();
        weaponCamera.enabled = false;
        yield return 0; //ждем один кадр, чтобы отрисовалась заглушка
        onFourCamModeChange.Invoke();
        camera.enabled = false;
        for (var i = 0; i < Cameras.Count; i++)
        {
            Cameras[i].enabled = true;
            Cameras[i].rect = rects[i];
        }
        isEnabled = true;
        hunter.SetLight();
    }

    public void DisableView()
    {
        if (!isEnabled)
            return;
        onFourCamModeChange.Invoke();
        foreach (var cam in Cameras)
        {
            cam.enabled = false;
        }
        isEnabled = false;
        weaponCamera.enabled = true;
        camera.enabled = true;
        hunter.SetDark();
        placeholder.Hide();
    }

    private void SetFixedCam(int camNumber)
    {
        if (Cameras.Count < camNumber + 1)
            return;
        DisableView();
        fixedCamView.DisableFixedCam();
        fixedCamView.SetFixedCam(Cameras[camNumber]);
    }
}
