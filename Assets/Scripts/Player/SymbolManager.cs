using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolManager: MonoBehaviour
{
    public readonly List<Material> possibleSymbols;
    [SerializeField] private GameObject symbol;
    public readonly Material noneSymbol;
    private GameObject[] symbolInserters;
    private Material currentSymbol;


    void Start()
    {
        StartCoroutine(ChangeSymbol());
        symbolInserters = GameObject.FindGameObjectsWithTag("SymbolInserter");
    }

    private IEnumerator ChangeSymbol()
    {
        while (true)
        {
            currentSymbol = possibleSymbols[Random.Range(0, possibleSymbols.Count)];
            symbol.GetComponent<MeshRenderer>().material = currentSymbol;
            yield return new WaitForSeconds(10f);
        }
    }

    public void CheckInsertedSymbol(Material insertedSymbol)
    {
        foreach (var symbolInserter in symbolInserters)
            symbolInserter.GetComponent<SymbolInserter>().InsertionResult(insertedSymbol.Equals(currentSymbol));
    }
}
