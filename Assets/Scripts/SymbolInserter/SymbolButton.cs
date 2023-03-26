using UnityEngine;

public class SymbolButton : MonoBehaviour
{
	public SymbolInserter SymbolInserter;
	public ButtonType ButtonType;
}

public enum ButtonType
{
	Insert,
	Change
}
