using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Client.Game;

namespace Client.UI
{
    public class UIControl : MonoBehaviour, IGameLogic
    {
        [SerializeField] private Button _StartGameButton;
        [SerializeField] private Button _RestartGameButton;


        private void Awake()
        {
            GameLogic.Init(this);

            _RestartGameButton.onClick.AddListener(() => { Game.GameLogic.RestartGame(); });
            _StartGameButton.onClick.AddListener(() => { Game.GameLogic.StartGame(); });
        }

        public void GameOver()
        {
            _RestartGameButton.gameObject.SetActive(true);
        }

        public void Pause()
        {
           
        }

        public void RestartGame()
        {
            _RestartGameButton.gameObject.SetActive(false);
            _StartGameButton.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            _StartGameButton.gameObject.SetActive(false);
        }
    }
}