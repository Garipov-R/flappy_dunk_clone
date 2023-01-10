using Client.EntityBehaviour.Spawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.EntityBehaviour.ScriptableObjects
{
    public abstract class SpawnEntitySO : ScriptableObject
    {
        protected EntitySpawner _EntitySpawner;

        public EntitySpawner EntitySpawner { get => _EntitySpawner; set => _EntitySpawner = value; }


        public void Init(EntitySpawner entitySpawner)
        {
            _EntitySpawner = entitySpawner;
        }

        public virtual List<EntityBase> InstanceEntity(Vector3 position, Vector3 forward, Vector3 up)
        {
            if (_EntitySpawner == null)
                return new List<EntityBase>();

            return new List<EntityBase>();
        }
    }
}