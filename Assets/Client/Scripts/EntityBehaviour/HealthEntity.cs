using System.Collections;
using UnityEngine;

namespace Client.EntityBehaviour
{
    public class HealthEntity : EntityBase
    {
        [SerializeField] private int _HealthAdd = 1;

        public int HealthAdd { get => _HealthAdd; set => _HealthAdd = value; }
    }
}