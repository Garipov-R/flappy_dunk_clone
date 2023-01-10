using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Client.ScriptableObjects;

namespace Client.UIDocuments
{
    public class PlayerSkinItemComponent
    {
        private Label _NameText;
        private VisualElement _Image;

        private PlayerSkinSO _PlayerSkinSO;


        public PlayerSkinItemComponent(PlayerSkinSO playerSkinSO)
        {
            _PlayerSkinSO = playerSkinSO;
        }

        public void SetVisualElements(TemplateContainer templateContainer)
        {
            _NameText = templateContainer.Q<Label>("name");
            _Image = templateContainer.Q<Image>("image");
        }

        public void SetGameData()
        {
            _NameText.text = _PlayerSkinSO.Name;
            if (_PlayerSkinSO.Image) _Image.style.backgroundImage = new StyleBackground(_PlayerSkinSO.Image);
        }
    }
}
