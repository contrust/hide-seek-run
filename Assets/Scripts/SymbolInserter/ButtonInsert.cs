using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInsert : MonoBehaviour, IInserterButton
{
    [SerializeField] private SymbolInserter symbolInserter;

    public void Press()
    {
        Debug.Log("Insert");
        symbolInserter.Insert();
    }
}
