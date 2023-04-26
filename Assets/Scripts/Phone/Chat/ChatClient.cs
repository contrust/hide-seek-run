using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phone.Chat
{
    public class ChatClient: MonoBehaviour
    {
        public List<Material> symbols;
        public int id;

        private ChatServer server;
        [SerializeField] private Message message1;
        [SerializeField] private Message message2;
        [SerializeField] private Message message3;

        private void Start()
        {
            StartCoroutine(ConnectToChatServer());
        }

        private IEnumerator ConnectToChatServer()
        {
            while (true)
            {
                server = FindObjectOfType<ChatServer>();
                if (server != null)
                {
                    server.Connect(this);
                    break;
                }

                yield return new WaitForSeconds(1);
            }
        }

        public void SendMessage(int symbolId)
        {
            server.CmdSendMessage(this.id, symbolId);
        }

        public void ReceiveMessage(int senderId, int symbolId)
        {
            CopyMessage(message2, message1);
            CopyMessage(message3, message2);
            message3.symbol.material = symbols[symbolId];
            message3.sender.text = $"User{senderId}";
        }

        private void CopyMessage(Message from, Message to)
        {
            to.sender.text = from.sender.text;
            to.symbol.material = from.symbol.material;
        }
    }
}