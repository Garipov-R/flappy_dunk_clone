using System.Collections.Generic;
using UnityEngine;

namespace Client.Inputs
{
    public class UnityInput : PlayerInput
    {
        private List<InputBase> _InputList = new List<InputBase>();


        private void Awake()
        {
            var newInputSystem = new NewInputSystemInput();
            var oldNewInputSystem = new OldInputSystemInput();

            _InputList.Add(newInputSystem);
            _InputList.Add(oldNewInputSystem);

            foreach (var input in _InputList)
            {
                input.Initialize(this);
            }

            Input.PlayerInput = this;
        }

        protected override bool GetButtonInternal(string name)
        {
            foreach (var input in _InputList)
            {
                if (input.GetButton(name, ButtonAction.GetButton) == true)
                {
                    return input.GetButton(name, ButtonAction.GetButton);
                }
            }

            return false;
        }

        protected override bool GetButtonDownInternal(string name)
        {
            foreach (var input in _InputList)
            {
                if (input.GetButton(name, ButtonAction.GetButtonDown) == true)
                {
                    return input.GetButton(name, ButtonAction.GetButtonDown);
                }
            }

            return false;
        }

        protected override bool GetButtonUpInternal(string name)
        {
            foreach (var input in _InputList)
            {
                if (input.GetButton(name, ButtonAction.GetButtonUp) == true)
                {
                    return input.GetButton(name, ButtonAction.GetButtonUp);
                }
            }

            return false;
        }

        protected override bool AnyButtonInternal()
        {
            foreach (var input in _InputList)
            {
                if (input.GetAnyButton() == true)
                {
                    return input.GetAnyButton();
                }
            }

            return false;
        }

        protected override bool AnyButtonUpInternal()
        {
            foreach (var input in _InputList)
            {
                if (input.GetAnyButtonUp() == true)
                {
                    return input.GetAnyButtonUp();
                }
            }

            return false;
        }

        protected override bool AnyButtonDownInternal()
        {
            foreach (var input in _InputList)
            {
                if (input.GetAnyButtonDown() == true)
                {
                    return input.GetAnyButtonDown();
                }
            }

            return false;
        }

        protected override Vector2 GetVectorInternal(string name)
        {
            foreach (var input in _InputList)
            {
                if (input.GetVector(name).magnitude > 0)
                {
                    return input.GetVector(name);
                }
            }

            return Vector2.zero;
        }
    }
}