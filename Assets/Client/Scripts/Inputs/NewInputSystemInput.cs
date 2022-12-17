using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client.Inputs
{
    public class NewInputSystemInput : InputBase
    {
        private InputControls _InputControls;


        public override void Initialize(PlayerInput playerInput)
        {
            base.Initialize(playerInput);

            _InputControls = new InputControls();
            _InputControls.Enable();
        }

        public override float GetAxis(string name)
        {
            try
            {
                InputAction inputAction = _InputControls.FindAction(name);

                return inputAction.ReadValue<Vector2>().normalized.x;
            }
            catch (Exception)
            {

            }

            return 0;
        }

        public override Vector2 GetVector(string name)
        {
            try
            {
                InputAction inputAction = _InputControls.FindAction(name);

                return inputAction.ReadValue<Vector2>().normalized;
            }
            catch (Exception)
            {

            }

            return Vector2.zero;
        }

        public override float GetAxisRaw(string axisName)
        {
            return 0;
        }

        public override bool GetButton(string name, ButtonAction action)
        {
            try
            {
                InputAction inputAction = _InputControls.FindAction(name);

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
                Debug.LogError($"please setup: ' {name} ' name");
            }

            return false;
        }

        public override bool GetAnyButton()
        {
            try
            {
                var gamepad = false;
                if (UnityEngine.InputSystem.Gamepad.current != null)
                {
                    foreach (UnityEngine.InputSystem.LowLevel.GamepadButton c in System.Enum.GetValues(typeof(UnityEngine.InputSystem.LowLevel.GamepadButton)))
                    {
                        if (UnityEngine.InputSystem.Gamepad.current[c].isPressed == true)
                        {
                            gamepad = UnityEngine.InputSystem.Gamepad.current[c].isPressed;
                            break;
                        }
                    }
                }

                var keyboard = false;
                if (UnityEngine.InputSystem.Keyboard.current != null)
                {
                    keyboard = UnityEngine.InputSystem.Keyboard.current.anyKey.isPressed;
                }

                return keyboard || gamepad;
            }
            catch (System.Exception /**/)
            {
                Debug.LogError("Input sucks");
            }

            return false;
        }

        public override bool GetAnyButtonUp()
        {
            try
            {
                var gamepad = false;
                if (UnityEngine.InputSystem.Gamepad.current != null)
                {
                    foreach (UnityEngine.InputSystem.LowLevel.GamepadButton c in System.Enum.GetValues(typeof(UnityEngine.InputSystem.LowLevel.GamepadButton)))
                    {
                        if (UnityEngine.InputSystem.Gamepad.current[c].wasReleasedThisFrame == true)
                        {
                            gamepad = UnityEngine.InputSystem.Gamepad.current[c].wasReleasedThisFrame;
                            break;
                        }
                    }
                }

                var keyboard = false;
                if (UnityEngine.InputSystem.Keyboard.current != null)
                {
                    keyboard = UnityEngine.InputSystem.Keyboard.current.anyKey.wasReleasedThisFrame;
                }

                return keyboard || gamepad;
            }
            catch (System.Exception /**/)
            {
                Debug.LogError("Input sucks");
            }

            return false;
        }

        public override bool GetAnyButtonDown()
        {
            try
            {
                var gamepad = false;
                if (UnityEngine.InputSystem.Gamepad.current != null)
                {
                    foreach (UnityEngine.InputSystem.LowLevel.GamepadButton c in System.Enum.GetValues(typeof(UnityEngine.InputSystem.LowLevel.GamepadButton)))
                    {
                        if (UnityEngine.InputSystem.Gamepad.current[c].wasPressedThisFrame == true)
                        {
                            gamepad = UnityEngine.InputSystem.Gamepad.current[c].wasPressedThisFrame;
                            break;
                        }
                    }
                }

                var keyboard = false;
                if (UnityEngine.InputSystem.Keyboard.current != null)
                {
                    keyboard = UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame;
                }

                return keyboard || gamepad;
            }
            catch (System.Exception /**/)
            {
                Debug.LogError("Input sucks");
            }

            return false;
        }
    }
}