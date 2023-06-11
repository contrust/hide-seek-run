using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    public Transform Parent;
    
    private Camera mainCam;
    private CharacterController controller;
    private PlayerInput playerInput;
    

    void Awake()
    {
        mainCam = Camera.main;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    public override void OnStartLocalPlayer()
    {
        if (mainCam != null)
        {
            // configure and make camera a child of player with 3rd person offset
            mainCam.orthographic = false;
            mainCam.transform.SetParent(Parent);
            SetControl(true);
            mainCam.transform.localPosition = new Vector3(0f, 0f, -0f);
            mainCam.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            
        }
        else
            Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");
    }

    public override void OnStopLocalPlayer()
    {
        if (mainCam != null)
        {
            mainCam.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
            mainCam.orthographic = true;
            mainCam.orthographicSize = 25f;
            mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }

    public void SetControl(bool isEnabled)
    {
        controller.enabled = isEnabled;
        playerInput.enabled = isEnabled;
    }
}