using System.Collections;
using UnityEngine;
using Client.Game;
using UnityEngine.InputSystem;
using Input = Client.Inputs.Input;
using Client.Trajectory;

namespace Client.Player
{
    public class PlayerControl : MonoBehaviour, IGameLogic
    {
        [Header("Test")]
        [SerializeField] private bool _Debug;

        [Header("Components")]
        [SerializeField] private TrajectoryRenderer _TrajectoryRenderer;

        [Header("Player config")]
        [SerializeField] private Vector3 _JumpForceDirection = new Vector3(0, 1, 0);
        [SerializeField] private Vector3 _PushForceDirection = new Vector3(1, 0, 0);
        [SerializeField] private float _PressedJumpForce = 0.4f;
        [SerializeField] private float _JumpForce = 4f;
        [SerializeField] private float _JumpForceTest = 4f;
        [SerializeField] private float _PushForce = 4f; 
        [SerializeField] protected Vector3 _PlayerInputVector = new Vector3(0, 0, 1);
        [SerializeField] private float _TimeScale = .2f;

        [Header("Inputs")]
        [SerializeField] private Vector2 _DeltaLimit = new Vector2(0, 0);
        [SerializeField] private float _JumpPressDelay = 0.23f;
        [SerializeField] private float _JumpDelay = 0.5f;

        private PlayerBase _Player;
        private Camera _Camera;
        private SpawnObjectsManager _SpawnObjectsManager;

        private Vector3 _InputVector;
        private bool _Pushed;
        private bool _Jumped;


        private void Awake()
        {
            GameLogic.Init(this);
            _Player = GameHandler.Instance.Player;
            _Camera = GameHandler.Instance.CameraControl.GetComponent<Camera>();
            _SpawnObjectsManager = GameHandler.Instance.SpawnObjectsManager;
        }

        private void Update()
        {
            PlayerInput();

            if (_Player != null)
            {
                if (_Player.IsAlive == true)
                {
                    _Player.Move(_PlayerInputVector);
                }
                else
                {
                    _Player.Move(Vector3.zero);
                }
            }

            if (_Debug == false)
            {
                if (_Camera != null)
                {
                    Vector3 playerPositionOnScreen = _Camera.WorldToScreenPoint(_Player.transform.position);

                    /*if (playerPositionOnScreen.y > Screen.height || playerPositionOnScreen.y < 0)
                    {
                        GameLogic.GameOver();
                    }*/
                    
                    if (playerPositionOnScreen.y < -30f)
                    {
                        GameLogic.GameOver();

                        if (playerPositionOnScreen.y < -100f)
                        {
                            _Player.IsFreezed = true;
                        }
                    }

                    if (playerPositionOnScreen.y > Screen.height)
                    {
                        if (_Player != null)
                        {
                            _Player.AddForce(Vector3.down * 10);
                        }
                    }
                }

                if (_SpawnObjectsManager.TargetRing != null)
                {
                    if (_SpawnObjectsManager.TargetRing.transform.position.z + 8f < _Player.transform.position.z)
                    {
                        GameLogic.GameOver();
                    }
                }
            }
        }

        public void PlayerInput()
        {
            if (_Player == null)
                return;

            if (_Player.IsFreezed == true)
                return;

            if (_Player.IsAlive == false)
                return;

            if (Input.Pressed("Right"))
            {
                _InputVector = Vector3.right;
            }
            else if (Input.Pressed("Left"))
            {
                _InputVector = Vector3.left;
            }
            else if (Input.Pressed("Jump"))
            {
                _InputVector = Vector3.up;
            }
            else if (Input.Pressed("Down"))
            {
                _InputVector = Vector3.down;
            }
            else if (Pointer.current.press.isPressed && Pointer.current.delta.ReadValue().magnitude > 0)
            {
                var vector = Pointer.current.delta.ReadValue();
                vector.Normalize();
                if (vector.x > _DeltaLimit.x)
                {
                    vector = Vector2.right;
                }
                else if (vector.x < -_DeltaLimit.x)
                {
                    vector = Vector2.left;
                }
                else if (vector.y > _DeltaLimit.y)
                {
                    vector = Vector2.up;
                }
                else if (vector.y < -_DeltaLimit.y)
                {
                    vector = Vector2.down;
                }
                else
                {
                    vector = Vector2.zero;
                }

                _InputVector = vector;
            }

            if (_InputVector.magnitude > 0 && (Pointer.current.press.isPressed == true || Input.AnyKeyDown() == true))
            {
                if ((_InputVector.x > 0) && _Pushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_PushForceDirection * _PushForce);

                    _Pushed = true;
                }
                else if ((_InputVector.x < 0) && _Pushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(-_PushForceDirection * _PushForce);

                    _Pushed = true;
                }
                else if ((_InputVector.y < 0) && _Pushed == false)
                {
                    //_Player.GravityAmount = 0;
                    _Player.AddForce(-_JumpForceDirection * _JumpForce);

                    _Pushed = true;
                }
                else if ((_InputVector.y > 0) && _Pushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_JumpForceDirection * _JumpForce);

                    _Pushed = true;
                }

            }

            if (_InputVector.magnitude == 0)
            {
                if (Input.LongPress("PrimaryAttack", _JumpDelay) && _Jumped == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_JumpForceDirection * _JumpForce);

                    _Jumped = true;
                }
            }

            if (Input.LongPress("Jump", _JumpPressDelay) || Input.LongPress("PrimaryAttack", _JumpPressDelay))
            {
                _Player.GravityAmount = 0;
                _Player.AddForce(_JumpForceDirection * _PressedJumpForce);

                //*var inputDirection = _StartInputPosition - Pointer.current.position.ReadValue();
                //inputDirection *= -1;
                //_Player.AddForce(new Vector3(0, inputDirection.normalized.y, inputDirection.normalized.x) * _PressedJumpForce);*//*
            }

            if (Pointer.current.press.wasReleasedThisFrame == true || Input.AnyKeyReleased())
            {
                _Pushed = false;
                _Jumped = false;
                _InputVector = Vector3.zero;
            }


            // TEST 
            /*if (Input.Pressed("SecondaryAttack") || Input.Pressed("SlowDown"))
            {
                _StartInputPosition = Pointer.current.position.ReadValue();
            }

            if (Input.Down("SecondaryAttack") || Input.Down("SlowDown"))
            {
                Vector3 inputDirection = _StartInputPosition - Pointer.current.position.ReadValue();
                Vector3 screenInputDirection = Pointer.current.position.ReadValue();
                Vector3 worldPoint = _Camera.ScreenToWorldPoint(screenInputDirection);

                //pos = new Vector3(_Player.transform.position.x, pos.y, pos.z);

                var screenToCameraDistance = _Camera.nearClipPlane;
                var mousePosNearClipPlane = new Vector3(screenInputDirection.x, screenInputDirection.y, screenToCameraDistance);
                var worldPointPos = _Camera.ScreenToWorldPoint(mousePosNearClipPlane);

                inputDirection *= -1;
                *//*Debug.Log("input dir " + inputDirection.normalized);
                Debug.Log(screenInputDirection);
                Debug.Log(worldPoint);
                Debug.Log(worldPointPos);*//*

                

                //Debug.DrawLine(_Player.transform.position, _Player.transform.position + -Vector3.Cross(_Camera.transform.forward, _Player.transform.up) * 10f);



                *//*var force = Client.Utilities.MathUtility.Ballistic(worldPoint, _Player.transform.position, 45f, _Player.GravityMagnitude);
                var rotation = Quaternion.LookRotation( _Player.transform.position, _Player.transform.up).eulerAngles;
                Debug.Log(rotation);
                Debug.DrawLine(_Player.transform.position, _Player.transform.position + rotation * 10f);*//*

                _TrajectoryRenderer.ShowTrajectory(
                    _Player.transform.position,
                    _Player.Speed + new Vector3(0, inputDirection.normalized.y, inputDirection.normalized.x) * _JumpForceTest, 
                    (_Player.GravityDirection * _Player.GravityMagnitude)
                );
                _Player.TimeScale = _TimeScale;
            }
            else
            {
                _Player.TimeScale = 1;
            }

            if (Input.Released("SecondaryAttack") || Input.Released("SlowDown"))
            {
                Vector3 inputDirection = _StartInputPosition - Pointer.current.position.ReadValue();
                inputDirection *= -1;
                _Player.GravityAmount = 0;
                _Player.SetForce(new Vector3(0, inputDirection.normalized.y, inputDirection.normalized.x) * _JumpForce);
                //_Player.AddForce(new Vector3(0, inputDirection.normalized.y, inputDirection.normalized.x) * _JumpForceTest);
            }*/
        }

        public void GameOver()
        {
            //_Player.IsFreezed = true;
        }

        public void Pause()
        {
            _Player.IsFreezed = true;
        }

        public void RestartGame()
        {
            _Player.IsFreezed = true;
            _Player.transform.position = Vector3.zero;
        }

        public void StartGame()
        {
            _Player.IsAlive = true;
            _Player.IsFreezed = false;
        }
    }
}