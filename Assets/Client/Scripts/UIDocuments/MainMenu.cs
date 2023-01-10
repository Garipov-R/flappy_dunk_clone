using Client.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Input = Client.Inputs.Input;

namespace Client.UIDocuments
{
    public class MainMenu : MonoBehaviour, IGameLogic
    {
        [SerializeField] private UIDocument _MainMenuDocument;

        private Button _ShopButton;
        private Button _SettingsButton;
        private VisualElement _ShopPanel;
        private VisualElement _SettingsPanel;


        private void Awake()
        {
            GameLogic.Init(this);

            
        }

        private void Update()
        {
            if (Input.AnyKeyPressed())
            {
                if (GameLogic.CurrentState == GameLogic.State.RestartGame) 
                {
                    GameLogic.StartGame();

                    return;
                }
                
                if (GameLogic.CurrentState == GameLogic.State.GameOver)
                {
                    GameLogic.RestartGame();

                    return;
                }
            }
        }

        public void GameOver()
        {

        }

        public void Pause()
        {

        }

        public void RestartGame()
        {

        }

        public void StartGame()
        {

        }

        private void OpenShop()
        {

        }
    }
}