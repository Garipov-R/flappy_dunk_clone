using System.Collections;
using UnityEngine;
using Client.Player;
using Client.Game;
using Client.EntityBehaviour;

namespace Client.Ball
{
    public class Ball : PlayerBase
    {
        private Rings.DestroyRingCollider _DestroyRing;
        private Rings.Ring _Ring;
        private RaycastHit _RingRaycast;


        protected override void UpdateProccess()
        {
            base.UpdateProccess();

            var verticalOffset = 1f;
            Vector3 direction = Vector3.zero;

            if (SingleCast(_Velocity.normalized, Vector3.zero, out RaycastHit hit, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.TryGetComponent(out Rings.Ring ring))
                {
                    if (_Ring != ring)
                    {
                        _Ring = ring;

                        if (ring.IsDanger == true)
                        {
                            _IsAlive = false;
                            GameLogic.GameOver();
                        }
                    }
                }
            }

            direction = _Velocity;

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
            Gizmos.DrawWireSphere(transform.position + _MoveDirection.normalized, 1f);
            Gizmos.DrawWireSphere(transform.position + _Velocity.normalized, 1f);
        }
    }
}