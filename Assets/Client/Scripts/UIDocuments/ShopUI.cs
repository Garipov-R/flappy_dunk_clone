using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Client.UIDocuments
{
    [Serializable]
    public class ShopUI
    {
        [SerializeField] private List<Client.ScriptableObjects.PlayerSkinSO> _PlayerSkinItems = new();
        [SerializeField] private VisualTreeAsset _PlayerSkinAsset;

        private ScrollView _PlayerSkinScrollView;

        private UIControl UIControl { get; set; }


        public void Init(UIControl uIControl)
        {
            UIControl = uIControl;

            _PlayerSkinScrollView = UIControl.RootVisualElement.Q<ScrollView>("player-skin-scroll-view");

            _PlayerSkinScrollView.Clear();

            if (_PlayerSkinAsset == null)
                return;

            foreach (var item in _PlayerSkinItems)
            {
                TemplateContainer playerSkinElement = _PlayerSkinAsset.Instantiate();
                var playerSkinItem = new PlayerSkinItemComponent(item);
                playerSkinItem.SetVisualElements(playerSkinElement);
                playerSkinItem.SetGameData();

                _PlayerSkinScrollView.Add(playerSkinElement);
            }
        }
    }
}