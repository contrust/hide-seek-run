using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SymbolInsert : MonoBehaviour
{
    [SerializeField] private float SymbolInserterRadius = 1f;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    
    private void PressButton()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        var inserterButton = FindSymbolInserterButton();
        inserterButton?.Press();
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
