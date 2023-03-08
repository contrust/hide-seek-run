using UnityEngine;

namespace blindness
{
    public class PlayerCamera : MonoBehaviour
    {

        public float sensX;
        public float sensY;

        public Transform orientation;

        private float xRotation;
        private float yRotation;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
            var mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
