using System.Collections;
using UnityEngine;

namespace Client.Test
{
    public class TestTrajectory : MonoBehaviour
    {
        [SerializeField] private Rigidbody _Rigidbody;
        [SerializeField] private ForceMode _ForceMode;
        [SerializeField] private Trajectory.TrajectoryRenderer _TrajectoryRenderer;
        [SerializeField] private Trajectory.TrajectoryRenderer _TrajectoryRenderer2;
        [SerializeField] private Vector3 _Force;
        [SerializeField] private Vector3 _Gravity;
        [SerializeField] private Player.PlayerBase _Player;


        public void Update()
        {
            _TrajectoryRenderer.ShowTrajectory(transform.position, _Force, _Gravity, 1);
            _TrajectoryRenderer2.ShowTrajectory3(transform.position + Vector3.right * 5f, _Force, _Player.GravityDirection, _Player.GravityMagnitude);

            if (Input.GetMouseButtonDown(0))
            {
                _Rigidbody.position = transform.position;
                _Rigidbody.velocity = Vector3.zero;
                _Rigidbody.AddForce(_Force, _ForceMode);

                _Player.GravityAmount = 0;
                _Player.SetForce(Vector3.zero);
                _Player.transform.position = transform.position + Vector3.right * 5f;
                _Player.SetForce(_Force);
            }
        }
    }
}