using System;
using UnityEngine;

public class SymbolButton : MonoBehaviour
{
	public SymbolInserter SymbolInserter;
	public ButtonType ButtonType;
	// private Outline outline;

	// public void Start()
	// {
	// 	outline = gameObject.GetComponent<Outline>();
	// 	outline.gameObject.SetActive(false);
	// }
	//
	public void OutlineSetActive(bool setActive)
	{
		// outline.gameObject.SetActive(setActive);
	}
}

public enum ButtonType
{
	Insert,
	Change
}
