using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client.Utilities
{
    public class MathUtility
    {
        public float Ballistic(Transform target, Transform origin, float angleInDeegrees, float gravity)
        {
            Vector3 fromTo = target.position - origin.position;
            Vector3 fromToXZ = new Vector3(fromTo.x, 0, fromTo.z);

            float x = fromToXZ.magnitude;
            float y = fromTo.y;
            float angleInRadians = angleInDeegrees * Mathf.PI / 180;
            float v2 = (gravity * x * x) / (2 * (y * Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
            float v = Mathf.Sqrt(Mathf.Abs(v2));

            return v;
        }

        public Quaternion LookTarget(Transform target, Transform origin)
        {
            Vector3 fromTo = target.position - origin.position;
            Vector3 fromToXZ = new Vector3(fromTo.x, 0, fromTo.z);

            return Quaternion.LookRotation(fromToXZ, Vector3.up);
        }
    }
}
