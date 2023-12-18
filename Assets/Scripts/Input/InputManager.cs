using UnityEngine;

namespace AsteroidsGame.Input
{
    public class InputManager : IInputManager
    {
        public Vector3 GetAimingPosition()
        {
            return UnityEngine.Input.mousePosition;
        }

        public float GetThrottle()
        {
            return UnityEngine.Input.GetKey(KeyCode.W) ? 1f : 0f;
        }

        public bool GetFireButton()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public bool GetWarpDriveButton()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.LeftShift);
        }
    }
}