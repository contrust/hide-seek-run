using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableOnScenes : MonoBehaviour
{
    [SerializeField] private List<string> scenes;

    private void Start()
    {
        SceneManager.activeSceneChanged += DisableThis;
    }

    private void DisableThis(Scene current, Scene next)
    {
        Debug.Log(next.name);
        Debug.Log(scenes);
        if (scenes.Contains(next.name))
            gameObject.SetActive(false);
    }
}
