using System;
using Mirror;
using Phone.Chat;
using UnityEngine;

public class ChatClientSounds : NetworkBehaviour
{
    [SerializeField] private ChatClient chatClient;
    [SerializeField] private AudioSource receiveMessageSound;

    private void Start()
    {
        chatClient.onReceiveMessage.AddListener(PlayReceiveMessageSound);
    }

    [ClientRpc]
    private void PlayReceiveMessageSound(string sender, int symbolId)
    {
        receiveMessageSound.Play();
    }
}