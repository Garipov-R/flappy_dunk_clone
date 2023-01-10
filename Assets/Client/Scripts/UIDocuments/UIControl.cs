using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Client.Game;
using System;
using Random = UnityEngine.Random;

namespace Client.UIDocuments
{
    public class UIControl : MonoBehaviour, IGameLogic
    {
        [SerializeField] private UIDocument _MainMenuDocument;
        [SerializeField] private VisualTreeAsset _BallItem;
        [SerializeField] private ShopUI _ShopUI = new ShopUI();

        [Header("styles")]
        [SerializeField] private string _EnableStyle;
        [SerializeField] private string _DisableStyle;

        private VisualElement _RootVisualElement;
        private VisualElement _MainMenuPanel;
        private VisualElement _GamePanel;
        private VisualElement _GameOverPanel;
        private VisualElement _ShopPanel;
        private VisualElement _SettingsPanel;
        private Button _StartGameButton;
        private Button _RestartGameButton; 
        private Button _ShopButton;
        private Button _SettingsButton;
        private Button _HomeButton;
        private Button _GoMainMenuButton;
        private Label _ScoreText;
        private Label _ResultText;
        private ListView _ShopItemsListView;

        private List<VisualElement> _VisualElementPanels = new ();

        public VisualElement RootVisualElement { get => _RootVisualElement; private set => _RootVisualElement = value; }


        private void Awake()
        {
            GameLogic.Init(this);

            if (_MainMenuDocument == null)
                return;

            _RootVisualElement = _MainMenuDocument.rootVisualElement;

            // panels
            _MainMenuPanel = _RootVisualElement.Q<VisualElement>("main-menu-panel");
            _GamePanel = _RootVisualElement.Q<VisualElement>("game-panel");
            _GameOverPanel = _RootVisualElement.Q<VisualElement>("game-over-panel"); 
            _ShopPanel = _RootVisualElement.Q<VisualElement>("shop-panel");
            _SettingsPanel = _RootVisualElement.Q<VisualElement>("settings-panel");
            _VisualElementPanels.Add(_MainMenuPanel);
            _VisualElementPanels.Add(_GamePanel);
            _VisualElementPanels.Add(_GameOverPanel); 
            _VisualElementPanels.Add(_ShopPanel); 
            _VisualElementPanels.Add(_SettingsPanel); 
            
            

            _StartGameButton = _RootVisualElement.Q<Button>("start-game-button");
            _RestartGameButton = _RootVisualElement.Q<Button>("restart-game-button"); 
            _ShopButton = _RootVisualElement.Q<Button>("shop-button");
            _SettingsButton = _RootVisualElement.Q<Button>("settings-button");
            _HomeButton = _RootVisualElement.Q<Button>("home-button");
            _GoMainMenuButton = _RootVisualElement.Q<Button>("go-main-menu-button");

            _ScoreText = _RootVisualElement.Q<Label>("score-text");
            _ResultText = _RootVisualElement.Q<Label>("result-text");

            _StartGameButton.RegisterCallback<ClickEvent>(e => { GameLogic.StartGame(); });
            _RestartGameButton.RegisterCallback<ClickEvent>(e => { GameLogic.RestartGame(); GameLogic.StartGame(); }); 
            _ShopButton.RegisterCallback<ClickEvent>(e => { ClickShopPanel(); });
            _SettingsButton.RegisterCallback<ClickEvent>(e => { ClickSettingsPanel(); });
            _HomeButton.RegisterCallback<ClickEvent>(e => { SetActivePanel(_MainMenuPanel); _HomeButton.style.display = DisplayStyle.None; });
            _GoMainMenuButton.RegisterCallback<ClickEvent>(e => { GameLogic.RestartGame(); });

            Score.OnChanged += (result) => 
            {
                _ScoreText.AddToClassList("score-text-anim");
                _ScoreText.schedule.Execute(() => _ScoreText.RemoveFromClassList("score-text-anim")).StartingIn(250);
                _ScoreText.text = result.ToString(); 
            };





            _ShopItemsListView = _RootVisualElement.Q<ListView>("ball-items");
            VisualElement item = _BallItem.CloneTree();

            Func<VisualElement> makeItem = () =>
            {
                return _BallItem.CloneTree();
            };
            var items = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                items.Add(i.ToString());
            }

            Action<VisualElement, int> bindItem = (e, i) =>
            {
                e.Q<Label>("name").text = items[i];
                e.style.backgroundColor = new StyleColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
                e.style.marginBottom = 10;
                e.style.marginLeft = 10;
                e.style.marginRight = 10;
                e.style.marginTop = 10;
                e.style.paddingBottom = 10;
                e.style.paddingTop = 10;
                e.style.paddingRight = 10;
                e.style.paddingLeft = 10;
            };

            _ShopItemsListView.makeItem = makeItem;
            _ShopItemsListView.bindItem = bindItem;
            _ShopItemsListView.itemsSource = items;
            _ShopItemsListView.selectionType = SelectionType.Multiple;

            _ShopItemsListView.RefreshItems();


            _ShopUI.Init(this);
        }

        public void GameOver()
        {
            SetActivePanel(_GameOverPanel);



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
            SetActivePanel(_MainMenuPanel);
        }

        public void StartGame()
        {
            SetActivePanel(_GamePanel);
        }

        private void SetActivePanel(VisualElement visualElement)
        {
            //_MainMenuPanel.style.display = visualElement == _MainMenuPanel ? DisplayStyle.Flex : DisplayStyle.None;
            //_GamePanel.style.display = visualElement == _GamePanel ? DisplayStyle.Flex : DisplayStyle.None;
            //_GameOverPanel.style.display = visualElement == _GameOverPanel ? DisplayStyle.Flex : DisplayStyle.None;

            foreach (var panel in _VisualElementPanels)
            {
                panel.RemoveFromClassList(visualElement == panel ? _DisableStyle : _EnableStyle);
                panel.AddToClassList(visualElement == panel ? _EnableStyle : _DisableStyle);
            }
        }

        private void ClickShopPanel()
        {
            SetActivePanel(_ShopPanel);

            _HomeButton.style.display = DisplayStyle.Flex;
        }
        
        private void ClickSettingsPanel()
        {
            SetActivePanel(_SettingsPanel);

            _HomeButton.style.display = DisplayStyle.Flex;
        }
    }
}