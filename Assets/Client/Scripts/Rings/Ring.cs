using System.Collections;
using UnityEngine;
using Client.Game;

namespace Client.Rings
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private bool _IsDanger;

        public System.Action OnCompleted;

        public bool IsDanger { get => _IsDanger; set => _IsDanger = value; }


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