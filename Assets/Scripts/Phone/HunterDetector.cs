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
        [SerializeField] private bool isTurnedOn;
        [SerializeField] private Transform scanStartPoint;
        [SerializeField] private Vector3 scanDirection;
        [SerializeField] private int scanAngle = 30;
        [SerializeField] private float angleToHunter;

        private float signalDelay
        {
            get
            {
                switch (currentDistance)
                {
                    case > VeryFar:
                        return float.PositiveInfinity;
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
        
        [SerializeField] private float directionMultiplier = 1;

        //distance to hunter constants
        private const float VeryFar = 20;
        private const float Far = 16;
        private const float Midrange = 10;
        private const float Close = 6;

        public void TurnOn()
        {
            isTurnedOn = true;
        }

        public void TurnOff()
        {
            isTurnedOn = false;
        }

        private void Start()
        {
            hunter = FindObjectOfType<Hunter>();
        }

        private void Update()
        {
            if(!isTurnedOn) return;
            UpdateDirection();
            UpdateDistance();
            UpdateDelay();
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

        private void UpdateDirection()
        {
            if (hunter is null)
            {
                angleToHunter = 180;
                directionMultiplier = 1;
                return;
            }
            scanDirection = scanStartPoint.forward;
            var hunterDirection = hunter.transform.position - scanStartPoint.transform.position;
            angleToHunter = Vector3.Angle(scanDirection, hunterDirection);
            if (angleToHunter < scanAngle)
            {
                directionMultiplier = 0.5f;
            }
            else
            {
                directionMultiplier = 1;
            }
        }

        private void UpdateDelay()
        {
            time += Time.deltaTime;
            if (time > signalDelay*directionMultiplier)
            {
                MakeSignal();
                time = 0;
            }
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
