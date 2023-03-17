using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SymbolInserter : NetworkBehaviour
{
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Color correctColor;


    [SyncVar(hook = nameof(SetColor))] private Color currentColor;
    
    
    
    private SymbolManager symbolManager;
    private List<Material> possibleSymbols;
    private int chosenSymbol;
    [SyncVar(hook = nameof(SetDisplay))] private int currentSymbolIndex;

    private MeshRenderer meshRenderer;

    private bool possibleToInsert;
    [SerializeField] private float insertionTimeOut = 10f;

    [SerializeField] private GameObject screen;
    private MeshRenderer screenMeshRenderer;
    
    
    void Start()
    {
        //Кажется, лучше спаунить Inserter вместе с охотником и спокойно получать SymbolManager здесь
        meshRenderer = GetComponent<MeshRenderer>();
        screenMeshRenderer = screen.GetComponent<MeshRenderer>();
        currentSymbolIndex = 0;
        possibleToInsert = true;
        symbolManager = FindObjectOfType<SymbolManager>();
        possibleSymbols = symbolManager.possibleSymbols;
    }


    private void SetColor(Color oldColor, Color newColor)
    {
        meshRenderer.material.color = newColor;
    }

    private void SetDisplay(int oldNumber, int newNumber)
    {
        screenMeshRenderer.material = possibleSymbols[currentSymbolIndex];
    }

    public void Insert()
    {
        if (!possibleToInsert) return;
        symbolManager.CheckInsertedSymbol(currentSymbolIndex);
        StartCoroutine(InsertionTimeOutCoroutine());
    }

    public void InsertionResult(bool result)
    {
        if (result)
            CorrectInsertion();
        else
            WrongInsertion();
    }

    private void CorrectInsertion()
    {
        currentColor = correctColor;
    }

    private void WrongInsertion()
    {
        currentColor = wrongColor;
    }

    public void ChangeSymbol()
    {
        currentSymbolIndex = (int)Mathf.Repeat(currentSymbolIndex + 1, possibleSymbols.Count);
    }
    
    private IEnumerator InsertionTimeOutCoroutine()
    {
        possibleToInsert = false;
        yield return new WaitForSeconds(insertionTimeOut);
        currentColor = neutralColor;
        possibleToInsert = true;
    }
    
    private IEnumerator TryGetSymbolManager()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        while (players.Length == 0)
        {
            yield return new WaitForSeconds(1f);
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        foreach (var player in players)
        {
            symbolManager = player.GetComponent<SymbolManager>();
            if (symbolManager != null)
                break;
        }
        possibleSymbols = symbolManager.possibleSymbols;
    }
}
