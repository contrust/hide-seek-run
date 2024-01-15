using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlock : MonoBehaviour
{
    public GameObject Tutorial;
    public void OnClick()
    {
        Tutorial.SetActive(true);
    }
}
