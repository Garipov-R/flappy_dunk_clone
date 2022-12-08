using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Client.Player;
using Client.Cameras;
using System;

namespace Client.Game
{
    public static class GameLogic
    {
        public enum State { StartGame, GameEnd, RestartGame }

        private static IGameLogic[] _IGameLogics;

        public static IGameLogic[] IGameLogics { get => _IGameLogics; }
        public static State CurrentState { get; private set; }


        public static void Init(IGameLogic gameLogic)
        {
            if (_IGameLogics != null && _IGameLogics.Length > 0)
            {
                for (int i = 0; i < _IGameLogics.Length; i++)
                {
                    if (_IGameLogics[i] == gameLogic)
                        return;
                }
            }

            if (_IGameLogics == null)
            {
                _IGameLogics = new IGameLogic[0];
            }

            Array.Resize(ref _IGameLogics, _IGameLogics.Length + 1);
            _IGameLogics[_IGameLogics.Length - 1] = gameLogic;
        }

        public static void StartGame()
        {
            if (_IGameLogics == null || _IGameLogics.Length == 0)
                return;

            if (CurrentState == State.StartGame)
                return;

            CurrentState = State.StartGame;

            for (int i = 0; i < _IGameLogics.Length; i++)
            {
                _IGameLogics[i].StartGame();
            }
        }

        public static void EndGame()
        {
            if (_IGameLogics == null || _IGameLogics.Length == 0)
                return;

            if (CurrentState == State.GameEnd)
                return;

            CurrentState = State.GameEnd;

            for (int i = 0; i < _IGameLogics.Length; i++)
            {
                _IGameLogics[i].GameEnd();
            }
        }

        public static void RestartGame()
        {
            if (_IGameLogics == null || _IGameLogics.Length == 0)
                return;

            if (CurrentState == State.RestartGame)
                return;

            CurrentState = State.RestartGame;

            for (int i = 0; i < _IGameLogics.Length; i++)
            {
                _IGameLogics[i].RestartGame();
            }
        }
    }
}