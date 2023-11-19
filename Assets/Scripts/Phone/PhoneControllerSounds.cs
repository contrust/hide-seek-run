using Mirror;
using Phone;
using UnityEngine;

public class PhoneControllerSounds : RequireInstance<Victim>
{
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private AudioSource showPhoneSound;
    [SerializeField] private AudioSource hidePhoneSound;

    private void Start()
    {
        phoneController.onShowPhone.AddListener(PlayShowPhoneSound);
        phoneController.onHidePhone.AddListener(PlayHidePhoneSound);
    }

    private void PlayShowPhoneSound()
    {
        PlayShowPhoneSoundCommand();
    }
    
    private void PlayHidePhoneSound()
    {
        PlayHidePhoneSoundCommand();
    }

    [Command]
    private void PlayShowPhoneSoundCommand()
    {
        PlayShowPhoneSoundRpc();
    }
    
    [Command]
    private void PlayHidePhoneSoundCommand()
    {
        PlayHidePhoneSoundRpc();
    }

    [ClientRpc]
    private void PlayShowPhoneSoundRpc()
    {
        showPhoneSound.Play();
    }
    
    [ClientRpc]
    private void PlayHidePhoneSoundRpc()
    {
        hidePhoneSound.Play();
    }
}