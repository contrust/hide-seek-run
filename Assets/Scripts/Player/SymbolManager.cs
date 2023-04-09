using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class SymbolManager: NetworkBehaviour
{
    public List<Material> possibleSymbols;
    private MeshRenderer symbolMeshRenderer;
    public Material noneSymbol;
    private GameObject[] symbolInsertersGO;
    private SymbolInserter[] symbolInserters;
    private Hunter hunter;
    [SerializeField] private float timeChangeSymbol = 60;
    
    [SyncVar(hook = nameof(SetMaterial))] private int currentSymbol;


    void Start()
    {
        symbolInserters = FindObjectsByType<SymbolInserter>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        hunter = GetComponent<Hunter>();
        if (isLocalPlayer)
            StartCoroutine(ChangeSymbol());
    }

    public float GetTimeChangeSymbol()
    {
        return timeChangeSymbol;
    }
    
    private void SetMaterial(int oldNumber, int newNumber)
    {
        hunter.SymbolMeshRenderer.material = possibleSymbols[newNumber];
    }

    private IEnumerator ChangeSymbol()
    {
        while (true)
        {
            ChangeSymbolOnce();
            yield return new WaitForSeconds(timeChangeSymbol);
        }
    }

    public void ChangeSymbolOnce()
    {
        currentSymbol = Random.Range(0, possibleSymbols.Count);
    }
    

    public void CheckInsertedSymbol(int insertedSymbol)
    {
        var result = insertedSymbol == currentSymbol;
        foreach (var symbolInserter in symbolInserters)
        {
            symbolInserter.InsertionResult(result);
            symbolInserter.Block();
        }
    }
}
