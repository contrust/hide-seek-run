using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SymbolManager: RequireInstance<Hunter>
{
    [SyncVar(hook = nameof(SetMaterial))] private int currentSymbol;
    [SyncVar(hook = nameof(SetCorrectInsertions))] private int currentCorrectInsertions;
    
    public List<Material> PossibleSymbols;
    
    private Hunter hunter;
    private MatchSettings matchSettings;
    
    public static readonly UnityEvent<bool> OnSymbolInserted = new UnityEvent<bool>();
    public static readonly UnityEvent<int> OnSymbolChanged = new UnityEvent<int>();
    public static readonly UnityEvent OnVictimsVictory = new UnityEvent();

    protected override void CallbackAll(Hunter instance)
    {
        hunter = instance;
        matchSettings = FindObjectOfType<MatchSettings>();
        CustomNetworkManager.OnSceneLoadedForPlayer += SetDirty;
    }

    protected override void CallbackServer()
    {
        StartCoroutine(ChangeSymbol());
    }
    
    public void InsertSymbol(int insertedSymbol)
    {
        if (insertedSymbol == currentSymbol)
            currentCorrectInsertions++;
        OnSymbolInserted?.Invoke(insertedSymbol == currentSymbol);
    }

    private void SetCorrectInsertions(int _, int newCorrectInsertions)
    {
        currentCorrectInsertions = newCorrectInsertions;
        if (currentCorrectInsertions == matchSettings.CountCorrectSymbolsToWin)
            OnVictimsVictory?.Invoke();
    }
    
    private void SetMaterial(int _, int newSymbol)
    {
        hunter.SymbolMeshRenderer.material = PossibleSymbols[newSymbol];
    }
    
    private void ChangeSymbolOnce()
    {
        currentSymbol = Random.Range(0, PossibleSymbols.Count);
        OnSymbolChanged?.Invoke(currentSymbol);
    }

    private IEnumerator ChangeSymbol()
    {
        while (true)
        {
            ChangeSymbolOnce();
            yield return new WaitForSeconds(matchSettings.timeChangeSymbol);
        }
    }
}