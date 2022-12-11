using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Client.Game;

namespace Client.UIDocuments
{
    public class UIControl : MonoBehaviour, IGameLogic
    {
        [SerializeField] private UIDocument _MainMenuDocument;

        private VisualElement _RootVisualElement;
        private VisualElement _MainMenuPanel;
        private VisualElement _GamePanel;
        private VisualElement _GameOverPanel;
        private Button _StartGameButton;
        private Button _RestartGameButton;
        private Label _ScoreText;
        private Label _ResultText;


        private void Awake()
        {
            GameLogic.Init(this);

            if (_MainMenuDocument == null)
                return;

            _RootVisualElement = _MainMenuDocument.rootVisualElement;
            _MainMenuPanel = _RootVisualElement.Q<VisualElement>("main-menu-panel");
            _GamePanel = _RootVisualElement.Q<VisualElement>("game-panel");
            _GameOverPanel = _RootVisualElement.Q<VisualElement>("game-over-panel");
            _StartGameButton = _RootVisualElement.Q<Button>("start-game-button");
            _RestartGameButton = _RootVisualElement.Q<Button>("restart-game-button");
            _ScoreText = _RootVisualElement.Q<Label>("score-text");
            _ResultText = _RootVisualElement.Q<Label>("result-text");

            _StartGameButton.RegisterCallback<ClickEvent>(e => { GameLogic.StartGame(); });
            _RestartGameButton.RegisterCallback<ClickEvent>(e => { GameLogic.RestartGame(); });

            Score.OnChanged += (result) => 
            { 
                _ScoreText.text = result.ToString(); 
            };
        }

        public void GameOver()
        {
            _MainMenuPanel.style.display = DisplayStyle.None;
            _GamePanel.style.display = DisplayStyle.None;
            _GameOverPanel.style.display = DisplayStyle.Flex;

            string wordOchko = "";
            int lastNum = Score.ScoreCount % 10;
            if (Score.ScoreCount >= 10 && Score.ScoreCount <= 19)
            {
                wordOchko = "очков";
            }
            else if (lastNum == 0 || (lastNum >= 5 && lastNum <= 9))
            {
                wordOchko = "очков";
            }
            else if (lastNum == 1)
            {
                wordOchko = "очко";
            }
            else if (lastNum >= 2 && lastNum <= 4)
            {
                wordOchko = "очка";
            }

            _ResultText.text = string.Format("у тебя {0} {1}", Score.ScoreCount, wordOchko);
        }

        public void Pause()
        {
            
        }

        public void RestartGame()
        {
            _MainMenuPanel.style.display = DisplayStyle.Flex;
            _GamePanel.style.display = DisplayStyle.None;
            _GameOverPanel.style.display = DisplayStyle.None;
        }

        public void StartGame()
        {
            _MainMenuPanel.style.display = DisplayStyle.None;
            _GamePanel.style.display = DisplayStyle.Flex;
            _GameOverPanel.style.display = DisplayStyle.None;
        }
    }
}