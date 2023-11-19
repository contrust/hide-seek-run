using UnityEngine;

namespace Phone
{
    public class CameraPriority: MonoBehaviour
    {
        private void Start()
        {
            Camera.main.depth = 100;
        }
    }
}