using System.Collections;
using UnityEngine;

namespace Client.Rings
{
    public class DestroyRingCollider : MonoBehaviour
    {
        [SerializeField] private Vector3 _EnterDirection;
        [SerializeField] private GameObject _Parent;

        public GameObject Parent 
        { 
            get
            {
                if (_Parent == null)
                {
                    _Parent = transform.parent.gameObject;
                }

                return _Parent;
            } 
        }
        public Vector3 EnterDirection { get => _EnterDirection.normalized; }
        public bool Entered { get => _Entered; set => _Entered = value; }

        private Ring _Ring;
        private bool _Entered;


        private void Awake()
        {
            _Ring = Parent.GetComponent<Ring>();
        }

        public void Enter(Vector3 direction)
        {
            if (_Entered == true)
                return;

            if (direction != _EnterDirection)
                return;

            _Entered = true;

            RingsConfig.RingsSetuper.SpawnDestroyPrefab(_Parent.transform.position, _Parent.transform.rotation);

            _Ring.Complete();
            //Destroy(_Parent);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + -_EnterDirection);
            Gizmos.DrawSphere(transform.position + -_EnterDirection, .1f);
        }
    }
}