using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.EntityBehaviour.Spawner
{
    public static class FreeSpaceGenerator
    {
        public static List<FreeSpaceForEntity> Generate(List<EntityChunck> entityChuncks)
        {
            var freeSpaceForEntityList = new List<FreeSpaceForEntity>();

            foreach (var entityChunck in entityChuncks)
            {
                Vector3 testSize = new Vector3(1,50,1);
                Vector3 centerPosition = Vector3.zero;

                Vector3 sizeUP = new Vector3(0, testSize.y, 0);
                Vector3 positionUP = GetOffset(entityChunck.GetCenterPoint(), Vector3.up * entityChunck.GetSize().y) + Vector3.up * sizeUP.y / 2;
                positionUP = positionUP - centerPosition;
                sizeUP.y -= positionUP.y;
                positionUP = GetOffset(entityChunck.GetCenterPoint(), Vector3.up * entityChunck.GetSize().y) + Vector3.up * sizeUP.y / 2;
                FreeSpaceForEntity freeSpaceForEntityUP = new FreeSpaceForEntity
                {
                    CenterPosition = positionUP,
                    Size = new Vector3(1, sizeUP.y, entityChunck.GetSize().z)
                };

                Vector3 sizeDOWN = new Vector3(0, testSize.y, 0);
                Vector3 positionDOWN = GetOffset(entityChunck.GetCenterPoint(), Vector3.down * entityChunck.GetSize().y) + Vector3.down * sizeDOWN.y / 2;
                positionDOWN = centerPosition - positionDOWN;
                sizeDOWN.y -= positionDOWN.y;
                positionDOWN = GetOffset(entityChunck.GetCenterPoint(), Vector3.down * entityChunck.GetSize().y) + Vector3.down * sizeDOWN.y / 2;
                FreeSpaceForEntity freeSpaceForEntityDOWN = new FreeSpaceForEntity
                {
                    CenterPosition = positionDOWN,
                    Size = new Vector3(1, sizeDOWN.y, entityChunck.GetSize().z)
                };
                
                FreeSpaceForEntity freeSpaceForEntityFORWARD = new FreeSpaceForEntity
                {
                    CenterPosition = new Vector3(
                        0, 
                        centerPosition.y, 
                        GetOffset(entityChunck.GetCenterPoint(), Vector3.forward * entityChunck.GetSize().z).z + testSize.z / 2
                    ),
                    Size = new Vector3(1, testSize.y, testSize.z)
                };

                freeSpaceForEntityList.Add(freeSpaceForEntityUP);
                freeSpaceForEntityList.Add(freeSpaceForEntityDOWN);
                freeSpaceForEntityList.Add(freeSpaceForEntityFORWARD);
            }

            return freeSpaceForEntityList;
        }

        public static Vector3 GetOffset(Vector3 position, Vector3 offset)
        {
            return position + (offset / 2);
        }
    }
}