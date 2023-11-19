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

    private void PlayReceiveMessageSound(string sender, int symbolId)
    {
        PlayReceiveMessageSoundCommand(sender, symbolId);
    }
    
    [Command]
    private void PlayReceiveMessageSoundCommand(string sender, int symbolId)
    {
        PlayReceiveMessageSoundRpc(sender, symbolId);
    }

    [ClientRpc]
    private void PlayReceiveMessageSoundRpc(string sender, int symbolId)
    {
        receiveMessageSound.Play();
    }
}