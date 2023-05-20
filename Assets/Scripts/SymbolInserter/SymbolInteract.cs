using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SymbolInteract : NetworkBehaviour
{
    [SerializeField] private float SymbolInserterRadius = 2f;
    private UIHelper UIHelper;
    private Camera mainCamera;
    private bool parentIsVictim;
    

    private void Start()
    {
        mainCamera = Camera.main;
        UIHelper = FindObjectOfType<UIHelper>(true);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        var inserterButton = FindSymbolInserterButton();
        if (inserterButton is null)
        {
            UIHelper.ButtonHelpSetActive(false);
            return;   
        }
        if (isLocalPlayer)
            UIHelper.ButtonHelpSetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            PressButton(inserterButton);
        }
    }

    [Command]
    private void PressButton(SymbolButton button)
    {
        button.Pressed?.Invoke();
    }

    private SymbolButton FindSymbolInserterButton()
    {
        var cameraTransform = mainCamera.transform;
        
        SymbolButton button = null;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, SymbolInserterRadius))
            button = hitInfo.collider.GetComponent<SymbolButton>();
        
        return button;
    }
}
