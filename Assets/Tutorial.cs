using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image imageBlock;
    public List<Sprite> hunterTutorial;
    public List<Sprite> girlTutorial;
    public bool isHunterTutorial;
    public TextMeshProUGUI text;

    private int currentIndex;
    private List<Sprite> currentList;

    public int CurrentIndex => currentIndex % currentList.Count;

    // Start is called before the first frame update
    void Start()
    {
        currentList = hunterTutorial;
        imageBlock.sprite = currentList[currentIndex];
        text.text = "Показать обучение девочки";
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex == currentList.Count)
            currentIndex = 0;
        imageBlock.sprite = currentList[CurrentIndex];
    }

    public void Previous()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = currentList.Count - 1;
        imageBlock.sprite = currentList[CurrentIndex];
    }

    public void ChangeTutorial()
    {
        currentList = isHunterTutorial ? hunterTutorial : girlTutorial;
        isHunterTutorial = !isHunterTutorial;
        text.text = isHunterTutorial ? "Показать обучение охотника" : "Показать обучение девочки";
        currentIndex = 0;
        imageBlock.sprite = currentList[CurrentIndex];
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
