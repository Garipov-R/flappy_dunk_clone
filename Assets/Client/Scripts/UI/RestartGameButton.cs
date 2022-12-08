using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class RestartGameButton : MonoBehaviour, Game.IGameLogic
    {
        private Button _RestarButton;


        private void Awake()
        {
            Game.GameLogic.Init(this);

            _RestarButton = GetComponent<Button>();

            _RestarButton.onClick.AddListener(() => { Game.GameLogic.RestartGame(); _RestarButton.interactable = false; });
        }

        public void GameEnd()
        {
            _RestarButton.interactable = true;
        }

        public void Pause()
        {

        }

        public void RestartGame()
        {
            _RestarButton.interactable = false;
        }

        public void StartGame()
        {
            _RestarButton.interactable = false;
        }
    }
}