using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SymbolInserter : NetworkBehaviour
{
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Color correctColor;

    private UIHelper uiHelper;


    [SyncVar(hook = nameof(SetColor))] private Color currentColor;
    [SyncVar] public int id;


    private SymbolManager symbolManager;
    private List<Material> possibleSymbols;
    private int chosenSymbol;
    [SyncVar(hook = nameof(SetCorrectInsertions))]
    private int correctInsertions;
    [SyncVar(hook = nameof(SetDisplay))] private int currentSymbolIndex;

    [SerializeField] private MeshRenderer meshRenderer;

    private bool possibleToInsert;
    [SerializeField] private float insertionTimeOut = 10f;

    [SerializeField] private GameObject screen;
    private MeshRenderer screenMeshRenderer;
    private MatchSettings matchSettings;


    void Start()
    {
        screenMeshRenderer = screen.GetComponent<MeshRenderer>();
        uiHelper = GameObject.FindWithTag("UIHelper").GetComponent<UIHelper>();
        matchSettings = FindObjectOfType<MatchSettings>();
        currentColor = neutralColor;
        currentSymbolIndex = 0;
        possibleToInsert = true;
        StartCoroutine(TryGetSymbolManager());
    }


    private void SetColor(Color oldColor, Color newColor)
    {
        Debug.Log("SetColor");
        meshRenderer.material.color = newColor;
    }

    private void SetDisplay(int oldNumber, int newNumber)
    {
        Debug.Log("SetDisplay");
        screenMeshRenderer.material = possibleSymbols[newNumber];
    }

    private void SetCorrectInsertions(int oldNumber, int newNumber)
    {
        if (correctInsertions == matchSettings.CountCorrectSymbolsToWin)
            CommitVictimsVictory();
    }

    public void Insert()
    {
        if (!possibleToInsert) return;
        symbolManager.CheckInsertedSymbol(currentSymbolIndex);
    }

    public void Block()
    {
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
        currentColor = correctColor;
        correctInsertions++;
    }

    private void CommitVictimsVictory()
    {
        uiHelper.ShowVictimsVictoryScreen();
    }

    private void WrongInsertion()
    {
        currentColor = wrongColor;
    }

    public void ChangeSymbol()
    {
        currentSymbolIndex = (int)Mathf.Repeat(currentSymbolIndex + 1, possibleSymbols.Count);
    }
    
    private IEnumerator InsertionTimeOutCoroutine()
    {
        possibleToInsert = false;
        yield return new WaitForSeconds(insertionTimeOut);
        currentColor = neutralColor;
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
    }
}
