using System.Collections;
using UnityEngine;
using Client.Game;

namespace Client.Rings
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private GameObject _DestroyPrefab;
        [SerializeField] private GameObject _PerfectDestroyPrefab;

        public System.Action OnCompleted;


        public void Complete()
        {
            OnCompleted?.Invoke();
        }

        private void OnDestroy()
        {
            OnCompleted?.Invoke();
        }
    }
}