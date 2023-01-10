using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.EntityBehaviour
{
    public class EntityChunck
    {
        [SerializeField] private List<EntityBase> _Entities = new List<EntityBase>();

        public List<EntityBase> Entities { get => _Entities; set => _Entities = value; }


        public void Clear()
        {
            _Entities.ForEach(entity => 
            {
                if (entity != null) 
                {
                    Object.Destroy(entity.gameObject);
                } 
            });

            _Entities.Clear();
        }

        public Vector3 GetCenterPoint()
        {
            return GetBounds().center;
        }

        public Vector3 GetSize()
        {
            return GetBounds().size;
        }

        public GameObject GetLast()
        {
            if (_Entities.Count > 1)
            {
                if (_Entities[_Entities.Count - 1] != null)
                {
                    return _Entities[_Entities.Count - 1].gameObject;
                }
            }

            return null;
        }

        private Bounds GetBounds()
        {
            List<EntityBase> spawnEntities = new List<EntityBase>();
            foreach (var item in _Entities)
            {
                if (item == null) continue;

                spawnEntities.Add(item);
            }

            if (spawnEntities.Count == 0)
                return new Bounds();

            var bounds = new Bounds(spawnEntities[0].transform.position, Vector3.zero);
            for (int i = 0; i < spawnEntities.Count; i++)
            {
                bounds.Encapsulate(spawnEntities[i].transform.position);
            }

            return bounds;
        }
    }
}