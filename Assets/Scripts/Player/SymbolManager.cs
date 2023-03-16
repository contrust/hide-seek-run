using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymbolManager: MonoBehaviour
{
    public List<Material> possibleSymbols;
    [SerializeField] private GameObject symbol;
    private MeshRenderer symbolMeshRenderer;
    public Material noneSymbol;
    private GameObject[] symbolInsertersGO;
    private SymbolInserter[] symbolInserters;
    private Material currentSymbol;


    void Start()
    {
        symbolMeshRenderer = symbol.GetComponent<MeshRenderer>();
        symbolInsertersGO = GameObject.FindGameObjectsWithTag("SymbolInserter");
        symbolInserters = symbolInsertersGO.Select(x => x.GetComponent<SymbolInserter>()).ToArray();
        StartCoroutine(ChangeSymbol());
        
    }

    private IEnumerator ChangeSymbol()
    {
        while (true)
        {
            currentSymbol = possibleSymbols[Random.Range(0, possibleSymbols.Count)];
            symbolMeshRenderer.material = currentSymbol;
            yield return new WaitForSeconds(10f);
        }
    }

    public void CheckInsertedSymbol(Material insertedSymbol)
    {
        foreach (var symbolInserter in symbolInserters)
            symbolInserter.InsertionResult(insertedSymbol.Equals(currentSymbol));
    }
}
