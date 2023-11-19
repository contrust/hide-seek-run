using System.Linq;
using Mirror;
using Symbols.Inserter;
using UnityEngine;

namespace Symbols
{
	public static class CustomReadWriteFunctions
	{
		public static void WriteSymbolButton(this NetworkWriter writer, SymbolButton symbolButton)
		{
			var c = symbolButton.SymbolInserter.GetComponent<NetworkIdentity>();
			writer.WriteNetworkIdentity(c);
			Debug.Log(c.netId);
			writer.WriteInt((int) symbolButton.ButtonType);
			Debug.Log(symbolButton.ButtonType);
		}
	
		public static SymbolButton ReadSymbolButton(this NetworkReader reader)
		{
			NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
			ButtonType buttonType = (ButtonType) reader.ReadInt();
			var symbolButton = networkIdentity.GetComponentsInChildren<SymbolButton>().First(btn => btn.ButtonType == buttonType);
			return symbolButton;
		}
	}
}
