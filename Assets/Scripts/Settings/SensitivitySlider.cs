using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SensitivitySlider: MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderValue;

        private void Start()
        {
            slider.onValueChanged.AddListener(UpdateSensitivity);
            slider.onValueChanged.AddListener((v) =>
            {
                sliderValue.text = v.ToString("0.00");
            });
        }

        private void UpdateSensitivity(float value)
        {
            UserSettings.mouseSensitivity = value;
        }
    }
}