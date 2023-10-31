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

    [ClientRpc]
    private void PlayShowPhoneSound()
    {
        Debug.Log("SHOw");
        showPhoneSound.Play();
    }
    
    [ClientRpc]
    private void PlayHidePhoneSound()
    {
        Debug.Log("HIDe");
        hidePhoneSound.Play();
    }
}