using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChange : MonoBehaviour, IInserterButton
{
    [SerializeField] private SymbolInserter symbolInserter;

    public void Press()
    {
        Debug.Log("Change");
        symbolInserter.ChangeSymbol();
    }
}
