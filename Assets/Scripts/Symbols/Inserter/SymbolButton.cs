using UnityEngine;
using UnityEngine.Events;

namespace Symbols.Inserter
{
	public class SymbolButton : MonoBehaviour
	{
		public SymbolInserter SymbolInserter;
		public ButtonType ButtonType;
		public UnityEvent Pressed;
	}

	public enum ButtonType
	{
		Insert,
		Change
	}
}