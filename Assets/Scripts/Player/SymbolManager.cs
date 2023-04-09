using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SymbolManager: NetworkBehaviour
{
    public List<Material> possibleSymbols;
    public static readonly UnityEvent onSymbolInserted = new UnityEvent();
    [SyncVar(hook=nameof(OnSymbolInsertedHook))]public bool correctSymbolInserted; //переделать

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
            correctSymbolInserted = false; //переделать
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
            correctSymbolInserted = true; //переделать
        }
        else
        {
            correctSymbolInserted = false;
        }
        foreach (var symbolInserter in symbolInserters)
        {
            symbolInserter.InsertionResult(result);
            symbolInserter.Block();
        }
        ChangeSymbolOnce();
    }

    private void OnSymbolInsertedHook(bool oldValue, bool newValue) //костыль чтобы работало по сети, хочется переписать все что с коробками и символами связано
    {
        if (newValue)
        {
            onSymbolInserted.Invoke();
        }
        correctSymbolInserted = newValue;
    }
}
