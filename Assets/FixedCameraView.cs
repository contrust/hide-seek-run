using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

public class FixedCameraView : MonoBehaviour
{
    private Camera fixedCam;
    private Rect defaultPos = new Rect(0.1f, 0.1f, 0.35f, 0.35f);

    public void SetFixedCam(Camera cam)
    {
        fixedCam = cam;
        fixedCam.enabled = true;
        fixedCam.rect = defaultPos;
        fixedCam.depth = 150;
    }

    public void DisableFixedCam()
    {
        if (fixedCam is not null)
            fixedCam.enabled = false;
    }

}
