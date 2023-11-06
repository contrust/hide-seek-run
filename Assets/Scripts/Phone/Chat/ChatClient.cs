using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Phone.Chat
{
    public class ChatClient: MonoBehaviour
    {
        public int id;
        public SymbolsContainer symbolsContainer;
        private ChatServer server;
        [SerializeField] private List<Message> messages;
        [SerializeField] private string username;
        public UnityEvent<string, int> onReceiveMessage = new();

        public void SendMessage(int symbolId)
        {
            server.CmdSendMessage(username, symbolId);
        }

        private void Start()
        {
            onReceiveMessage.AddListener(UpdateMessages);
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

        private void UpdateMessages(string senderName, int symbolId)
        {
            for (var i = 1; i < messages.Count; i++)
            {
                Debug.Log($"i = {i}");
                messages[i - 1].CopyValues(messages[i]);
            }

            messages[^1].SetValues(senderName, symbolId);
        }
    }
}