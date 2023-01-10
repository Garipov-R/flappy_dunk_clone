using System.Collections;
using UnityEngine;

namespace Client.EntityBehaviour.Spawner
{
    public class FreeSpaceForEntity
    {
        private Vector3 _CenterPosition;
        private Vector3 _Size;

        public Vector3 CenterPosition { get => _CenterPosition; set => _CenterPosition = value; }
        public Vector3 Size { get => _Size; set => _Size = value; }


        public FreeSpaceForEntity()
        {

        }
    }
}