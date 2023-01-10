using System.Collections;
using UnityEngine;
using System;
using Client.EntityBehaviour.ScriptableObjects;

namespace Client.EntityBehaviour.Spawner
{
    [Serializable]
    public class SpawnEntitySettings
    {
        [SerializeField] private bool _Enable;
        [SerializeField][Range(0, 100)] private float _SpawnChance;
        [SerializeField] private SpawnEntitySO _SpawnEntityItem;

        public bool Enable { get => _Enable; set => _Enable = value; }
        public float SpawnChance { get => _SpawnChance; set => _SpawnChance = value; }
        public SpawnEntitySO SpawnEntityItem { get => _SpawnEntityItem; set => _SpawnEntityItem = value; }
    }
}