using System.Collections;
using UnityEngine;

namespace Client.Utilities
{
    [RequireComponent(typeof(Camera))]
    public class CameraConstantWidth : MonoBehaviour
    {
        [SerializeField] private Vector2 _DefaultResolution = new Vector2(720, 1280);
        [SerializeField] [Range(0f, 1f)] private float _WidthOrHeight = 0;

        private Camera _ComponentCamera;

        private float _InitialSize;
        private float _TargetAspect;

        private float _InitialFov;
        private float _HorizontalFov = 120f;


        private void Start()
        {
            _ComponentCamera = GetComponent<Camera>();
            _InitialSize = _ComponentCamera.orthographicSize;

            _TargetAspect = _DefaultResolution.x / _DefaultResolution.y;

            _InitialFov = _ComponentCamera.fieldOfView;
            _HorizontalFov = CalcVerticalFov(_InitialFov, 1 / _TargetAspect);

            UpdateProccess();
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateProccess();
        }
#endif

        private void UpdateProccess()
        {
            if (_ComponentCamera.orthographic)
            {
                float constantWidthSize = _InitialSize * (_TargetAspect / _ComponentCamera.aspect);
                _ComponentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, _InitialSize, _WidthOrHeight);
            }
            else
            {
                float constantWidthFov = CalcVerticalFov(_HorizontalFov, _ComponentCamera.aspect);
                _ComponentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, _InitialFov, _WidthOrHeight);
            }
        }

        private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

            float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}