using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Symbols
{
    public class SymbolManager: RequireInstance<Hunter>
    {
        [SyncVar(hook = nameof(SetMaterial))] private int currentSymbol = -1;

        [field: SerializeField]
        [field: SyncVar(hook = nameof(SetCorrectInsertions))]
        public int CurrentCorrectInsertions { get; private set; }

        public List<Material> PossibleSymbols;
    
        private Hunter hunter;
        private MatchSettings matchSettings;
    
        public static readonly UnityEvent<bool> OnSymbolInserted = new UnityEvent<bool>();
        public static readonly UnityEvent<int> OnSymbolChanged = new UnityEvent<int>();
        public static readonly UnityEvent OnVictimsVictory = new UnityEvent();

        protected override void CallbackAll(Hunter instance)
        {
            hunter = instance;
            matchSettings = FindObjectOfType<MatchSettings>();
        }

        protected override void CallbackServer()
        {
            StartCoroutine(ChangeSymbol());
        }
    
        public void InsertSymbol(int insertedSymbol)
        {
            if (insertedSymbol == currentSymbol)
                CurrentCorrectInsertions++;
            OnSymbolInserted?.Invoke(insertedSymbol == currentSymbol);
        }

        private void SetCorrectInsertions(int _, int newCorrectInsertions)
        {
            CurrentCorrectInsertions = newCorrectInsertions;
            if (CurrentCorrectInsertions == matchSettings.CountCorrectSymbolsToWin)
                OnVictimsVictory?.Invoke();
        }
    
        private void SetMaterial(int _, int newSymbol)
        {
            StartCoroutine(SetMaterialCoroutine(newSymbol));
        }
    
        private void ChangeSymbolOnce()
        {
            var newSymbol = currentSymbol;
            while (currentSymbol == newSymbol)
            {
                newSymbol = Random.Range(0, PossibleSymbols.Count);
            }
            currentSymbol = newSymbol;
            OnSymbolChanged?.Invoke(currentSymbol);
        }

        private IEnumerator ChangeSymbol()
        {
            while (true)
            {
                ChangeSymbolOnce();
                yield return new WaitForSeconds(matchSettings.timeChangeSymbol);
            }
        }

        private IEnumerator SetMaterialCoroutine(int newSymbol)
        {
            while (hunter is null)
                yield return null;
            hunter.SetSymbol(PossibleSymbols[newSymbol]);
        }
    }
}
