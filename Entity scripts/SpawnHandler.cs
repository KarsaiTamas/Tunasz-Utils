using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TunaszUtils
{
    public class SpawnHandler : MonoBehaviour
    { 
        public List<Entity> spawnedDownEntities = new List<Entity>();

        public virtual Entity SpawnEntity(Entity entityToSpawn, Vector3 position, Vector3 rotation)
        {
            Quaternion rotation2 = Quaternion.Euler(rotation);
            var foundEntity= 
                spawnedDownEntities.FirstOrDefault(x => x.entityType == entityToSpawn.entityType && !x.gameObject.activeSelf);
            if (foundEntity!=null)
            {
                foundEntity.transform.SetPositionAndRotation(position, rotation2);
                foundEntity.gameObject.SetActive(true);
                return foundEntity;
            }
            
            var entitySpawned = Instantiate(entityToSpawn,position, rotation2);
            spawnedDownEntities.Add(entitySpawned);

                return entitySpawned;

        }
        public virtual Entity SpawnEntity(Entity entityToSpawn, Vector3 position, Quaternion rotation)
        { 
            var foundEntity =
                spawnedDownEntities.FirstOrDefault(x => x.entityType == entityToSpawn.entityType && !x.gameObject.activeSelf);
            if (foundEntity != null)
            {
                foundEntity.transform.SetPositionAndRotation(position, rotation);
                foundEntity.gameObject.SetActive(true);
                return foundEntity;
            }

            var entitySpawned = Instantiate(entityToSpawn, position, rotation);
            spawnedDownEntities.Add(entitySpawned);

            return foundEntity;

        }
        public virtual MenuPopUp SpawnPopUp(MenuPopUp popUpToSpawn, Vector3 position, Vector3 rotation,Transform parent)
        {
            Quaternion rotation2 = Quaternion.Euler(rotation);
             
            var entitySpawned = Instantiate(popUpToSpawn, position, rotation2,parent);
            entitySpawned.transform.localPosition = position;
            return entitySpawned;

        }
    }
}
