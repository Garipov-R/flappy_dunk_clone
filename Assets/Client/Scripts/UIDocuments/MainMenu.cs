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

        private VisualElement _RootVisualElement;
        private Button _StartButton;
        private Button _RestartButton;


        private void Awake()
        {
            _RootVisualElement = _MainMenuDocument.rootVisualElement;

            _StartButton = _RootVisualElement.Q<Button>("StartGame");
            _RestartButton = _RootVisualElement.Q<Button>("RestartGame");

            _StartButton.RegisterCallback<ClickEvent>(e => { GameLogic.StartGame(); });
            _RestartButton.RegisterCallback<ClickEvent>(e => { GameLogic.RestartGame(); });

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
                
                if (GameLogic.CurrentState == GameLogic.State.GameEnd)
                {
                    GameLogic.RestartGame();

                    return;
                }
            }
        }

        public void GameEnd()
        {
            _RestartButton.style.display = DisplayStyle.Flex;
        }

        public void Pause()
        {

        }

        public void RestartGame()
        {
            _StartButton.style.display = DisplayStyle.Flex;
            _RestartButton.style.display = DisplayStyle.None;
        }

        public void StartGame()
        {
            _StartButton.style.display = DisplayStyle.None;
        }
    }
}