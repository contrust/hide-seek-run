using UnityEngine;

public class BodyRotation : MonoBehaviour
{
    [SerializeField] private Vector3 bottom;
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 top;

    [SerializeField] private Transform playerCameraRoot;

    private void Update()
    {
        var angle = playerCameraRoot.localEulerAngles.x;
        if (angle > 270)
            angle -= 360;
        Rotate(-angle / 90);
    }

    public void Rotate(float value)
    {
        if (value < 0)
        {
            value *= -1;
            transform.localEulerAngles = Vector3.Lerp(center, bottom, value);
        }
        else
        {
            transform.localEulerAngles = Vector3.Lerp(center, top, value);
        }
    }
}
