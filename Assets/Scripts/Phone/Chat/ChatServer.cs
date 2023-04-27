using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Phone.Chat
{
    public class ChatServer: NetworkBehaviour
    {
        [SerializeField]private List<ChatClient> connectedClients = new List<ChatClient>();
        [SerializeField]private List<Material> symbols = new List<Material>();

        public void Connect(ChatClient chatClient)
        {
            connectedClients.Add(chatClient);
            chatClient.symbols = symbols;
            chatClient.id = connectedClients.Count-1;
        }

        [Command(requiresAuthority = false)]
        public void CmdSendMessage(string username, int symbolId)
        {
            Debug.Log("CMDSendMessage");
            RpcSendMessage(username, symbolId);
        }
        
        [ClientRpc]
        private void RpcSendMessage(string username, int symbolId)
        {
            Debug.Log("RPCSendMessage");
            foreach (var client in connectedClients)
            {
                client.ReceiveMessage(username, symbolId);
            }
        }
        
    }
}