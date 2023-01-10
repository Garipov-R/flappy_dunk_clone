using Client.EntityBehaviour;
using System;
using System.Collections;
using UnityEngine;

namespace Client.EntityBehaviour
{
    public class RingEntity : EntityBase
    {
        [SerializeField] private Vector3 _EnterDirection;
        [SerializeField] private bool _IsDanger;
        [SerializeField] private GameObject _Particle;

        public Vector3 EnterDirection { get => _EnterDirection.normalized; }
        public bool IsDanger { get => _IsDanger; set => _IsDanger = value; }


        private Action _OnCompleted;

        public Action OnCompleted { get => _OnCompleted; }


        public void Complete()
        {
            _OnCompleted?.Invoke();

            if (_Particle)
            {
                var g = Instantiate(_Particle, transform.position, Quaternion.identity);
                Destroy(g, 5f);
            }
        }

        public void Enter(Vector3 direction)
        {
            if (Check(direction) == false)
                return;

            _Entered = true;

            //RingsConfig.RingsSetuper.SpawnDestroyPrefab(transform.position, transform.rotation);

            Complete();
        }

        public bool Check(Vector3 direction)
        {
            bool enterCheck = false;

            if (Entered == false && Vector3.Dot(direction.normalized, transform.forward) > 0)
            {
                enterCheck = true;
            }

            return enterCheck;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up + _EnterDirection);
            Gizmos.DrawSphere(transform.position + transform.up + _EnterDirection, .2f);
        }
    }
}