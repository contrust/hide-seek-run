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

    [ClientRpc]
    private void PlayShowPhoneSound()
    {
        Debug.Log("SHOw");
        showPhoneSound.Play();
    }
    
    [Command]
    private void CmdPlayHidePhoneSound()
    {
        PlayHidePhoneSound();
    }
    
    [ClientRpc]
    private void PlayHidePhoneSound()
    {
        Debug.Log("HIDe");
        hidePhoneSound.Play();
    }
}