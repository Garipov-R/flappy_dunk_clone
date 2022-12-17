
using UnityEngine;

namespace Client.Inputs
{
    public static class Input
    {
        public static PlayerInput PlayerInput { get; set; }


        public static bool Pressed(string name)
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.GetButtonDown(name);
        }

        public static bool Down(string name)
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.GetButton(name);
        }

        public static bool Released(string name)
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.GetButtonUp(name);
        }

        public static bool AnyKeyPressed()
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.AnyButtonDown();
        }

        public static bool AnyKeyReleased()
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.AnyButtonUp();
        }

        public static bool AnyKeyDown()
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.AnyButton();
        }

        public static bool LongPress(string name, float duration, bool waitForRelease)
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.GetLongPress(name, duration, waitForRelease);
        }

        public static bool LongPress(string name, float duration)
        {
            if (PlayerInput == null)
                return false;

            return PlayerInput.GetLongPress(name, duration);
        }

        public static Vector2 GetVector(string name)
        {
            if (PlayerInput == null)
                return Vector2.zero;

            return PlayerInput.GetVector(name);
        }
    }
}
