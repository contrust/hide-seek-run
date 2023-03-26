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
    
    [SyncVar(hook = nameof(SetMaterial))] private int currentSymbol;


    void Start()
    {
        symbolInserters = FindObjectsByType<SymbolInserter>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        if (isLocalPlayer)
            StartCoroutine(ChangeSymbol());
    }
    
    
    private void SetMaterial(int oldNumber, int newNumber)
    {
        GetComponent<Hunter>().SymbolMeshRenderer.material = possibleSymbols[newNumber];
    }

    private IEnumerator ChangeSymbol()
    {
        while (true)
        {
            ChangeSymbolOnce();
            yield return new WaitForSeconds(10f);
        }
    }

    public void ChangeSymbolOnce()
    {
        currentSymbol = Random.Range(0, possibleSymbols.Count);
    }
    

    public void CheckInsertedSymbol(int insertedSymbol)
    {
        foreach (var symbolInserter in symbolInserters)
            symbolInserter.InsertionResult(insertedSymbol == currentSymbol);
    }
}
