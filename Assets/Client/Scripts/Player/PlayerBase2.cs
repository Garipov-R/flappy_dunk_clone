using Client.Utilities;
using System.Collections;
using UnityEngine;

namespace Client.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBase2 : MonoBehaviour
    {
        public const float c_ColliderSpace = 0.1f;

        enum UpdateMethod { Update, FixedUpdate }

        [SerializeField] private UpdateMethod _UpdateMethod;
        [SerializeField] private Vector3 _InputVector = new Vector3(0, 0, 1f);
        //[SerializeField] private float _Speed = 5f;
        [SerializeField] private Vector3 _Speed = new Vector3(0, 0, 3f);
        [SerializeField] private Vector3 _GravityDirection = Vector3.down;
        [SerializeField] private float _GravityMagnitude = 5.22f;
        [SerializeField] private bool _IsGravity = true;
        [SerializeField] private float _JumpForce = 5f;
        [SerializeField] private bool _IsFreezed = true;
        [SerializeField][Range(0, 1)] private float _ExternalForceDamping = 1f;
        [SerializeField] private LayerMask _SolidLayerMask = ~0;
        [SerializeField] private bool _OnGrounded;

        public Vector3 MoveDirection { get => _MoveDirection; set => _MoveDirection = value; }
        public bool IsFreezed { get => _IsFreezed; set => _IsFreezed = value; }
        public float JumpForce { get => _JumpForce; set => _JumpForce = value; }

        private Vector3 _ExternalForce;
        private float _GravityAmount;
        private Vector3 _KeepMoveDirection;
        private Collider[] _Colliders;
        private Vector3 _MoveDirection = Vector3.forward;
        private RaycastHit _RaycastHit;

        public float GravityAmount
        {
            get => _GravityAmount;
            set
            {
                if (value <= 0)
                {
                    _GravityAmount = 0;
                }

                _GravityAmount = value;
            }
        }

        private Rigidbody _Rigidbody;



        #region Unity

        protected virtual void Awake()
        {
            _Rigidbody = GetComponent<Rigidbody>();
            _KeepMoveDirection = _InputVector;

            var colliders = GetComponentsInChildren<Collider>();
            _Colliders = new Collider[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                _Colliders[i] = colliders[i];
            }
        }

        protected virtual void FixedUpdate()
        {
            if (_UpdateMethod == UpdateMethod.FixedUpdate)

                UpdateProccess();
        }

        protected virtual void Update()
        {
            if (_UpdateMethod == UpdateMethod.Update)

                UpdateProccess();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Player base collision {collision.gameObject.name}");

            /*_GroundTransform = collision.gameObject.transform;
            _Rigidbody.constraints = RigidbodyConstraints.None;
            _Rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            _KeepMoveDirection = _MoveDirection;
            _MoveDirection = Vector3.zero;*/
        }

        #endregion


        protected virtual void UpdateProccess()
        {
            UpdatePosition();

            if (_OnGrounded == true)
            {
                _Rigidbody.constraints = RigidbodyConstraints.None;
                _Rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                _InputVector = Vector3.zero;
            }
            else
            {
                _Rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                _InputVector = _KeepMoveDirection;
            }
        }

        private void UpdatePosition()
        {
            if (_IsFreezed == true)
                return;

            var deltaTime = Time.timeScale * TimeUtility.FramerateDeltaTime;
            var externalForce = _ExternalForce * deltaTime;
            var accelerate = new Vector3(_InputVector.normalized.x * _Speed.x, 0, _InputVector.normalized.z * _Speed.z) * 0.01f * deltaTime;
            var gravity = _IsGravity ? _GravityDirection * _GravityAmount * deltaTime : Vector3.zero;
            _MoveDirection = externalForce + accelerate - gravity;

            DetectVerticalCollision();

            GravityProccess();

            ApplyPosition();

            UpdateExternalForce();
        }

        private void ApplyPosition()
        {
            transform.position += _MoveDirection;

            _MoveDirection = Vector3.zero;
        }

        private void UpdateExternalForce()
        {
            var deltaTime = Time.timeScale * TimeUtility.FramerateDeltaTime;
            //_ExternalForce /= (1 + (m_Grounded ? m_ExternalForceDamping : m_ExternalForceAirDamping) * deltaTime);
            _ExternalForce /= (1 + (_ExternalForceDamping) * deltaTime);
        }

        private void GravityProccess()
        {
            _OnGrounded = CheckGround();

            if (_OnGrounded == true)
            {
                _GravityAmount = 0;
                return;
            }

            _GravityAmount += (_GravityMagnitude * -0.001f) / Time.timeScale;
        }

        public void AddForce(Vector3 force)
        {
            AddExternalForce(force, false);
        }

        private void AddExternalForce(Vector3 force, bool scaleByTime)
        {
            if (scaleByTime)
            {
                var timeScale = Time.timeScale;
                if (timeScale < 1)
                {
                    force /= timeScale;
                }
            }
            _ExternalForce += force;
        }

        public void AddRotateForce(Vector3 force)
        {
            _Rigidbody.AddRelativeTorque(force, ForceMode.Impulse);
            //_Rigidbody.AddTorque(force, ForceMode.Force);
        }

        private void DetectVerticalCollision()
        {
            var result = OverlapCount() > 0;
            result = false;

            var activeCollider = _Colliders[0];
            var collider = OverlapColliders();
            var offset = Vector3.zero;


            var localMoveDirection = transform.InverseTransformDirection(_MoveDirection);

            if (collider.Length > 0)
            {
                var overlap = ComputePenetration(activeCollider, collider[0], out offset);

                if (overlap == true)
                {
                    var localOffset = transform.InverseTransformDirection(offset);

                    localMoveDirection.y = transform.InverseTransformDirection(offset).y;

                    /*if (localOffset.y < 0)
                    {
                        var localExternalForce = transform.InverseTransformDirection(_ExternalForce);
                        localExternalForce.y = 0;
                        _ExternalForce = transform.TransformDirection(localExternalForce);
                    }*/


                }
                else
                {
                    localMoveDirection.y = 0;
                }

                _MoveDirection = transform.TransformDirection(localMoveDirection);
            }
        }

        private bool CheckGround()
        {
            var result = OverlapCount() > 0;
            result = false;

            var activeCollider = _Colliders[0];
            var collider = OverlapColliders();
            var offset = Vector3.zero;

            /*if (collider.Length > 0)
            {
                var overlap = ComputePenetration(activeCollider, collider[0], out offset);

                if (overlap == true)
                {
                    var localOffset = transform.InverseTransformDirection(offset);

                    *//*if (localOffset.y < 0)
                    {
                        var localExternalForce = transform.InverseTransformDirection(_ExternalForce);
                        localExternalForce.y = 0;
                        _ExternalForce = transform.TransformDirection(localExternalForce);
                    }*//*

                    var verticalOffset = 0f;
                    if (localOffset.y > 0) { verticalOffset = localOffset.y; localOffset.y = 0; }

                    //_MoveDirection += transform.TransformDirection(localOffset);

                    if (SingleCast(
                        _GravityDirection * (verticalOffset + c_ColliderSpace),
                        Vector3.ProjectOnPlane(_MoveDirection, -_GravityDirection) + Vector3.up * (verticalOffset + c_ColliderSpace)
                    ))
                    {
                        result = true;
                    }
                }
                else
                {
                    //_MoveDirection = Vector3.zero;
                }
            }*/

            var verticalOffset = .2f;
            if (SingleCast(
                _GravityDirection * (verticalOffset + c_ColliderSpace),
                Vector3.up * (verticalOffset - c_ColliderSpace) //Vector3.ProjectOnPlane(_MoveDirection, -_GravityDirection) + Vector3.up * (verticalOffset + c_ColliderSpace)
            ))
            {
                result = true;
            }


            return result;
        }

        #region Physics

        private void TestPhysics()
        {
            var direction = -transform.up;

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius;

                    bool sphereCast = Physics.SphereCast(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, direction.normalized, out RaycastHit hit, direction.magnitude, _SolidLayerMask, QueryTriggerInteraction.Ignore);
                    Debug.Log($"sphereCast {sphereCast}");

                    Collider[] overlapSphere = Physics.OverlapSphere(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, _SolidLayerMask, QueryTriggerInteraction.Ignore);
                    foreach (var c in overlapSphere) Debug.Log($"overlapSphere {c.name}");

                    var overlapColliders = new Collider[10];
                    int overlapSphereNonAlloc = Physics.OverlapSphereNonAlloc(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, overlapColliders, _SolidLayerMask, QueryTriggerInteraction.Ignore);
                    Debug.Log($"overlapSphereNonAlloc {overlapSphereNonAlloc}");

                    bool checkSphere = Physics.CheckSphere(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, _SolidLayerMask, QueryTriggerInteraction.Ignore);
                    Debug.Log($"check sphere {checkSphere}");
                }
            }
        }

        private int OverlapCount()
        {
            int count = 0;

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius + c_ColliderSpace;

                    Collider[] overlapSphere = Physics.OverlapSphere(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, _SolidLayerMask, QueryTriggerInteraction.Ignore);

                    var overlapColliders = new Collider[10];
                    int overlapSphereNonAlloc = Physics.OverlapSphereNonAlloc(
                        sphereCollider.transform.TransformPoint(sphereCollider.center),
                        radius,
                        overlapColliders,
                        _SolidLayerMask,
                        QueryTriggerInteraction.Ignore
                    );

                    count = overlapSphereNonAlloc;
                }
            }

            return count;
        }

        private Collider[] OverlapColliders()
        {
            var result = new Collider[0];

            for (int i = 0; i < _Colliders.Length; i++)
            {
                var collider = _Colliders[i];

                if (collider is SphereCollider)
                {
                    var sphereCollider = collider as SphereCollider;
                    var radius = sphereCollider.radius - c_ColliderSpace;

                    Collider[] overlapSphere = Physics.OverlapSphere(sphereCollider.transform.TransformPoint(sphereCollider.center), radius, _SolidLayerMask, QueryTriggerInteraction.Ignore);
                    result = overlapSphere;
                }
            }

            return result;
        }

        private bool SingleCast(Vector3 direction, Vector3 offset)
        {
            var result = false;

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
                        out RaycastHit _RaycastHit,
                        direction.magnitude,
                        _SolidLayerMask,
                        QueryTriggerInteraction.Ignore
                    );

                    result = sphereCast;
                }
            }

            return result;
        }

        private bool ComputePenetration(Collider colliderA, Collider colliderB, out Vector3 offset)
        {
            offset = Vector3.zero;

            var result = Physics.ComputePenetration(
                colliderA,
                colliderA.transform.position,
                colliderA.transform.rotation,
                colliderB,
                colliderB.transform.position,
                colliderB.transform.rotation,
                out Vector3 direction,
                out float distance
            );

            if (result)
            {
                offset += direction.normalized * (distance + c_ColliderSpace);
            }

            /*if (OverlapCount(colliderA, horizontalDirection + offset) == 0)
            {
                overlap = false;
                break;
            }*/

            return result;
        }

        #endregion
    }
}