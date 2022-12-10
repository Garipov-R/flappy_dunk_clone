using System.Collections;
using UnityEngine;

namespace Client.Inputs
{
    public abstract class InputBase
    {
        //public enum ButtonAction { GetButton, GetButtonDown, GetButtonUp }

        protected PlayerInput m_PlayerInput;


        public virtual void Initialize(PlayerInput playerInput)
        {
            m_PlayerInput = playerInput;
        }

        public abstract bool GetButton(string name, ButtonAction action);

        public abstract float GetAxis(string name);

        public abstract float GetAxisRaw(string axisName);

        public abstract bool GetAnyButton();

        public abstract bool GetAnyButtonUp();

        public abstract bool GetAnyButtonDown();
    }
}