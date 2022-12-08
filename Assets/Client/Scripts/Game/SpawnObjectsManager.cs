using Client.Rings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Game
{
    public class SpawnObjectsManager : MonoBehaviour, IGameLogic
    {
        [SerializeField] [Min(1)] private int _StartSpawnCount = 1;
        [SerializeField] [Min(4f)] private float _RandomHorizontalSpawn = 4f;
        [SerializeField] private float _RandomVerticalSpawn = 1f;
        [SerializeField] private SpawnObjectConfig[] _SpawnObjectConfig;
        [SerializeField] private Transform _StartSpawnTransform;

        private List<GameObject> _SpawnedObjects = new List<GameObject>();
        private List<GameObject> _SpawnedRings = new List<GameObject>();
        private Vector3 _LastSpawnedPosition;

        public List<GameObject> SpanwnedObjects { get => _SpawnedObjects; set => _SpawnedObjects = value; }
        public GameObject TargetRing { get; private set; }


        private void Awake()
        {
            GameLogic.Init(this);
        }

        public void GameEnd()
        {

        }

        public void Pause()
        {

        }

        public void RestartGame()
        {
            if (_StartSpawnTransform != null)
                _LastSpawnedPosition = _StartSpawnTransform.position;

            DestroyAllObjects();
        }

        public void StartGame()
        {
            for (int i = 0; i < _StartSpawnCount; i++)
                Spawn();
        }

        private void Spawn()
        {
            if (_StartSpawnTransform == null)
                return;

            if (_SpawnObjectConfig == null)
            {
                return;
            }

            /*if (_SpanwnedObjects != null && _SpanwnedObjects.Length > 0)
            {
                Destroy(_SpanwnedObjects[_SpanwnedObjects.Length - 1]);
            }*/

            var spawnHorizontalPosition =  Vector3.forward * UnityEngine.Random.Range(_RandomHorizontalSpawn / 2, _RandomHorizontalSpawn);
            var spawnVerticalPosition = Vector3.up * UnityEngine.Random.Range(-_RandomVerticalSpawn, _RandomVerticalSpawn);
            var spawnPosition = _LastSpawnedPosition + spawnHorizontalPosition + spawnVerticalPosition;

            var spawnObjectConfig = _SpawnObjectConfig[UnityEngine.Random.Range(0, _SpawnObjectConfig.Length)];
            var spawnChance = UnityEngine.Random.Range(0f, 100f);

            int iteration = 0;
            while (iteration < 10)
            {
                spawnObjectConfig = _SpawnObjectConfig[UnityEngine.Random.Range(0, _SpawnObjectConfig.Length)];

                if (spawnChance < spawnObjectConfig.SpawnChance)
                {
                    break;
                }

                iteration++;
            }


            var ringPrefab = spawnObjectConfig.RingPrefab;

            var spawnedObject = Instantiate(ringPrefab, spawnPosition, ringPrefab.transform.rotation);
            _LastSpawnedPosition = new Vector3(0,0, spawnedObject.transform.position.z);

            _SpawnedObjects.Add(spawnedObject.gameObject);
            _SpawnedRings.Add(spawnedObject.gameObject);

            TargetRing = _SpawnedRings[0];


            // TODO delete this!!!
            spawnedObject.OnCompleted += () => 
            {
                if (_SpawnedRings.Count > 0)
                {
                    _SpawnedRings.Remove(_SpawnedRings[0]);
                }
                
                if (GameHandler.Instance.GameStarted == true) 
                { 
                    Spawn();
                }
            };
        }

        private void DestroyAllObjects()
        {
            if (_SpawnedObjects == null)
                return;

            foreach (var g in _SpawnedObjects)
            {
                if (g != null)
                    Destroy(g);
            }

            _SpawnedObjects.Clear();
            _SpawnedRings.Clear();
        }
    }

    [Serializable]
    public class SpawnObjectConfig
    {
        [SerializeField] private Ring _RingPrefab; 
        [SerializeField] [Range(0, 100)] private float _SpawnChance;

        public Ring RingPrefab { get => _RingPrefab;  }
        public float SpawnChance { get => _SpawnChance; set => _SpawnChance = value; }
    }
}