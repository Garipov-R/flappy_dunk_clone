using System.Collections;
using UnityEngine;

namespace Client.Rings
{
    public class RingsSetuper : MonoBehaviour
    {
        [SerializeField] private GameObject _DestroyPrefab;
        [SerializeField] private GameObject _PerfectDestroyPrefab;

        public GameObject DestroyPrefab { get => _DestroyPrefab; set => _DestroyPrefab = value; }
        public GameObject PerfectDestroyPrefab { get => _PerfectDestroyPrefab; set => _PerfectDestroyPrefab = value; }


        private void Awake()
        {
            RingsConfig.RingsSetuper = this;
        }

        public void SpawnDestroyPrefab(Vector3 position, Quaternion rotation)
        {
            if (_DestroyPrefab != null)
            {
                var g = Instantiate(_DestroyPrefab, position, rotation);
                Destroy(g, 5f);
            }
        }
    }
}