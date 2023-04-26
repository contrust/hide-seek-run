using System;
using System.Collections;
using UnityEngine;

namespace Phone
{
    public class HunterDetector: MonoBehaviour
    {
        private Hunter hunter;
        [SerializeField] private float currentDistance = float.MaxValue;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip signal;
        private float time;

        [SerializeField] private MeshRenderer detector;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color activeColor;

        private float signalDelay
        {
            get
            {
                switch (currentDistance)
                {
                    case > VeryFar:
                        return 3;
                    case < VeryFar and > Far:
                        return 2;
                    case < Far and > Midrange:
                        return 1;
                    case < Midrange and > Close:
                        return 0.5f;
                    case < Close:
                        return 0.3f;
                    default:
                        return float.MaxValue;
                }
            }
        }

        //distance to hunter constants
        private const float VeryFar = 20;
        private const float Far = 16;
        private const float Midrange = 10;
        private const float Close = 6;

        private void Start()
        {
            hunter = FindObjectOfType<Hunter>();
        }

        private void Update()
        {
            UpdateDistance();
            time += Time.deltaTime;
            if (time > signalDelay)
            {
                MakeSignal();
                time = 0;
            }
        }

        private void UpdateDistance()
        {
            if (hunter is null)
            {
                currentDistance = float.MaxValue;
                return;
            }
            currentDistance = Vector3.Distance(this.transform.position, hunter.transform.position);
        }

        private IEnumerator ChangeColor()
        {
            detector.material.color = activeColor;
            yield return new WaitForSeconds(0.2f);
            detector.material.color = defaultColor;
        }

        private void MakeSignal()
        {
            audioSource.PlayOneShot(signal);
            StartCoroutine(ChangeColor());
        }
    }
}
