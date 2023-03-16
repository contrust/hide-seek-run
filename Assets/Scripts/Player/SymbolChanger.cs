using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolChanger : MonoBehaviour
{
    [SerializeField] private List<Material> possibleSymbols;
    [SerializeField] private GameObject symbol;
    [SerializeField] private Material noneSymbol;
    
    
    void Start()
    {
        StartCoroutine(ChangeSymbol());
    }

    private IEnumerator ChangeSymbol()
    {
        while (true)
        {
            var currentSymbol = possibleSymbols[Random.Range(0, possibleSymbols.Count)];
            symbol.GetComponent<MeshRenderer>().material = currentSymbol;
            yield return new WaitForSeconds(10f);
        }
    }
}
