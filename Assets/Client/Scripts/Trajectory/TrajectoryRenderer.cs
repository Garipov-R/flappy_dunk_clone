using System.Collections;
using UnityEngine;

namespace Client.Trajectory
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryRenderer : MonoBehaviour
    {
        [SerializeField] private int _Lenght = 100;
        [SerializeField] private int _Iteration = 100;
        [SerializeField] private int _AddLenght = 5;
        [SerializeField] private float _MaxForce = 70f;
        [SerializeField] private float _TimeStep = 0.02f;

        [Header("Color")]
        [SerializeField] private Color _MinColor = Color.white;
        [SerializeField] private Color _MaxColor = Color.red;
        [SerializeField] private Color _StartColor = Color.white;

        private LineRenderer _LineRenderer;
        [Range (0.1f, 2f)] private float _Step = .1f;


        private void Awake()
        {
            _LineRenderer = GetComponent<LineRenderer>(); 
            //SetLineLenght(_Lenght);
        }

        private void OnValidate()
        {
            _LineRenderer = GetComponent<LineRenderer>();
            //SetLineLenght(_Lenght);
        }

        public void ShowTrajectory4(Vector3 origin, Vector3 force, Vector3 gravity, float drag)
        {
            _Iteration = (int)force.magnitude + _AddLenght;

            Vector3[] points = new Vector3[_Iteration];

            _LineRenderer.positionCount = points.Length;


            float timeStep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
            Vector3 gravityaccel = gravity * timeStep * timeStep;

            drag = 1f - timeStep * drag;
            Vector3 moveStep = force * timeStep;

            for (int i = 0; i < _Iteration; i++)
            {
                moveStep = (moveStep + (gravity * timeStep * timeStep)) * (1f - timeStep * drag);

                //moveStep += gravityaccel;
                //moveStep *= drag;
                origin += moveStep;
                points[i] = origin;
            }

            SetGradient(force);
            _LineRenderer.SetPositions(points);
        }

        private void SetGradient(Vector3 force)
        {
            float interpolate = Mathf.InverseLerp(0, _MaxForce, force.magnitude);
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(_StartColor, 0.0f), new GradientColorKey(Color.Lerp(_MinColor, _MaxColor, interpolate), 0.05f) },
                new GradientAlphaKey[] { new GradientAlphaKey(_StartColor.a, 0.2f), new GradientAlphaKey(Color.Lerp(_MinColor, _MaxColor, interpolate).a, 1.0f) }
            );
            _LineRenderer.colorGradient = gradient;
        }

        public void SetLineLenght(int lenght)
        {
            _Lenght = lenght;
            _LineRenderer.positionCount = _Lenght;
        }

        public void ShowTrajectory(Vector3 origin, Vector3 force, Vector3 gravity, float drag)
        {
            Vector3[] points = new Vector3[_Iteration];

            _LineRenderer.positionCount = points.Length;
            //SetLenght(_Lenght);

            drag = 1f - Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations * drag;
            Vector3 velocity = force;

            for (int i = 0; i < points.Length; i++)
            {
                float time = i * .1f;
                time = i * _Step;
                //time = i * Time.fixedDeltaTime * 5;

                points[i] = origin + force * time + (gravity * time * time / 2f);
            }

            SetGradient(force);
            _LineRenderer.SetPositions(points);
        }

        public void ShowTrajectory2(Vector3 origin, Vector3 force, Vector3 gravityDirection, float gravityMagnitude)
        {
            Vector3[] points = new Vector3[50];

            _LineRenderer.positionCount = points.Length;

            float gravityAmount = 0f;
            Vector3 externalForceAmount = force;
            Vector3 moveDirection = Vector3.zero;

            points[0] = origin;

            for (int i = 0; i < points.Length; i++)
            {
                float time = i * .1f;
                time = i * _Step;

                var gravity = gravityDirection * gravityAmount * time;
                var externalForce = externalForceAmount * time;
                var accelerate = Vector3.zero;
                moveDirection = (externalForce + accelerate - gravity);


                points[i] = origin + (externalForce * time) - gravity; 

                gravityAmount += (gravityMagnitude * -0.1f);
                //externalForceAmount /= 1 + .05f * Utilities.TimeUtility.FramerateDeltaTime;
                //externalForceAmount /= 1 + .05f * _Test2;
                externalForceAmount /= 1 + .05f * _Step;
            }

            _LineRenderer.SetPositions(points);
        }

        public void ShowTrajectory3(Vector3 origin, Vector3 force, Vector3 gravityDirection, float gravityMagnitude)
        {
            Vector3[] points = new Vector3[50];

            _LineRenderer.positionCount = points.Length;

            float gravityAmount = 0;

            for (int i = 0; i < points.Length; i++)
            {
                float time = i * 0.1f;
                points[i] = origin + (force * time) - (gravityDirection * -gravityMagnitude * time * time / 2f);
            }

            _LineRenderer.SetPositions(points);
        }
    }
}