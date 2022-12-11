using System.Collections;
using UnityEngine;
using Client.Player;
using Client.Game;

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

            bool up = Physics.Raycast(transform.position, Vector3.up, verticalOffset, _SolidLayerMask, QueryTriggerInteraction.Collide);
            bool down = Physics.Raycast(transform.position, Vector3.down, verticalOffset, _SolidLayerMask, QueryTriggerInteraction.Collide);
            bool forward = Physics.Raycast(transform.position, Vector3.forward, verticalOffset, _SolidLayerMask, QueryTriggerInteraction.Collide);
            bool back = Physics.Raycast(transform.position, Vector3.back, verticalOffset, _SolidLayerMask, QueryTriggerInteraction.Collide);


            Vector3 direction = Vector3.zero;
            if (up) direction = Vector3.up;
            else if (down) direction = Vector3.down;
            else if (forward) direction = Vector3.forward;
            else if (back) direction = Vector3.back;
            direction.Normalize();

            if (SingleCast(_MoveDirection.normalized, Vector3.zero, out RaycastHit hit, QueryTriggerInteraction.Ignore))
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

            if (Physics.Raycast(transform.position, direction.normalized, out _RingRaycast, verticalOffset, _SolidLayerMask, QueryTriggerInteraction.Collide))
            {
                if (_DestroyRing == null)
                {
                    if (_RingRaycast.transform.TryGetComponent(out Rings.DestroyRingCollider destroyRing))
                    {
                        _DestroyRing = destroyRing;
                    }
                }

                if (_DestroyRing != null)
                {
                    bool enterCheck = false;
                    if (_DestroyRing.Entered == false && _DestroyRing.EnterDirection == Vector3.zero)
                    {
                        enterCheck = true;
                    }
                    else if (_DestroyRing.Entered == false && _DestroyRing.EnterDirection == direction)
                    {
                        enterCheck = true;
                    }

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

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1.5f);
            Gizmos.DrawLine(transform.position, transform.position + -Vector3.up * 1.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 1.5f);
            Gizmos.DrawLine(transform.position, transform.position + -Vector3.forward * 1.5f);
            Gizmos.DrawWireSphere(transform.position + _MoveDirection.normalized, 1f);
        }
    }
}