using System.Collections;
using UnityEngine;
using Client.Player;
using Client.Game;
using Client.EntityBehaviour;

namespace Client.Ball
{
    public class Ball : PlayerBase
    {
        [SerializeField] private Mouth _Mouth;
        [SerializeField] private Vector3 _OffsetLookAt;
        [SerializeField] private float _RotateLerp = 1f;

        private RaycastHit _RingRaycast;
        private RaycastHit[] _RingRaycasts;
        private RaycastHit[] _BonusEntityRaycasts;
        private EntityBase _HealthEntity;


        protected override void UpdateProccess()
        {
            base.UpdateProccess();

            var verticalOffset = 1f;
            Vector3 direction = Vector3.zero;


            direction = _Velocity;

            if (_Mouth)
            {
                _BonusEntityRaycasts = Physics.SphereCastAll(
                    transform.position,
                    1f,
                    direction.normalized,
                    _Mouth.MaxDistance,
                    _SolidLayerMask,
                    QueryTriggerInteraction.Collide
                );

                foreach (var cast in _BonusEntityRaycasts)
                {
                    if (cast.transform.TryGetComponent(out EntityBase spawnEntityBase))
                    {
                        if (spawnEntityBase is HealthEntity health)
                        {
                            _Mouth.SetMouth(health.transform.position);

                            _HealthEntity = health;

                            break;
                        }
                    }
                }

                if (_HealthEntity == null)
                {
                    _Mouth.CloseMouth();
                }
                else
                {
                    _Mouth.SetMouth(_HealthEntity.transform.position);
                }
            }


            _RingRaycasts = Physics.RaycastAll(
                transform.position,
                direction.normalized, 
                verticalOffset, 
                _SolidLayerMask, 
                QueryTriggerInteraction.Collide
            );

            foreach (var hitItem in _RingRaycasts)
            {
                if (hitItem.transform.TryGetComponent(out EntityBase spawnEntityBase))
                {
                    if (spawnEntityBase is HealthEntity health)
                    {
                        Destroy(health.gameObject);
                    }
                    else if (spawnEntityBase is RingEntity ring)
                    {
                        bool enterCheck = ring.Check(direction);

                        if (ring.IsDanger == true)
                        {
                            _IsAlive = false;
                            GameLogic.GameOver();
                        }
                        else if (enterCheck)
                        {
                            Score.AddScore();
                            Destroy(ring.gameObject, .3f);
                        }

                        ring.Enter(direction);
                    }
                }
            }

            //transform.LookAt(transform.position + direction.normalized * 10f);
            //transform.rotation = Quaternion.LookRotation(direction.normalized + _OffsetLookAt);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction.normalized + _OffsetLookAt), _RotateLerp);

            /*
            if (Physics.Raycast(transform.position, direction.normalized, out _RingRaycast, verticalOffset, _SolidLayerMask, QueryTriggerInteraction.Collide))
            {
                if (_RingRaycast.transform.TryGetComponent(out EntityBase spawnEntityBase))
                {
                    if (spawnEntityBase is HealthEntity health)
                    {
                        Destroy(health.gameObject, .3f);
                    }
                    else if (spawnEntityBase is RingEntity ring)
                    {
                        bool enterCheck = ring.Check(direction);

                        if (enterCheck)
                        {
                            Score.AddScore();
                            Destroy(ring.gameObject, .3f);
                        }

                        ring.Enter(direction);
                    }
                }





                if (_DestroyRing == null)
                {
                    if (_RingRaycast.transform.TryGetComponent(out Rings.DestroyRingCollider destroyRing))
                    {
                        _DestroyRing = destroyRing;
                    }
                }

                if (_DestroyRing != null)
                {
                    direction = (transform.position - _DestroyRing.transform.position).normalized;
                    bool enterCheck = _DestroyRing.Check(direction);


                    if (enterCheck)
                    {
                        Score.AddScore();
                    }
                    else if (_DestroyRing.Entered == false)
                    {
                        _IsAlive = false;
                        GameLogic.GameOver();
                    }

                    _DestroyRing.Enter(direction);

                    _DestroyRing = null;
                }
            }
            */
        }

        protected override void ApplyPosition()
        {
            base.ApplyPosition();
        }

        public override void AddForce(Vector3 force)
        {
            //base.AddForce(force);
            _Rigidbody.velocity = Vector3.zero;
            _Rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1.5f);
            Gizmos.DrawLine(transform.position, transform.position + -Vector3.up * 1.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 1.5f);
            Gizmos.DrawLine(transform.position, transform.position + -Vector3.forward * 1.5f);
            //Gizmos.DrawWireSphere(transform.position + _MoveDirection.normalized, 1f);
            Gizmos.DrawWireSphere(transform.position + _Velocity.normalized, 1f);
        }
    }
}