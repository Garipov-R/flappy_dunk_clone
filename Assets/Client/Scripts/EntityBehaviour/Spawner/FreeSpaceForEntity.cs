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

        public Vector3 GetRandomPosition()
        {
            Vector3 position = Vector3.zero;

            position = _CenterPosition + 
                new Vector3(
                    Random.Range(-_Size.x, _Size.x) / 2, 
                    Random.Range(-_Size.y, _Size.y) / 2, 
                    Random.Range(-_Size.z, _Size.z) / 2
                );

            return position;
        }
    }
}