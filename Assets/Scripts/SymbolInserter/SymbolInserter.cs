using System.Collections;
using Mirror;
using UnityEngine;

public class SymbolInserter : RequireInstance<SymbolManager>
{
    [SyncVar] public int ID;
    
    [SyncVar(hook = nameof(SetColor))] private Color currentColor;
    [SyncVar(hook = nameof(SetExpirationColor))] private Color currentExpirationColor;
    [SyncVar(hook = nameof(SetDisplay))] private int currentSymbolIndex;
    
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Color correctColor;

    [SerializeField] private Color expireSoonColor;
    [SerializeField] private Color expireAfterSomeTimeColor;
    [SerializeField] private Color expireNotSoonColor;
    [SerializeField] private Color expireBlack;
    [SerializeField] private float blinkingTime = 5;
    [SerializeField] private float insertionTimeOut = 10f;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer screen;
    [SerializeField] private MeshRenderer expirationSignal;
    
    private int chosenSymbol;
    private float changeExpirationSignalTime = -1;
    private bool possibleToInsert = true;
    
    private SymbolManager symbolManager;
    private MatchSettings matchSettings;
    

    protected override void CallbackAll(SymbolManager instance)
    {
        symbolManager = instance;
        matchSettings = FindObjectOfType<MatchSettings>();
    }
    protected override void CallbackServer()
    {
        SymbolManager.OnSymbolInserted.AddListener(InsertionResult);
        
        changeExpirationSignalTime = matchSettings.timeChangeSymbol / 3;
        StartCoroutine(ChangeExpirationSignalColors());
    } 

    public void InsertSymbol()
    {
        if (!possibleToInsert) return;
        symbolManager.InsertSymbol(currentSymbolIndex);
    }
    
    public void ChangeSymbol()
    {
        currentSymbolIndex = (int)Mathf.Repeat(currentSymbolIndex + 1, symbolManager.PossibleSymbols.Count);
    }

    private void SetColor(Color _, Color newColor) => meshRenderer.material.color = newColor;
    private void SetExpirationColor(Color _, Color newColor) => expirationSignal.material.color = newColor;
    private void SetDisplay(int _, int newNumber) => screen.material = symbolManager.PossibleSymbols[newNumber];
    
    private void InsertionResult(bool result)
    {
        currentColor = result ? correctColor : wrongColor;
        StartCoroutine(BlockInsertionCoroutine());
    }
    
    private IEnumerator BlockInsertionCoroutine()
    {
        possibleToInsert = false;
        yield return new WaitForSeconds(insertionTimeOut);
        currentColor = neutralColor;
        possibleToInsert = true;
    }
    
    private IEnumerator ChangeExpirationSignalColors()
    {
        while (true)
        {
            currentExpirationColor = expireNotSoonColor;
            yield return new WaitForSeconds(changeExpirationSignalTime);
            currentExpirationColor = expireAfterSomeTimeColor;
            yield return new WaitForSeconds(changeExpirationSignalTime);
            currentExpirationColor = expireSoonColor;
            yield return new WaitForSeconds(changeExpirationSignalTime - blinkingTime);
            
            for (var i = 0; i < blinkingTime; i++)
            {
                currentExpirationColor = expireBlack;
                yield return new WaitForSeconds(0.5f);
                currentExpirationColor = expireSoonColor;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
