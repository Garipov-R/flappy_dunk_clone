using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.EntityBehaviour.ScriptableObjects;

namespace Client.EntityBehaviour
{
    public static class EntityBuilder
    {
        public static EntityChunck Build(SpawnEntitySO spawnEntitySO, Vector3 position, Vector3 forward, Vector3 up)
        {
            List<EntityBase> entities = spawnEntitySO.InstanceEntity(position, forward, up);

            EntityChunck chunck = new EntityChunck();

            entities.ForEach(entities => 
            { 
                chunck.Entities.Add(entities); 
            });

            return chunck;
        }
    }
}