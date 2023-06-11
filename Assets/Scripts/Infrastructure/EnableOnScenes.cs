using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableOnScenes: MonoBehaviour
{
    [SerializeField] private List<string> scenes;

    private void Start()
    {
        SceneManager.activeSceneChanged += EnableThis;
    }

    private void EnableThis(Scene current, Scene next)
    {
        Debug.Log(next.name);
        Debug.Log(scenes);
        if (scenes.Contains(next.name))
            gameObject.SetActive(true);
    }
}