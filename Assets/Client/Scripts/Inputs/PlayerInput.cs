using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Inputs
{
    public abstract class PlayerInput : MonoBehaviour
    {
        private Dictionary<string, float> m_ButtonDownTime;
        private Dictionary<string, float> m_ButtonUpTime;


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

        public bool AnyButton() { return AnyButtonInternal(); }

        protected virtual bool AnyButtonInternal() { return false; }
        
        public bool AnyButtonUp() { return AnyButtonUpInternal(); }

        protected virtual bool AnyButtonUpInternal() { return false; }

        public bool AnyButtonDown() { return AnyButtonDownInternal(); }

        protected virtual bool AnyButtonDownInternal() { return false; }








        public bool GetLongPress(string name, float duration)
        {
            return GetLongPress(name, duration, false);
        }

        public bool GetLongPress(string name, float duration, bool waitForRelease)
        {
            if (m_ButtonDownTime == null)
            {
                m_ButtonDownTime = new Dictionary<string, float>();
                m_ButtonUpTime = new Dictionary<string, float>();
            }

            if (GetButtonInternal(name))
            {
                if (m_ButtonDownTime.TryGetValue(name, out var downTime))
                {
                    // Only set the down time if the up time is greater than the down time. This will prevent the current time from being set every tick. 
                    var upTime = -1f;
                    m_ButtonUpTime.TryGetValue(name, out upTime);
                    if (upTime > downTime)
                    {
                        m_ButtonDownTime[name] = downTime = Time.unscaledTime;
                    }
                    // Return true as soon as the button has been pressed for the duration.
                    if (!waitForRelease)
                    {
                        return downTime + duration <= Time.unscaledTime;
                    }
                }
                else
                {
                    m_ButtonDownTime.Add(name, Time.unscaledTime);
                }
            }
            else
            {
                if (m_ButtonUpTime.TryGetValue(name, out var upTime))
                {
                    // Only set the up time if the down time is greater than the up time. This will prevent the current time from being set every tick. 
                    var downTime = -1f;
                    m_ButtonDownTime.TryGetValue(name, out downTime);
                    if (downTime > upTime)
                    {
                        m_ButtonUpTime[name] = upTime = Time.unscaledTime;
                        if (waitForRelease)
                        {
                            return downTime + duration <= Time.unscaledTime;
                        }
                    }
                }
                else
                {
                    m_ButtonUpTime.Add(name, Time.unscaledTime);
                }
            }

            return false;
        }
    }
}