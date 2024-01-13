using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cameras;
using DefaultNamespace;
using StarterAssets;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class FourCamerasView : MonoBehaviour
{ 
    public static FourCamerasView instance;
    public UnityEvent onFourCamModeChange;
    
    private bool isEnabled;
    
    private Hunter hunter;
    private new Camera camera;
    private Camera weaponCamera;
    private StarterAssetsInputs input;
    private FixedCameraView fixedCamView;
    private Placeholder placeholder;
    private NicknamesOnCameras nicknamesOnCameras;
    
    private List<Victim> victims => FindObjectsOfType<Victim>().ToList();
    private List<Camera> cameras => victims.Select(v => v.GetComponentInChildren<Camera>()).ToList();
    

    private readonly Rect[] rects = {
        new Rect(0, 0.5f, 0.5f, 0.5f),
        new Rect(0.5f, 0.5f, 0.5f, 0.5f),
        new Rect(0, 0, 0.5f, 0.5f),
        new Rect(0.5f, 0, 0.5f, 0.5f),
    };

    private void Start()
    {
        camera = Camera.main;
        hunter = GetComponent<Hunter>();
        fixedCamView = hunter.GetComponent<FixedCameraView>();
        input = GetComponent<StarterAssetsInputs>();
        placeholder = GameObject.FindGameObjectWithTag("CameraPlaceholder").GetComponent<Placeholder>();
        nicknamesOnCameras = FindObjectOfType<NicknamesOnCameras>();
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
        for (int i = 0; i < 4; i++)
        {
            if (input.fixedCamNum == i + 1)
            {
                SetFixedCam(i);
                input.fixedCamNum = 0;
            }
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
        nicknamesOnCameras.UpdateNicknames(victims);
        weaponCamera.enabled = false;
        yield return 0; //ждем один кадр, чтобы отрисовалась заглушка
        onFourCamModeChange.Invoke();
        camera.enabled = false;
        var _cameras = cameras;
        for (var i = 0; i < _cameras.Count; i++)
        {
            _cameras[i].enabled = true;
            _cameras[i].rect = rects[i];
        }
        isEnabled = true;
        hunter.SetLight();
    }

    public void DisableView()
    {
        if (!isEnabled)
            return;
        onFourCamModeChange.Invoke();
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
        isEnabled = false;
        weaponCamera.enabled = true;
        camera.enabled = true;
        hunter.SetDark();
        placeholder.Hide();
        nicknamesOnCameras.ClearNicknames();
    }

    private void SetFixedCam(int camNumber)
    {
        var _cameras = cameras;
        if (_cameras.Count < camNumber + 1)
            return;
        DisableView();
        fixedCamView.DisableFixedCam();
        fixedCamView.SetFixedCam(_cameras[camNumber]);
    }
}
