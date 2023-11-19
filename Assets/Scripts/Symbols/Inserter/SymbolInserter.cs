using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Symbols.Inserter
{
    public class SymbolInserter : RequireInstance<SymbolManager>
    {
        [SyncVar] public int ID;
        [SyncVar] private bool isSender;
    
        [SyncVar(hook = nameof(SetColor))] private Color currentColor;
        [SyncVar(hook = nameof(SetExpirationColor))] private Color currentExpirationColor;
        [SyncVar(hook = nameof(SetDisplay))] private int currentSymbolIndex;

        [SerializeField] private Material outOfOrder;
    
        [SerializeField] private Color neutralColor;
        [SerializeField] private Color wrongColor;
        [SerializeField] private Color correctColor; 
        [SerializeField] private float insertionTimeOut = 10f;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshRenderer screen;
        [SerializeField] private MeshRenderer expirationSignal;
        [SerializeField] private SymbolExpirationIndicator expirationIndicator;
        public readonly UnityEvent onCorrectSymbol = new ();
        public readonly UnityEvent onWrongSymbol = new ();

        private int chosenSymbol;
        private bool possibleToInsert = true;
    
        private SymbolManager symbolManager;

        protected override void CallbackAll(SymbolManager instance)
        {
            symbolManager = instance;
        }

        protected override void CallbackServer()
        {
            SymbolManager.OnSymbolInserted.AddListener(InsertionResult);
            onCorrectSymbol.AddListener(BlockAfterCorrectInsertion);
            StartCoroutine(expirationIndicator.ChangeExpirationSignalColors());
        } 

        public void InsertSymbol()
        {
            if (!possibleToInsert) return;
            isSender = true;
            symbolManager.InsertSymbol(currentSymbolIndex);
        }
    
        public void ChangeSymbol()
        {
            if (isSender)
                return;
            currentSymbolIndex = (int)Mathf.Repeat(currentSymbolIndex + 1, symbolManager.PossibleSymbols.Count);
        }

        private void SetColor(Color _, Color newColor) => meshRenderer.material.color = newColor;
        private void SetExpirationColor(Color _, Color newColor) => expirationSignal.material.color = newColor;

        private void SetDisplay(int _, int newNumber)
        {
            screen.material = newNumber == - 1 ? outOfOrder : symbolManager.PossibleSymbols[newNumber];
        }

        private void InsertionResult(bool result)
        {
            if (!possibleToInsert)
                return;
            currentColor = result ? correctColor : wrongColor;
            if (isSender)
            {
                if (result)
                {
                    onCorrectSymbol.Invoke();
                    return;
                }
                onWrongSymbol.Invoke();
            }
            StartCoroutine(BlockInsertionCoroutine());
        }

        private void BlockAfterCorrectInsertion()
        {
            possibleToInsert = false;
            currentSymbolIndex = -1;
            expirationIndicator.isActive = false;
        }

        private IEnumerator BlockInsertionCoroutine()
        {
            possibleToInsert = false;
            isSender = false;
            yield return new WaitForSeconds(insertionTimeOut);
            currentColor = neutralColor;
            possibleToInsert = true;
        }
    }
}
