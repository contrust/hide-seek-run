using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI survivorsVictoryText;
    [SerializeField] private TextMeshProUGUI buttonHelp;
    [SerializeField] private Button hostLobbyButton;
    [SerializeField] private Button leaveLobbyButton;
    public bool isPause { get; private set; }

    private void Start()
    {
        slider = GameObject.FindWithTag("SensSlider").GetComponent<Slider>();
        SymbolManager.OnVictimsVictory.AddListener(ShowVictimsVictoryScreen);
    }

    public void Pause()
    {
        isPause = !isPause;
        SensitivitySetActive(isPause);
    }

    public void SensitivitySetActive(bool setActive)
    {
        foreach(Transform child in slider.transform) 
            child.gameObject.SetActive(setActive);
        slider.interactable = setActive;
    }

    public void ShowVictimsVictoryScreen()
    {
        survivorsVictoryText.gameObject.SetActive(true);
    }

    public void ButtonHelpSetActive(bool setActive)
    {
        buttonHelp.gameObject.SetActive(setActive);
    }

    public void ButtonHostLobbySetActive(bool setActive)
    {
        hostLobbyButton.gameObject.SetActive(setActive);
    }

    public void ButtonLeaveLobbySetActive(bool setActive)
    {
        leaveLobbyButton.gameObject.SetActive(setActive);
    }

    public void LobbyEnterUISetActive(bool setActive)
    {
        ButtonHostLobbySetActive(!setActive);
        SensitivitySetActive(!setActive);
        ButtonLeaveLobbySetActive(setActive);
    }
}
