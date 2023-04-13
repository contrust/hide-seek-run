using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SymbolManager: NetworkBehaviour
{
    public List<Material> possibleSymbols;
    private MeshRenderer symbolMeshRenderer;
    private SymbolInserter[] symbolInserters;
    private Hunter hunter;
    private float timeChangeSymbol;
    
    [SyncVar(hook = nameof(SetMaterial))] private int currentSymbol;

    #region onSymbolInsertedEvent
    public static readonly UnityEvent onSymbolInserted = new();
    [Command]
    private void CmdInvokeOnSymbolInserted()
    {
        RpcInvokeOnSymbolInserted();
    }
    [ClientRpc]
    void RpcInvokeOnSymbolInserted()
    {
        onSymbolInserted.Invoke();
    }
    #endregion
    
    
    void Start()
    {
        symbolInserters = FindObjectsByType<SymbolInserter>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        timeChangeSymbol = FindObjectOfType<MatchSettings>().timeChangeSymbol;
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
        if (result)
        {
            CmdInvokeOnSymbolInserted();
        }
        foreach (var symbolInserter in symbolInserters)
        {
            symbolInserter.InsertionResult(result);
            symbolInserter.Block();
        }
    }

    
}
