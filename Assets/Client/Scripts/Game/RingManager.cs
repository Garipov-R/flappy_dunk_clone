using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Game
{
    public class RingManager : MonoBehaviour
    {
        [SerializeField] private GameObject _Ring;
        [SerializeField] private Transform _StartSpawnPoint;
        [SerializeField] private Vector3 _SpawnOffset = new Vector3(0,0,15f);
        [SerializeField] private float _YPositionSpawnRange = 3f;
        [SerializeField] private int _SpawnCount = 3;

        private float _SpawnDistance;
        private Vector3 _LastSpawnPosition = Vector3.zero;
        private List<GameObject> _Rings = new List<GameObject>();


        private void Awake()
        {
            _LastSpawnPosition = _StartSpawnPoint.position;
            Spawn();
        }

        /*private void Update()
        {
            if (GameLogic.Instance.PlayerDistanceCovered > _SpawnDistance)
            {
                Spawn();
            }
        }*/

        private void Spawn()
        {
            foreach (var r in _Rings) Destroy(r);
            _Rings.Clear();

            var positionOffset = _LastSpawnPosition;

            for (int i = 0; i < _SpawnCount; i++)
            {
                var ring = Instantiate(_Ring, _StartSpawnPoint.position + positionOffset, Quaternion.identity);
                _Rings.Add(ring);
                positionOffset += _SpawnOffset;

                _SpawnDistance = positionOffset.z;
                positionOffset.y = Random.Range(-_YPositionSpawnRange, _YPositionSpawnRange);
            }

            positionOffset.y = 0;
            _LastSpawnPosition = positionOffset;
        }
    }
}