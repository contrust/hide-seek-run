using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI survivorsVictoryText;
    public bool isPause { get; private set; }

    private void Start()
    {
        slider = GameObject.FindWithTag("SensSlider").GetComponent<Slider>();
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
    }

    public void ShowVictimsVictoryScreen()
    {
        survivorsVictoryText.gameObject.SetActive(true);
    }
}