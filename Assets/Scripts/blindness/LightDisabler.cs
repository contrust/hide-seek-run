using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace blindness
{
    public class LightDisabler : MonoBehaviour
    {

        public void Start()
        {
            RenderSettings.fogDensity = 1;
            RenderSettings.fogColor = Color.black;
        }
    }
}
