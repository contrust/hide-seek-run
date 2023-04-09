using UnityEngine;

namespace Player
{
    public class SymbolRotation : MonoBehaviour
    {
        [SerializeField] private double rotateDistance = 2;
        [SerializeField] private int rotationStep = 1;
        [SerializeField] private Transform leftRayDirection;
        [SerializeField] private Transform centerRayDirection;
        [SerializeField] private Transform rightRayDirection;
        [SerializeField] private float yAngle;
        private Vector3 transformPosition;
        public bool showRays;
        
        void Update()
        {
            UpdatePosition();
            if (showRays)
            {
                DrawRays();
            }
        }

        private void UpdatePosition()
        {
            transformPosition = transform.position;
            var mainRay = new Ray(transformPosition, transform.parent.transform.forward * -1);
            var isMainHit = Physics.Raycast(mainRay, out var mainHit);
            
            var centerRay = new Ray(transformPosition, centerRayDirection.forward);
            var isCenterHit = Physics.Raycast(centerRay, out var centerHit);
            
            var leftRay = new Ray(transformPosition, leftRayDirection.forward);
            var isLeftHit = Physics.Raycast(leftRay, out var leftHit);
            
            var rightRay = new Ray(transformPosition, rightRayDirection.forward);
            var isRightHit = Physics.Raycast(rightRay, out var rightHit);

            if (isMainHit && mainHit.distance > rotateDistance)
            {
                RotateToDefaultPosition();
            }
            else if (isCenterHit && centerHit.distance < rotateDistance)
            {
                RotateFromWall();
            }
            else if(isRightHit && rightHit.distance > rotateDistance && 
                    isLeftHit && leftHit.distance > rotateDistance)
            {
                RotateToDefaultPosition();
            }
            
            #region RotationMethods
            void RotateToDefaultPosition()
            {
                yAngle = transform.localEulerAngles.y;
                switch (yAngle % 360)
                {
                    case < 180 when yAngle > 2:
                        transform.RotateAround(transformPosition, Vector3.up, -rotationStep);
                        break;
                    case > 180 when yAngle < 358:
                        transform.RotateAround(transformPosition, Vector3.up, rotationStep);
                        break;
                }
            }

            void RotateFromWall()
            {
                if (leftHit.distance < rotateDistance && rightHit.distance > leftHit.distance)
                {
                    transform.RotateAround(transformPosition, Vector3.up, -rotationStep);
                }
                else
                {
                    transform.RotateAround(transformPosition, Vector3.up, rotationStep);
                }
            }
            #endregion
        }

        private void DrawRays()
        { 
            Debug.DrawRay(transformPosition, transform.parent.transform.forward * -1, Color.blue);
            Debug.DrawRay(transformPosition, centerRayDirection.forward, Color.yellow);
            Debug.DrawRay(transformPosition, leftRayDirection.forward, Color.red);
            Debug.DrawRay(transformPosition, rightRayDirection.forward, Color.green);
        }
    }
}
