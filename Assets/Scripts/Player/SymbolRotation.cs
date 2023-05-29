using UnityEngine;

namespace Player
{
    public class SymbolRotation : MonoBehaviour
    {
        [SerializeField] private float rotateDistance = 2;
        [SerializeField] private int rotationStep = 1;
        [SerializeField] private Transform leftRayDirection;
        [SerializeField] private Transform centerRayDirection;
        [SerializeField] private Transform rightRayDirection;
        [SerializeField] private float angle;
        [SerializeField] private LayerMask ignoreLayers; 
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
            var mainRay = new Ray(transformPosition, transform.parent.transform.forward);
            var isMainHit = Physics.Raycast(mainRay, out var mainHit, rotateDistance, ~ignoreLayers);
            
            var centerRay = new Ray(transformPosition, centerRayDirection.forward);
            var isCenterHit = Physics.Raycast(centerRay, out var centerHit, rotateDistance, ~ignoreLayers);
            
            var leftRay = new Ray(transformPosition, leftRayDirection.forward);
            var isLeftHit = Physics.Raycast(leftRay, out var leftHit, rotateDistance, ~ignoreLayers);
            
            var rightRay = new Ray(transformPosition, rightRayDirection.forward);
            var isRightHit = Physics.Raycast(rightRay, out var rightHit, rotateDistance, ~ignoreLayers);

            if (!isMainHit)
            {
                RotateToDefaultPosition();
            }
            else if (isCenterHit)
            {
                RotateFromWall();
            }
            else if(!isRightHit && !isLeftHit)
            {
                RotateToDefaultPosition();
            }
            
            #region RotationMethods
            void RotateToDefaultPosition()
            {
                angle = Vector3.Angle(mainRay.direction, centerRay.direction);
                switch (angle % 360)
                {
                    case < 180 when angle > 2:
                        transform.RotateAround(transformPosition, transform.up, -rotationStep);
                        break;
                    case > 180 when angle < 358:
                        transform.RotateAround(transformPosition, transform.up, rotationStep);
                        break;
                }
            }

            void RotateFromWall()
            {
                if (leftHit.distance < rotateDistance && !isRightHit)
                {
                    transform.RotateAround(transformPosition, transform.up, -rotationStep);
                }
                else
                {
                    transform.RotateAround(transformPosition, transform.up, rotationStep);
                }
            }
            #endregion
        }

        private void DrawRays()
        { 
            Debug.DrawRay(transformPosition, transform.parent.transform.forward, Color.blue); //default position
            Debug.DrawRay(transformPosition, centerRayDirection.forward, Color.yellow);       //center ray
            Debug.DrawRay(transformPosition, leftRayDirection.forward, Color.red);            //left ray
            Debug.DrawRay(transformPosition, rightRayDirection.forward, Color.green);         //right ray
            Debug.DrawRay(transformPosition, transform.up, Color.magenta);                    //rotation axis
        }
    }
}
