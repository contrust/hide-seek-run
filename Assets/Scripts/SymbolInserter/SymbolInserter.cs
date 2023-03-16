using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInserter : MonoBehaviour
{
    [SerializeField] private Material neutralColor;
    [SerializeField] private Material wrongColor;
    [SerializeField] private Material correctColor;
    private SymbolManager symbolManager;
    private List<Material> possibleSymbols;
    private Material chosenSymbol;
    private int currentSymbolIndex;

    private MeshRenderer meshRenderer;

    private bool possibleToInsert;
    [SerializeField] private float insertionTimeOut = 10f;

    [SerializeField] private GameObject screen;
    private MeshRenderer screenMeshRenderer;
    void Start()
    {
        //Кажется, лучше спаунить Inserter вместе с охотником и спокойно получать SymbolManager здесь
        StartCoroutine(TryGetSymbolManager());
        meshRenderer = GetComponent<MeshRenderer>();
        screenMeshRenderer = screen.GetComponent<MeshRenderer>();
        currentSymbolIndex = 0;
        possibleToInsert = true;
    }

    public void Insert()
    {
        symbolManager.CheckInsertedSymbol(chosenSymbol);
        StartCoroutine(InsertionTimeOutCoroutine());
    }

    public void InsertionResult(bool result)
    {
        if (result)
            CorrectInsertion();
        else
            WrongInsertion();
    }

    private void CorrectInsertion()
    {
        meshRenderer.material = correctColor;
    }

    private void WrongInsertion()
    {
        meshRenderer.material = wrongColor;
    }

    public void ChangeSymbol()
    {
        chosenSymbol = possibleSymbols[currentSymbolIndex];
        screen.GetComponent<MeshRenderer>().material = chosenSymbol;
        currentSymbolIndex++;
        if (currentSymbolIndex == possibleSymbols.Count)
            currentSymbolIndex = 0;
    }
    
    private IEnumerator InsertionTimeOutCoroutine()
    {
        possibleToInsert = false;
        yield return new WaitForSeconds(insertionTimeOut);
        screenMeshRenderer.material = neutralColor;
        possibleToInsert = true;
    }
    
    private IEnumerator TryGetSymbolManager()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        while (players.Length == 0)
        {
            yield return new WaitForSeconds(1f);
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        foreach (var player in players)
        {
            symbolManager = player.GetComponent<SymbolManager>();
            if (symbolManager != null)
                break;
        }
        possibleSymbols = symbolManager.possibleSymbols;
        chosenSymbol = symbolManager.noneSymbol;
    }
}
