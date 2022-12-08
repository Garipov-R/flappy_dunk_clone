using System.Collections;
using UnityEngine;
using Client.Game;
using UnityEngine.InputSystem;

namespace Client.Player
{
    public class PlayerControl : MonoBehaviour, IGameLogic
    {
        [Header("Test")]
        [SerializeField] private bool _Debug;

        [Header("Player config")]
        [SerializeField] private Vector3 _JumpForceDirection = new Vector3(0, 1, 0);
        [SerializeField] private Vector3 _PushForceDirection = new Vector3(1, 0, 0);
        [SerializeField] private float _PressedJumpForce = 0.4f;
        [SerializeField] private float _JumpForce = 4f;
        [SerializeField] private float _PushForce = 4f;

        private PlayerBase _Player;
        private Camera _Camera;
        private SpawnObjectsManager _SpawnObjectsManager;



        // inputs  **** все поменять!!!!
        [Header("Inputs")]
        [SerializeField] private Vector2 _DeltaLimit = new Vector2(0, 0);
        [SerializeField] private float _TimePressed = 0.3f;
        [SerializeField] private float _TimePressDelay = 0.1f;
        private float _TimePressedCount;
        private float _TimePressDelayCount;
        private bool _IsPressed;
        private Vector3 _DeltaVector;
        private Vector2 _StartInputPosition;

        private bool _IsPushed;
        // inputs endd


        private void Awake()
        {
            _Player = GameHandler.Instance.Player;
            _Camera = GameHandler.Instance.CameraControl.GetComponent<Camera>();
            _SpawnObjectsManager = GameHandler.Instance.SpawnObjectsManager;
            GameLogic.Init(this);
        }

        private void Update()
        {
            // inputs
            _DeltaVector = Pointer.current.delta.ReadValue();
            //Debug.Log(_DeltaVector);

            if (Pointer.current.press.wasPressedThisFrame)
            {
                _StartInputPosition = Pointer.current.position.ReadValue();
            }

            if (Pointer.current.press.isPressed == true || Keyboard.current.anyKey.isPressed == true)
            {
                /*if ((Keyboard.current.rightArrowKey.isPressed || _DeltaVector.x > _DeltaLimit.x) && _IsPushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_PushForceDirection * _PushForce);

                    _IsPushed = true;
                }
                else if((Keyboard.current.downArrowKey.isPressed || _DeltaVector.y < -_DeltaLimit.y) && _IsPushed == false)
                {
                    //_Player.GravityAmount = 0;
                    _Player.AddForce(-_JumpForceDirection * _JumpForce);

                    _IsPushed = true;
                }
                else if((Keyboard.current.upArrowKey.isPressed || _DeltaVector.y > _DeltaLimit.y) && _IsPushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_JumpForceDirection * _JumpForce);

                    _IsPushed = true;
                }
                else if ((Keyboard.current.leftArrowKey.isPressed || _DeltaVector.x < 0) && _IsPushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(-_PushForceDirection * _PushForce);

                    _IsPushed = true;
                }*/

                if (Mathf.Abs(_DeltaVector.normalized.magnitude) > 0 && _IsPushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(new Vector3(0, _DeltaVector.normalized.y, _DeltaVector.normalized.x) * _JumpForce);

                    _IsPushed = true;
                }

                

                _TimePressDelayCount += Time.deltaTime;
                _TimePressedCount += Time.deltaTime;

                /*if ((Pointer.current.press.isPressed || Keyboard.current.upArrowKey.isPressed) && (_TimePressDelayCount >= _TimePressDelay) && _IsPushed == false)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_JumpForceDirection * _JumpForce);

                    _IsPushed = true;
                }*/

                if ((Pointer.current.press.isPressed || Keyboard.current.upArrowKey.isPressed) && _TimePressDelayCount >= _TimePressDelay)
                {
                    if (_TimePressedCount > _TimePressed)
                    {
                        _Player.GravityAmount = 0;
                        //_Player.AddForce(_JumpForceDirection * _PressedJumpForce);

                        var inputDirection = _StartInputPosition - Pointer.current.position.ReadValue();
                        inputDirection *= -1;
                        _Player.AddForce(new Vector3(0, inputDirection.normalized.y, inputDirection.normalized.x) * _PressedJumpForce);
                    }
                }
            }

            if (Pointer.current.press.wasReleasedThisFrame == true || Keyboard.current.anyKey.wasReleasedThisFrame == true)
            {
                _IsPushed = false;
                _TimePressDelayCount = 0;
                _TimePressedCount = 0;
            }
            //********




            if (_Debug == false)
            {
                if (_Camera != null)
                {
                    Vector3 playerPositionOnScreen = _Camera.WorldToScreenPoint(_Player.transform.position);

                    if (playerPositionOnScreen.y > Screen.height || playerPositionOnScreen.y < 0)
                    {
                        GameLogic.EndGame();
                    }
                }

                if (_SpawnObjectsManager.TargetRing != null)
                {
                    if (_SpawnObjectsManager.TargetRing.transform.position.z + 5f < _Player.transform.position.z)
                    {
                        GameLogic.EndGame();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            /*if (_IsPressed == true)
            {
                _Player.GravityAmount = 0;
                _Player.AddForce(_JumpForceDirection * _JumpForce);

                _IsPressed = false;
            }
            else if (_IsPushed == true)
            {
                _Player.GravityAmount = 0;
                _Player.AddForce(_PushForceDirection * _PushForce);

                _IsPushed = false;
            }  

            if (_TimePressDelayCount >= _TimePressDelay)
            {
                if (_TimePressed > _TimePressedCount)
                {
                    _Player.GravityAmount = 0;
                    _Player.AddForce(_JumpForceDirection * _PressedJumpForce);
                }
            }*/
        }


        public void GameEnd()
        {
            _Player.IsFreezed = true;
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
            _Player.IsFreezed = false;
        }
    }
}