using System.Collections;
using UnityEngine;

namespace Client.Trajectory
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryRenderer : MonoBehaviour
    {
        private LineRenderer _LineRenderer;
        public float _Test = .1f;
        public float _Test2 = .94f;


        private void Awake()
        {
            _LineRenderer = GetComponent<LineRenderer>();
        }

        public void ShowTrajectory(Vector3 origin, Vector3 force, Vector3 gravity)
        {
            Vector3[] points = new Vector3[100];

            _LineRenderer.positionCount = points.Length;

            for (int i = 0; i < points.Length; i++)
            {
                float time = i * .1f;
                time = i * _Test;

                //points[i] = origin + force * time + (gravity * time * time / 2f);
                points[i] = origin + force * time + (gravity * time * time / 2f);
                points[i] = origin + force * time + (gravity / 2f * time * time );
            }

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
                time = i * _Test;

                var gravity = gravityDirection * gravityAmount * time;
                var externalForce = externalForceAmount * time;
                var accelerate = Vector3.zero;
                moveDirection = (externalForce + accelerate - gravity);


                points[i] = origin + (externalForce * time) - gravity; 

                gravityAmount += (gravityMagnitude * -0.1f);
                //externalForceAmount /= 1 + .05f * Utilities.TimeUtility.FramerateDeltaTime;
                //externalForceAmount /= 1 + .05f * _Test2;
                externalForceAmount /= 1 + .05f * _Test;
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