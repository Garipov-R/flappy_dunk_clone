using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client.Inputs
{
    public class NewInputSystem : InputBase
    {
        private InputControls _InputControls;


        public override void Initialize(PlayerInputBase playerInput)
        {
            base.Initialize(playerInput);

            _InputControls = new InputControls();
        }

        public override float GetAxis(string name)
        {
            return 0;
        }

        public override float GetAxisRaw(string axisName)
        {
            return 0;
        }

        public override bool GetButton(string name, ButtonAction action)
        {
            InputAction inputAction = _InputControls.FindAction(name);

            if (inputAction == null)
                return false;

            try
            {
                switch (action)
                {
                    case ButtonAction.GetButton:
                        //_Game.TextDebug.StaticLog($"Button");
                        return inputAction.IsPressed();
                    case ButtonAction.GetButtonDown:
                        //_Game.TextDebug.StaticLog($"Button down");
                        return inputAction.WasPressedThisFrame();
                    case ButtonAction.GetButtonUp:
                        //_Game.TextDebug.StaticLog($"Button up");
                        return inputAction.WasReleasedThisFrame();
                }
            }
            catch (System.Exception /**/)
            {
                Debug.LogError("Input sucks");
            }

            return false;
        }
    }
}