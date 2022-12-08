using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client.Cameras
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Transform _Follower;
        [SerializeField] private Vector3 _OffsetPosition = Vector3.one;

        public Transform Follower { get => _Follower; set => _Follower = value; }


        private void LateUpdate()
        {
            FollowProccess();
        }

        private void OnValidate()
        {
            FollowProccess();
        }

        private void FollowProccess()
        {
            if (_Follower == null)
                return;

            var pos = _Follower.position + _OffsetPosition;
            transform.position = new Vector3(_OffsetPosition.x, _OffsetPosition.y, pos.z);
        }
    }
}

