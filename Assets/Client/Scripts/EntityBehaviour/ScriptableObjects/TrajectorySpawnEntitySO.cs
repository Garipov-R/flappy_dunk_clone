using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.EntityBehaviour.Spawner;

namespace Client.EntityBehaviour.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/SpawnEntityItems/TrajectorySpawnEntityItem", menuName = "GAME/Spawn Entity/Trajectory spawn item", order = 4)]
    public class TrajectorySpawnEntitySO : SpawnEntitySO
    {
        [SerializeField] private SpawnEntityConfig _RingEntityConfig = new SpawnEntityConfig();
        [SerializeField] private Gradient _RingsGradient;

        [Header("Trajectory settings")]
        [SerializeField] private Vector2 _UpForceRange = new Vector2(50, 50);
        [SerializeField] private Vector2 _ForwardForceRange = new Vector2(50, 50);
        [SerializeField] private Vector2 _AmountRange = new Vector2(3, 6);
        [SerializeField] private float _Step = 15f;


        public override List<EntityBase> InstanceEntity(Vector3 position, Vector3 forward, Vector3 up)
        {
            base.InstanceEntity(position, forward, up);

            if (_RingEntityConfig.EntityPrefab == null)
                return null;

            var spawnHorizontalPosition = forward * Random.Range(_RingEntityConfig.MinHorizontalSpawn, _RingEntityConfig.MaxHorizontalSpawn);
            var spawnVerticalPosition = up * Random.Range(_RingEntityConfig.MinVerticalSpawn, _RingEntityConfig.MaxVerticalSpawn);
            var spawnPosition = position + spawnHorizontalPosition + spawnVerticalPosition;
            Vector3 origin = spawnPosition;

            int iterations = (int)Random.Range(_AmountRange.x, _AmountRange.y);
            Vector3 gravity = Physics.gravity;
            float timeStep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations * _Step;
            float drag = _EntitySpawner.Player.Rigidbody.drag;
            Vector3[] points = new Vector3[iterations];
            Vector3 force = new Vector3(0, Random.Range(_UpForceRange.x, _UpForceRange.y), Random.Range(_ForwardForceRange.x, _ForwardForceRange.y));

            Vector3 moveStep = force * timeStep;

            List<EntityBase> entities = new List<EntityBase>();

            for (int i = 0; i < iterations; i++)
            {
                moveStep = (moveStep + (gravity * timeStep * timeStep)) * (1f - timeStep * drag);
                origin += moveStep;
                points[i] = origin;


                var spawnedObject = Instantiate(
                            _RingEntityConfig.EntityPrefab,
                            origin,
                            Quaternion.identity
                        );
                spawnedObject.transform.LookAt(origin + moveStep);
                entities.Add(spawnedObject);
            }

            //_EntitySpawner.LastSpawnedPosition = new Vector3(0, 0, points[points.Length - 1].z);

            SetGradient(entities);

            return entities;
        }

        private void SetGradient(List<EntityBase> entities)
        {
            for (int z = 0; z < entities.Count; z++)
            {
                Color color = _RingsGradient.Evaluate((float)z / entities.Count);
                entities[z].GetComponentInChildren<Renderer>().material.color = color;
            }
        }
    }
}