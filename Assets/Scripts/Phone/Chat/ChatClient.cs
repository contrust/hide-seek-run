using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private string username;
        public UnityEvent<string, int> onReceiveMessage = new();

        private void Start()
        {
            onReceiveMessage.AddListener(UpdateMessageQueue);
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
            OnConnectCallback();
        }

        private void OnConnectCallback()
        {
            username = GetUsername();
        }
        
        private string GetUsername()
        {
            var localPlayer = NetworkClient.localPlayer.GetComponent<Victim>();
            if (localPlayer is null || localPlayer.steamName == "")
            {
                return $"User{id}";
            }

            return localPlayer.steamName;
        }

        public void SendMessage(int symbolId)
        {
            server.CmdSendMessage(username, symbolId);
        }

        public void UpdateMessageQueue(string senderName, int symbolId)
        {
            CopyMessage(message2, message1);
            CopyMessage(message3, message2);
            message3.symbol.material = symbols[symbolId];
            message3.sender.text = senderName;
        }

        private void CopyMessage(Message from, Message to)
        {
            to.sender.text = from.sender.text;
            to.symbol.material = from.symbol.material;
        }
    }
}