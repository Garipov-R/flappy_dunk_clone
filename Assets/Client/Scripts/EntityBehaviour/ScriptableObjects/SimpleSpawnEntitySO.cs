using Client.EntityBehaviour.Spawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.EntityBehaviour.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/SpawnEntityItems/SimpleSpawnEntityItem", menuName = "GAME/Spawn Entity/Simple spawn item", order = 4)]
    public class SimpleSpawnEntitySO : SpawnEntitySO
    {
        [SerializeField] private SpawnEntityConfig _RingEntityConfig = new SpawnEntityConfig();


        public override List<EntityBase> InstanceEntity(Vector3 position, Vector3 forward, Vector3 up)
        {
            if (_RingEntityConfig.EntityPrefab == null)
                return null;

            var spawnHorizontalPosition = forward * Random.Range(_RingEntityConfig.MinHorizontalSpawn, _RingEntityConfig.MaxHorizontalSpawn);
            var spawnVerticalPosition = up * Random.Range(_RingEntityConfig.MinVerticalSpawn, _RingEntityConfig.MaxVerticalSpawn);
            var spawnPosition = position + spawnHorizontalPosition + spawnVerticalPosition;

            List<EntityBase> entities = new List<EntityBase>();

            var spawnedObject = Instantiate(
                _RingEntityConfig.EntityPrefab,
                spawnPosition,
                _RingEntityConfig.EntityPrefab.transform.rotation
            );

            entities.Add(spawnedObject);

            return entities;
        }
    }
}