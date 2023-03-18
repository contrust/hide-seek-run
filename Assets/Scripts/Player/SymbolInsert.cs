using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class SymbolInsert : NetworkBehaviour
{
    [SerializeField] private float SymbolInserterRadius = 5f;
    private Camera mainCamera;
    private SymbolInserter symbolInserter;
    

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    public override void OnStartServer()
    {
        symbolInserter = FindObjectOfType<SymbolInserter>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var inserterButton = FindSymbolInserterButton();
            if (inserterButton is null) return;
            
            PressButton(inserterButton is ButtonInsert);
        }
    }

    [Command]
    private void PressButton(bool action)
    {
        if (action)
            symbolInserter.Insert();
        else
            symbolInserter.ChangeSymbol();
    }

    private IInserterButton FindSymbolInserterButton()
    {
        var cameraTransform = mainCamera.transform;
        
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo,
            SymbolInserterRadius))
            return null;
        var button = hitInfo.collider.GetComponent<IInserterButton>();
        return button;
    }
}
