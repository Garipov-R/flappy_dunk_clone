using System;
using System.Collections;
using UnityEngine;

namespace Client.EntityBehaviour.Spawner
{
    [Serializable]
    public class SpawnEntityConfig
    {
        [SerializeField] private EntityBase _EntityPrefab;

        [Header("Transform")]
        [SerializeField] private float _MaxVerticalSpawn;
        [SerializeField] private float _MinVerticalSpawn;
        [SerializeField][Min(2f)] private float _MaxHorizontalSpawn = 10f;
        [SerializeField][Min(2f)] private float _MinHorizontalSpawn = 12f;

        public EntityBase EntityPrefab { get => _EntityPrefab; }
        public float MaxVerticalSpawn { get => _MaxVerticalSpawn; set => _MaxVerticalSpawn = value; }
        public float MinVerticalSpawn { get => _MinVerticalSpawn; set => _MinVerticalSpawn = value; }
        public float MaxHorizontalSpawn { get => _MaxHorizontalSpawn; set => _MaxHorizontalSpawn = value; }
        public float MinHorizontalSpawn { get => _MinHorizontalSpawn; set => _MinHorizontalSpawn = value; }
    }
}