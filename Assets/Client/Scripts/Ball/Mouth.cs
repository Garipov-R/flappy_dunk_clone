using System.Collections;
using UnityEngine;

namespace Client.Ball
{
    public class Mouth : MonoBehaviour
    {
        [SerializeField] private float _MaxDistance = 10f;
        [SerializeField] private float _MinDistance = 5f;
        [SerializeField] private float _MaxOpen = 80f;
        [SerializeField] private float _MinOpen = -13f;
        [SerializeField] private Transform _MouthTransform;
        [SerializeField] private float _SpeedLerp = 0.2f;

        public float MaxDistance { get => _MaxDistance; set => _MaxDistance = value; }
        public float MinDistance { get => _MinDistance; set => _MinDistance = value; }


        public void SetMouth(Vector3 position)
        {
            float distance = Vector3.Distance(transform.position, position);

            float interpolant = Mathf.InverseLerp(_MaxDistance, _MinDistance, distance);
            float angle = Mathf.Lerp(_MinOpen, _MaxOpen, interpolant);

            if (angle < 0)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }

            //_MouthTransform.localEulerAngles = new Vector3(angle, _MouthTransform.localEulerAngles.y, _MouthTransform.localEulerAngles.z);

            _MouthTransform.localEulerAngles = Vector3.Lerp(
                _MouthTransform.localEulerAngles,
                new Vector3(angle, _MouthTransform.localEulerAngles.y, _MouthTransform.localEulerAngles.z),
                _SpeedLerp
            );
        }

        public void CloseMouth()
        {
            SetMouth(transform.position + transform.position.normalized * _MaxDistance);
        }
    }
}