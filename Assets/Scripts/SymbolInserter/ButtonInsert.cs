using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInsert : MonoBehaviour, IInserterButton
{
    [SerializeField] private GameObject symbolInserterGO;
    private SymbolInserter symbolInserter;
    void Start()
    {
        symbolInserter = symbolInserterGO.GetComponent<SymbolInserter>();
    }
    public void Press()
    {
        Debug.Log("Insert");
        symbolInserter.Insert();
    }
}
