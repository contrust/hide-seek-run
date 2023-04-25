using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    private static DontDestroyMusic instance;
    void Awake(){
        DontDestroyOnLoad (gameObject);
         
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
