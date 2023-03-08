using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace blindness
{
    public class LightDisabler : MonoBehaviour
    {
        public GameObject lightingContainer;
        private List<Light> _lights;

        public void Start()
        {
            _lights = lightingContainer.GetComponentsInChildren<Light>().ToList();
        }

        private void OnPreCull()
        {
            foreach (var lightSource in _lights)
            {
                if (lightSource != null)
                {
                    lightSource.enabled = false;
                }
            }
        }
    }
}
