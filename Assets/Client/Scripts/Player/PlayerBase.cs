using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Utilities;


namespace Client.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBase : MonoBehaviour
    {
        public enum UpdateMethod { Update, FixedUpdate }
        public const float c_ColliderSpace = 0.01f;

        [SerializeField] protected UpdateMethod _UpdateMethod = UpdateMethod.FixedUpdate;
        [SerializeField] protected float _GravityMagnitude = 6f;
        [SerializeField] protected Vector3 _Speed = new Vector3(0, 0, 2.7f);
        [SerializeField] protected bool _IsFreezed;
        [SerializeField] protected Vector3 _GravityDirection = new Vector3(0, -1, 0); 
        [SerializeField] protected LayerMask _SolidLayerMask = ~0;
        [SerializeField][Range(0, 1)] private float _ExternalForceDamping = 0.01f;
        [SerializeField] protected float _TimeScale = 1f;

        public bool IsFreezed 
        { 
            get => _IsFreezed;
            set 
            {
                if (_Rigidbody != null)
                {
                    _Rigidbody.isKinematic = value;
                }

                _GravityAmount = 0;
                _ExternalForce = Vector3.zero;

                _IsFreezed = value;
            } 
        }
        public float TimeScale { get => _TimeScale; set => _TimeScale = value; }
        public Vector3 GravityDirection { get => _GravityDirection; set => _GravityDirection = value; }
        public float GravityMagnitude { get => _GravityMagnitude; set => _GravityMagnitude = value; }
        public Vector3 Speed { get => _Speed; set => _Speed = value; }

        protected Rigidbody _Rigidbody;
        protected Vector3 _MoveDirection;
        protected float _GravityAmount;
        protected Collider[] _Colliders;
        protected Vector3 _ExternalForce;
        protected RaycastHit _RaycastHit;
        protected RaycastHit _BlankRaycastHit = new RaycastHit();
        protected bool _OnGrounded;
        protected Vector3 _InputVector = Vector3.zero;
        protected bool _IsAlive;
        protected Vector3 _Velocity;
        protected Vector3 _PrevPosition;

        public float GravityAmount { get => _GravityAmount; set => _GravityAmount = value; }
        public bool IsAlive { get => _IsAlive; set => _IsAlive = value; }
        public Vector3 ExternalForce { get => _ExternalForce; set => _ExternalForce = value; }
        public Vector3 MoveDirection { get => _MoveDirection;  }
        public Rigidbody Rigidbody { get => _Rigidbody; }
        public Vector3 Velocity { get => _Velocity; set => _Velocity = value; }
        public Vector3 PrevPosition { get => _PrevPosition; set => _PrevPosition = value; }


        protected void Awake()
        {
            _Rigidbody = GetComponent<Rigidbody>();

            var colliders = GetComponentsInChildren<Collider>();
            _Colliders = new Collider[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                _Colliders[i] = colliders[i];
            }

            _IsAlive = true;

            _PrevPosition = transform.position;
        }

        protected void Update()
        {
            if (_UpdateMethod != UpdateMethod.Update)
                return;

            UpdateProccess();
        }

        protected void FixedUpdate()
        {
            if (_UpdateMethod != UpdateMethod.FixedUpdate)
                return;

            UpdateProccess();
        }

        public void Move(Vector3 moveInput)
        {
            _InputVector = moveInput;
        }

        protected virtual void UpdateProccess()
        {
            UpdatePosition();

            ApplyPosition();

            /*if (CheckCollision() == true)
            {
                _InputVector = Vector3.zero;
            }
            else
            {
                _InputVector = _KeepInputVector;
            }*/
        }

        protected void UpdatePosition()
        {
            if (_IsFreezed == true)
                return;

            var deltaTime = (_TimeScale * _TimeScale) * Time.timeScale * TimeUtility.FramerateDeltaTime;
            var externalForce = _ExternalForce * deltaTime;
            var accelerate = new Vector3(_InputVector.normalized.x * _Speed.x, 0, _InputVector.normalized.z * _Speed.z) * deltaTime;
            var gravity = _GravityDirection * _GravityAmount * deltaTime;
            _MoveDirection = externalForce + accelerate - gravity;

            //Debug.Log(_MoveDirection);

            // m_MotorThrottle /= (1 + ((m_Grounded ? m_MotorDamping : m_MotorAirborneDamping) * m_TimeScale * Time.timeScale));

            GravityProccess();

            UpdateExternalForce();
        }

        protected virtual void ApplyPosition()
        {
            //_Rigidbody.velocity = _MoveDirection;

            _Velocity = _Rigidbody.velocity;
            //_Velocity = (transform.position - _PrevPosition) / (_TimeScale * Time.deltaTime);
            _PrevPosition = transform.position;
        }

        protected void GravityProccess()
        {
            _OnGrounded = CheckGround();

            if (_OnGrounded == true)
            {
                var distance = (_RaycastHit.point - (transform.position + _GravityDirection * _RaycastHit.distance)).magnitude;

                if (distance > .0001f) _GravityAmount += (_GravityMagnitude * -0.02f * distance) / Time.timeScale;
                else _GravityAmount = 0;

                return;
            }

            _GravityAmount += (_GravityMagnitude * -0.1f) / Time.timeScale;
            //_GravityAmount += (-_GravityMagnitude * Time.deltaTime * Time.deltaTime / 2f) / Time.timeScale;
            //var deltaTime = _TimeScale * TimeUtility.FramerateDeltaTime;
            //_GravityAmount += -(_GravityMagnitude * (deltaTime * deltaTime) / 2f);
        }

        protected bool CheckGround()
        {
            var result = false;

            var verticalOffset = 1f;

            /*if (CheckCollision() == true)
            {
                if (SingleCast(_GravityDirection * (verticalOffset + c_ColliderSpace), -_GravityDirection * (verticalOffset - c_ColliderSpace)) == true)
                {
                    result = true;

                    if (_OnGrounded == false)
                    {
                        _ExternalForce.y = 0;
                        _GravityAmount = 0;
                    }
                }
            }*/

            if (SingleCast(_GravityDirection * (verticalOffset + c_ColliderSpace), Vector3.zero))
            {
                if (_RaycastHit.distance <= (verticalOffset - c_ColliderSpace) / 2)
                {
                    result = true;

                    if (_OnGrounded == false)
                    {
                        _ExternalForce.y = 0;
                        _GravityAmount = 0;
                    }
                }
            }

            /*if (SingleCastWithColliderSpacing(_GravityDirection * (verticalOffset + c_ColliderSpace), -_GravityDirection * (verticalOffset - c_ColliderSpace)) == true)
            {
                result = true;

                if (_OnGrounded == false)
                {
                    _ExternalForce.y = 0;
                    _GravityAmount = 0;
                }
            }*/


            /*if (RayCast(_GravityDirection * (verticalOffset + c_ColliderSpace), Vector3.zero))
            {
                result = true;

                //_ExternalForce.y = _ExternalForce.y > 0 ? 0 : _ExternalForce.y;
            }*/


            return result;
        }

        protected void UpdateExternalForce()
        {
            var deltaTime = _TimeScale * TimeUtility.FramerateDeltaTime;
            //_ExternalForce /= (1 + (_Grounded ? _ExternalForceDamping : _ExternalForceAirDamping) * deltaTime);
            //_ExternalForce /= (1 + (_ExternalForceDamping) * deltaTime);
            _ExternalForce /= (1 + (_ExternalForceDamping) * deltaTime);
        }

        public virtual void AddForce(Vector3 force)
        {
            _ExternalForce += force;
        }

        public void SetForce(Vector3 force)
        {
            _ExternalForce = force;
        }

        public void AddForceRigidbody(Vector3 force)
        {
            _Rigidbody.constraints = RigidbodyConstraints.None;
            _Rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        public void AddForceTorque(Vector3 force)
        {
            _Rigidbody.AddTorque(force, ForceMode.Impulse);
        }


        #region Physics

        protected bool CheckCollision()
        {
            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius + c_ColliderSpace;

                    bool checkSphere = Physics.CheckSphere(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, _SolidLayerMask, QueryTriggerInteraction.Ignore);
                    //Debug.Log($"check sphere {checkSphere}");

                    return checkSphere;
                }
            }

            return false;
        }

        protected bool SingleCast(Vector3 direction, Vector3 offset)
        {
            return SingleCast(direction, offset, QueryTriggerInteraction.Ignore);
        }

        protected bool SingleCast(Vector3 direction, Vector3 offset, QueryTriggerInteraction queryTriggerInteraction)
        {
            return SingleCast(direction, offset, out _RaycastHit, queryTriggerInteraction);
        }

        protected bool SingleCast(Vector3 direction, Vector3 offset, out RaycastHit raycastHit, QueryTriggerInteraction queryTriggerInteraction)
        {
            var result = false;
            raycastHit = _BlankRaycastHit;

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius - c_ColliderSpace;

                    bool sphereCast = Physics.SphereCast(
                        sphereCollider.transform.TransformPoint(sphereCollider.center) + offset,
                        radius,
                        direction.normalized,
                        out raycastHit,
                        direction.magnitude,
                        _SolidLayerMask,
                        queryTriggerInteraction
                    );

                    result = sphereCast;
                }
            }

            raycastHit = result == false ? _BlankRaycastHit : raycastHit;

            return result;
        }
        
        protected bool SingleCastWithColliderSpacing(Vector3 direction, Vector3 offset, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var result = false;

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius + c_ColliderSpace;

                    bool sphereCast = Physics.SphereCast(
                        sphereCollider.transform.TransformPoint(sphereCollider.center) + offset,
                        radius,
                        direction.normalized,
                        out _RaycastHit,
                        direction.magnitude,
                        _SolidLayerMask,
                        queryTriggerInteraction
                    );

                    result = sphereCast;
                }
            }

            _RaycastHit = result == false ? _BlankRaycastHit : _RaycastHit;

            return result;
        }

        protected bool RayCast(Vector3 direction, Vector3 offset)
        {
            var result = false;

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;

                    bool rayCast = Physics.Raycast(
                        sphereCollider.transform.TransformPoint(sphereCollider.center) + offset,
                        direction.normalized,
                        out _RaycastHit,
                        direction.magnitude,
                        _SolidLayerMask,
                        QueryTriggerInteraction.Ignore
                    );

                    result = rayCast;
                }
            }

            return result;
        }

        protected int OverlapCount()
        {
            var result = 0;

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius + c_ColliderSpace;

                    var overlapColliders = new Collider[10];
                    int overlapSphereNonAlloc = Physics.OverlapSphereNonAlloc(
                        sphereCollider.transform.TransformPoint(sphereCollider.center), 
                        radius, 
                        overlapColliders, 
                        _SolidLayerMask, 
                        QueryTriggerInteraction.Ignore
                    );

                    Debug.Log($"overlapSphereNonAlloc {overlapSphereNonAlloc}");

                    result = overlapSphereNonAlloc;
                }
            }

            return result;
        }

        #endregion

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var verticalOffset = 1;
            Gizmos.DrawLine(transform.position, transform.position + (_GravityDirection * (verticalOffset + c_ColliderSpace)));

            Gizmos.DrawWireSphere(transform.position + _GravityDirection * (verticalOffset + c_ColliderSpace), 1f - c_ColliderSpace);

            if (_RaycastHit.transform != null)
            {
                Gizmos.DrawSphere(_RaycastHit.point, .1f);
                Gizmos.DrawSphere(transform.position + _GravityDirection * _RaycastHit.distance, .1f);
                Gizmos.DrawLine(_RaycastHit.point, _RaycastHit.point + _RaycastHit.normal * _RaycastHit.distance);
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_RaycastHit.point, transform.position);
            }
        }
    }
}

