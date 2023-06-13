using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

public class FixedCameraView : MonoBehaviour
{
    private Camera fixedCam;
    private Rect defaultPos = new Rect(0, 0.5f, 0.25f, 0.25f);

    public void SetFixedCam(Camera cam)
    {
        fixedCam = cam;
        fixedCam.enabled = true;
        fixedCam.rect = defaultPos;
    }

    public void DisableFixedCam()
    {
        fixedCam.enabled = false;
    }

}
