using System.Collections.Generic;
using UnityEngine;

namespace Client.Inputs
{
    public class PlayerInput : PlayerInputBase
    {
        private List<InputBase> _InputList;


        private void Awake()
        {
            var newInputSystem = new NewInputSystem();

            _InputList.Add(newInputSystem);
        }

        protected override bool GetButtonDownInternal(string name)
        {
            foreach (var input in _InputList)
            {
                if (input.GetButton(name, InputBase.ButtonAction.GetButtonDown) == true)
                {
                    return input.GetButton(name, InputBase.ButtonAction.GetButtonDown);
                }
            }

            return false;
        }
    }
}