using System;
using System.Collections;
using UnityEngine;

namespace Client.EntityBehaviour
{
    public class EntityBase : MonoBehaviour
    {
        public Action OnDestroyed;

        protected bool _Entered;

        public bool Entered { get => _Entered; set => _Entered = value; }


        protected virtual void OnDestroy()
        {
            OnDestroyed?.Invoke();
            OnDestroyed = null;
        }
    }
}