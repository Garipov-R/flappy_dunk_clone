using System.Collections;
using UnityEngine;

namespace Client.Utilities.Transforms
{
    public class Translate : MonoBehaviour
    {
        [SerializeField] private float _Speed = 1f;
        [SerializeField] private float _Frequency = 1f;
        [SerializeField] private Vector3 _AmplitudeVector = new Vector3(0,1,0);
        [SerializeField] private Vector3 _Rotate = Vector3.up;

        private Vector3 _StartPosition;


        private void Start()
        {
            _StartPosition = transform.position;
        }

        private void FixedUpdate()
        {
            float sin = Mathf.Sin(Time.time * _Frequency * 2f * Mathf.PI);
            Vector3 offset = _AmplitudeVector * sin;
            transform.position = _StartPosition + offset;
            transform.eulerAngles += _Rotate * _Speed * Time.deltaTime;
        }
    }
}