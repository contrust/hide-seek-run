using UnityEngine;

namespace Player
{
    public class SymbolRotation : MonoBehaviour
    {
        [SerializeField] private double rotateDistance = 2;
        [SerializeField]private double blindZone;
        [SerializeField] private int rotateStep = 1;
        [SerializeField] private Transform mainRayDirection;
        [SerializeField] private Transform leftRayDirection;
        [SerializeField] private Transform rightRayDirection;
        [SerializeField] private float yAngle;
        private Vector3 transformPosition;
        public bool showRays;

        void Start()
        {
            blindZone = rotateDistance / 2;
        }

        void Update()
        {
            transformPosition = transform.position;
            var mainRay = new Ray(transformPosition, transform.parent.transform.forward * -1);
            var isMainHit = Physics.Raycast(mainRay, out var mainHit);
            
            var centerRay = new Ray(transformPosition, mainRayDirection.forward);
            var isCenterHit = Physics.Raycast(centerRay, out var centerHit);
            
            var leftRay = new Ray(transformPosition, leftRayDirection.forward);
            var isLeftHit = Physics.Raycast(leftRay, out var leftHit);
            
            var rightRay = new Ray(transformPosition, rightRayDirection.forward);
            var isRightHit = Physics.Raycast(rightRay, out var rightHit);
            if (showRays)
            {
                Debug.DrawRay(transformPosition, transform.parent.transform.forward * -1, Color.blue);
                Debug.DrawRay(transformPosition, mainRayDirection.forward, Color.yellow);
                Debug.DrawRay(transformPosition, leftRayDirection.forward, Color.red);
                Debug.DrawRay(transformPosition, rightRayDirection.forward, Color.green);
            }
            if (mainHit.distance > rotateDistance)
            {
                yAngle = transform.localEulerAngles.y;
                switch (yAngle % 360)
                {
                    case < 180 when yAngle > 2:
                        transform.RotateAround(transformPosition, Vector3.up, -rotateStep);
                        break;
                    case > 180 when yAngle < 358:
                        transform.RotateAround(transformPosition, Vector3.up, rotateStep);
                        break;
                }
            }
            
            else if (centerHit.distance<rotateDistance)
            {
                if (isLeftHit && leftHit.distance < rotateDistance && rightHit.distance > rotateDistance)
                {
                    transform.RotateAround(transformPosition, Vector3.up, -rotateStep);
                }
                else
                {
                    transform.RotateAround(transformPosition, Vector3.up, rotateStep);
                }
            }
            else
            {
                yAngle = transform.localEulerAngles.y;
                switch (yAngle % 360)
                {
                    case < 180 when yAngle > 2 && leftHit.distance-rotateDistance> blindZone:
                        transform.RotateAround(transformPosition, Vector3.up, -rotateStep);
                        break;
                    case > 180 when yAngle < 358 && rightHit.distance - rotateDistance > blindZone:
                        transform.RotateAround(transformPosition, Vector3.up, rotateStep);
                        break;
                }
            }
        }
    }
}
