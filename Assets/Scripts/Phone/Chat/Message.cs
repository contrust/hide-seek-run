using Symbols;
using TMPro;
using UnityEngine;

namespace Phone.Chat
{
    public class Message: MonoBehaviour
    {
        public MeshRenderer symbol;
        public TextMeshPro sender;
        public SymbolsContainer symbolsContainer;

        public void SetValues(string senderName, int symbolId)
        {
            symbol.material = symbolsContainer.GetSymbol(symbolId);
            sender.text = senderName;
        }

        public void CopyValues(Message from)
        {
            symbol.material = from.symbol.material;
            sender.text = from.sender.text;
        }
    }
}