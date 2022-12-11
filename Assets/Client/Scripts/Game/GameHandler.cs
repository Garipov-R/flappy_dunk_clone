using Client.Cameras;
using Client.Player;
using System.Collections;
using UnityEngine;

namespace Client.Game
{
    public class GameHandler : MonoBehaviour, IGameLogic
    {
        public static GameHandler Instance { get; private set; }

        [SerializeField] private PlayerBase _Player;
        [SerializeField] private CameraControl _CameraControl;
        [SerializeField] private SpawnObjectsManager _SpawnObjectsManager;
        [SerializeField] private int _TargetFrameRate = 60;

        public PlayerBase Player { get => _Player; set => _Player = value; }
        public CameraControl CameraControl { get => _CameraControl; set => _CameraControl = value; }
        public SpawnObjectsManager SpawnObjectsManager { get => _SpawnObjectsManager; set => _SpawnObjectsManager = value; }
        public bool GameStarted { get; private set; }

        private void Awake()
        {
            Instance = this;

            _CameraControl.Follower = _Player.transform;

            GameLogic.Init(this);

            Application.targetFrameRate = _TargetFrameRate;
        }

        private void Start()
        {
            GameLogic.RestartGame();
        }


        public void GameOver()
        {
            GameStarted = false;
        }

        public void Pause()
        {

        }

        public void RestartGame()
        {
            Score.ResetScore();
        }

        public void StartGame()
        {
            GameStarted = true;
        }
    }
}