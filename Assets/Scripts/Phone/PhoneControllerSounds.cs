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
        phoneController.onShowPhone.AddListener(CmdPlayShowPhoneSound);
        phoneController.onHidePhone.AddListener(CmdPlayHidePhoneSound);
    }

    [Command]
    private void CmdPlayShowPhoneSound()
    {
        PlayShowPhoneSound();
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
    
    [Command]
    private void CmdPlayHidePhoneSound()
    {
        PlayHidePhoneSound();
    }
    
    [ClientRpc]
    private void PlayHidePhoneSoundRpc()
    {
        hidePhoneSound.Play();
    }
}