using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValue;
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            sliderValue.text = v.ToString("0.00");
        });
    }
}
