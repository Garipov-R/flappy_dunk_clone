using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/PlayerSkinItems/PlayerSkin", menuName = "GAME/PlayerSkinItem", order = 4)]
    public class PlayerSkinSO : ScriptableObject
    {
        [SerializeField] private string _Name;
        [SerializeField] private Sprite _Image;

        public string Name { get => _Name; set => _Name = value; }
        public Sprite Image { get => _Image; set => _Image = value; }
    }
}

