using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnScene : MonoBehaviour
{
    [SerializeField] private string GameplayScene;

    private void Start()
    {
        SceneManager.activeSceneChanged += DestroyThis;
    }

    private void DestroyThis(Scene current, Scene next)
    {
        Debug.Log(next.name);
        Debug.Log(GameplayScene);
        if (next.name == GameplayScene)
            Destroy(gameObject);
    }
}
