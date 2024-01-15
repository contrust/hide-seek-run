using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image imageBlock;
    public List<Sprite> hunterTutorial;
    public List<Sprite> girlTutorial;
    public bool isHunterTutorial;

    private int currentIndex;
    private List<Sprite> currentList;

    public int CurrentIndex => currentIndex % currentList.Count;

    // Start is called before the first frame update
    void Start()
    {
        currentList = hunterTutorial;
        imageBlock.sprite = currentList[currentIndex];
    }

    public void Next()
    {
        currentIndex++;
        imageBlock.sprite = currentList[CurrentIndex];
    }

    public void Previous()
    {
        currentIndex--;
        imageBlock.sprite = currentList[CurrentIndex];
    }

    public void ChangeTutorial()
    {
        currentList = isHunterTutorial ? hunterTutorial : girlTutorial;
        isHunterTutorial = !isHunterTutorial;
        currentIndex = 0;
        imageBlock.sprite = currentList[CurrentIndex];
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
