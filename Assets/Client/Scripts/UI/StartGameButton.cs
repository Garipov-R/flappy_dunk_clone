using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class StartGameButton : MonoBehaviour
    {
        private Button _Button;


        private void Awake()
        {
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            Game.GameLogic.StartGame();
        }
    }
}