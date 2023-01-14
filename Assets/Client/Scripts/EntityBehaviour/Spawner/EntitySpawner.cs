using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Game;
using Client.EntityBehaviour.ScriptableObjects;
using Client.Player;

namespace Client.EntityBehaviour.Spawner
{
    public class EntitySpawner : MonoBehaviour, IGameLogic
    {
        [SerializeField] private Player.PlayerBase _Player;
        [SerializeField] private int _SpawnAmount = 10;
        [SerializeField] private Transform _StartSpawnTransform;
        [SerializeField] private List<SpawnEntitySettings> _SpawnEntitySettings = new List<SpawnEntitySettings>();
        [SerializeField] private List<EntityBase> _HealthEntities = new();
        [SerializeField] private float _PlayerDistance = 10f;

        private List<EntityChunck> _EntityChuncks = new List<EntityChunck>();
        private Vector3 _LastSpawnedPosition;
        private List<FreeSpaceForEntity> _FreeSpaceForEntities = new List<FreeSpaceForEntity>();
        private List<EntityBase> _HealthSpawnEntities = new();

        public PlayerBase Player { get => _Player; set => _Player = value; }


        private void Awake()
        {
            GameLogic.Init(this);

            foreach (var spawnEntitySetting in _SpawnEntitySettings)
            {
                spawnEntitySetting.SpawnEntityItem.Init(this);
            }
        }

        public void Spawn()
        {
            _LastSpawnedPosition = _StartSpawnTransform.position;
            SpawnRings();
            //SpawnHealth();
            SpawnTest();
            CheckPlayerDistance();
        }

        private void CheckPlayerDistance()
        {
            _EntityChuncks.ForEach(entityChunck => 
            {
                var entity = entityChunck.Entities[entityChunck.Entities.Count - 1];
                entity.OnDestroyed += () =>
                {
                    if (Vector3.Distance(_LastSpawnedPosition, _Player.transform.position) < _PlayerDistance)
                    {
                        SpawnRings();
                        SpawnTest();
                        CheckPlayerDistance();
                    }
                };
            });
        }

        private void SpawnTest()
        {
            foreach (var freeSpace in _FreeSpaceForEntities)
            {
                var healthEntity = Instantiate(
                        _HealthEntities[Random.Range(0, _HealthEntities.Count)],
                        freeSpace.GetRandomPosition(),
                        Quaternion.identity
                    );

                _HealthSpawnEntities.Add(healthEntity);
            }
        }

        private void SpawnRings()
        {
            List<EntityChunck> newEntityChuncks = new();
            for (int i = 0; i < _SpawnAmount; i++)
            {
                var chunck = EntityBuilder.Build(
                    GetSpawnEntitySO(),
                    _LastSpawnedPosition,
                    _StartSpawnTransform.forward,
                    _StartSpawnTransform.up
                );

                newEntityChuncks.Add(chunck);
                _EntityChuncks.Add(chunck);

                _LastSpawnedPosition = new Vector3(_LastSpawnedPosition.x, _LastSpawnedPosition.y, chunck.Entities[chunck.Entities.Count - 1].transform.position.z);
            }

            _FreeSpaceForEntities = FreeSpaceGenerator.Generate(newEntityChuncks);
        }

        public void SpawnHealth()
        {
            foreach (var entityChunck in _EntityChuncks)
            {
                float spawnChance = Random.Range(0, 100);
                if (spawnChance > 30)
                {
                    var healthEntity = Instantiate(
                        _HealthEntities[Random.Range(0, _HealthEntities.Count)],
                        entityChunck.Entities[entityChunck.Entities.Count - 1].transform.position + entityChunck.Entities[entityChunck.Entities.Count - 1].transform.forward * 5f,
                        Quaternion.identity
                    );

                    _HealthSpawnEntities.Add(healthEntity);
                }
            }

            /*foreach (var spawnSetting in _SpawnSettings)
            {
                foreach (var entityProperty in spawnSetting.SpawnEntityItem.EntityProperties)
                {
                    float spawnChance = Random.Range(0, 100);
                    if (spawnChance > 30)
                    {
                        var healthEntity = Instantiate(
                            _HealthEntities[Random.Range(0, _HealthEntities.Count)],
                            entityProperty.Entities[entityProperty.Entities.Count - 1].transform.position + entityProperty.Entities[entityProperty.Entities.Count - 1].transform.forward * 5f,
                            Quaternion.identity
                        );

                        _HealthSpawnEntities.Add(healthEntity);
                    }
                }
            }*/
        }

        public void Clear()
        {
            _EntityChuncks.ForEach(entity => { entity.Clear(); });
            _EntityChuncks.Clear();

            _HealthSpawnEntities.ForEach(entity => { if (entity) Destroy(entity.gameObject); });
            _HealthSpawnEntities.Clear();
        }

        public SpawnEntitySO GetSpawnEntitySO()
        {
            if (_StartSpawnTransform == null)
                return null;


            List<SpawnEntitySettings> selectEntity = new List<SpawnEntitySettings>();
            foreach (var item in _SpawnEntitySettings)
            {
                if (item.Enable == true)
                {
                    selectEntity.Add(item);
                }
            }

            SpawnEntitySettings spawnEntitySettings = selectEntity[Random.Range(0, selectEntity.Count)];
            float spawnChance = Random.Range(0f, 100f);

            int iteration = 0;
            while (iteration < 10)
            {
                spawnEntitySettings = selectEntity[Random.Range(0, selectEntity.Count)];

                if (spawnEntitySettings.SpawnChance >= spawnChance)
                {
                    break;
                }

                iteration++;
            }

            return spawnEntitySettings.SpawnEntityItem;
        }


        #region Game Logic

        public void GameOver()
        {
            
        }

        public void Pause()
        {
            
        }

        public void RestartGame()
        {
            Clear();
            Spawn();
        }

        public void StartGame()
        {
            
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            foreach (var chunckItem in _EntityChuncks)
            {
                Gizmos.DrawWireCube(chunckItem.GetCenterPoint(), chunckItem.GetSize());
            }

            Gizmos.color = Color.green;
            _FreeSpaceForEntities.ForEach(freeSpaceItem => 
            {
                Gizmos.DrawWireCube(freeSpaceItem.CenterPosition, freeSpaceItem.Size); 
            });
        }
    }
}