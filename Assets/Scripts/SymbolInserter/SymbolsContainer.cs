using System.Collections.Generic;
using UnityEngine;

public class SymbolsContainer : MonoBehaviour
{
    [SerializeField] public List<Material> symbols;

    public Material GetSymbol(int symbolId)
    {
        return symbols[symbolId];
    }
}