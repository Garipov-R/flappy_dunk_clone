using System.Collections;
using UnityEngine;

namespace Client.Inputs
{
    public abstract class PlayerInputBase : MonoBehaviour
    {
        public bool GetButton(string name) { return GetButtonInternal(name); }

        protected virtual bool GetButtonInternal(string name) { return false; }

        public bool GetButtonDown(string name) { return GetButtonDownInternal(name); }

        protected virtual bool GetButtonDownInternal(string name) { return false; }

        public bool GetButtonUp(string name) { return GetButtonUpInternal(name); }

        protected virtual bool GetButtonUpInternal(string name) { return false; }

        public float GetAxis(string name) { return GetAxisInternal(name); }

        protected virtual float GetAxisInternal(string name) { return 0; }

        public float GetAxisRaw(string name) { return GetAxisRawInternal(name); }

        protected virtual float GetAxisRawInternal(string name) { return 0; }
    }
}