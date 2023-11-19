using System.Collections;
using HUD.Effects;
using UnityEngine;
using UnityEngine.UI;

public class SlapEffect : HUDEffect
{
    [SerializeField] private Image slap;
    [SerializeField] private float duration;
    [SerializeField] private float firstPartCoef;
    private float secondPartCoef;
    [SerializeField] private float scale;
    
        
    public override void Show()
    {
        secondPartCoef = 1 - firstPartCoef;
        StartCoroutine(ShowSlapEffect());
    }
        
    private IEnumerator ShowSlapEffect()
    {
        var defaultSize = slap.rectTransform.sizeDelta;
        var sizeStep = defaultSize * scale;
        var firstPart = duration * firstPartCoef;
        var secondPart = duration * secondPartCoef;
        var time = 0f;
        slap.gameObject.SetActive(true);
        while (time < firstPart)
        {
            slap.rectTransform.sizeDelta += sizeStep;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        time = 0;
        while (time < secondPart)
        {
            slap.rectTransform.sizeDelta -= 2 * sizeStep;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        slap.rectTransform.sizeDelta = defaultSize;
        slap.gameObject.SetActive(false);
    }
}
