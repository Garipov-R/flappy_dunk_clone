using UnityEngine;

namespace Client.Inputs
{
    public class OldInputSystemInput : InputBase
    {
        public override float GetAxis(string name)
        {
            try
            {
                return UnityEngine.Input.GetAxis(name);
            }
            catch (UnityException /*e*/)
            {
                //Debug.LogError("Axis \"" + name + "\" is not setup. Please create an axis mapping within the Unity Input Manager.");
            }
            return 0;
        }

        public override float GetAxisRaw(string axisName)
        {
            try
            {
                return UnityEngine.Input.GetAxisRaw(axisName);
            }
            catch (UnityException /*e*/)
            {
                //Debug.LogError("Axis \"" + axisName + "\" is not setup. Please create an axis mapping within the Unity Input Manager.");
            }
            return 0;
        }

        public override bool GetButton(string name, ButtonAction action)
        {
            try
            {
                switch (action)
                {
                    case ButtonAction.GetButton:
                        return UnityEngine.Input.GetButton(name);
                    case ButtonAction.GetButtonDown:
                        return UnityEngine.Input.GetButtonDown(name);
                    case ButtonAction.GetButtonUp:
                        return UnityEngine.Input.GetButtonUp(name);
                }
            }
            catch (System.Exception /*e*/)
            {
                //Debug.LogError("Button \"" + name + "\" is not setup. Please create a button mapping within the Unity Input Manager.");
            }

            return false;
        }

        public override bool GetAnyButton()
        {
            return UnityEngine.Input.anyKey;
        }

        public override bool GetAnyButtonUp()
        {
            return false; // UnityEngine.Input.anyKey;
        }

        public override bool GetAnyButtonDown()
        {
            return UnityEngine.Input.anyKeyDown;
        }
    }
}
