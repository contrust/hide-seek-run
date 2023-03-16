using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    private Slider slider;
    public bool isPause { get; private set; }

    private void Start()
    {
        slider = GameObject.FindWithTag("SensSlider").GetComponent<Slider>();
    }

    public void Pause()
    {
        isPause = !isPause;
    }

    public void SensitivitySetActive(bool setActive)
    {
        foreach(Transform child in slider.transform) 
            child.gameObject.SetActive(setActive);
    }
}
