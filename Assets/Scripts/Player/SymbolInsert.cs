using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class SymbolInsert : NetworkBehaviour
{
    [SerializeField] private float SymbolInserterRadius = 2f;
    private UIHelper UIHelper;
    private Camera mainCamera;
    private Dictionary<int, SymbolInserter> symbolInserters = new Dictionary<int, SymbolInserter>();
    private bool parentIsVictim;
    

    private void Start()
    {
        mainCamera = Camera.main;
        parentIsVictim = transform.parent.gameObject.GetComponent<Victim>() != null;
        UIHelper = GameObject.FindWithTag("UIHelper").GetComponent<UIHelper>();
    }
    
    public override void OnStartServer()
    {
        var findInserters = FindObjectsOfType<SymbolInserter>();
        foreach (SymbolInserter symbolInserter in findInserters)
        {
            symbolInserters[symbolInserter.id] = symbolInserter;
        }
    } 
    
    private void Update()
    {
        var inserterButton = FindSymbolInserterButton();
        if (inserterButton is null)
        {
            UIHelper.ButtonHelpSetActive(false);
            return;   
        }
        if (parentIsVictim)
            UIHelper.ButtonHelpSetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            PressButton(inserterButton.SymbolInserter.id, inserterButton.ButtonType);
        }
    }

    [Command]
    private void PressButton(int id, ButtonType buttonType)
    {
        var symbolInserter = symbolInserters[id];
        
        if (buttonType == ButtonType.Insert)
        {
            symbolInserter.Insert();
        }
        else if (buttonType == ButtonType.Change)
        {
            symbolInserter.ChangeSymbol();
        }
    }

    private SymbolButton FindSymbolInserterButton()
    {
        var cameraTransform = mainCamera.transform;
        
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo,
            SymbolInserterRadius))
            return null;
        var button = hitInfo.collider.GetComponent<SymbolButton>();
        return button;
    }
}
