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

        /// <summary>
        /// Spawns down a cutscene
        /// </summary>
        /// <param name="cutscen">Item to spawn</param>
        /// <param name="startAction">Starting action to perform</param>
        /// <param name="finishAction">Finish action to perform</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual Cutscene SpawnCutscene(Cutscene cutscen, Cutscene.CutsceenTodo startAction, Cutscene.CutsceenTodo finishAction,Canvas canvas, Vector3 position, Vector3 rotation, Transform parent)
        {
            Quaternion rotation2 = Quaternion.Euler(rotation);

            var entitySpawned = Instantiate(cutscen, position, rotation2, parent);
            entitySpawned.StartAction = startAction;
            entitySpawned.FinishAction = finishAction;
            entitySpawned.transform.localPosition = position;
            return entitySpawned;
        }
        /// <summary>
        /// Spawns down a skipable cutscene
        /// </summary>
        /// <param name="cutscen">Item to spawn</param>
        /// <param name="startAction">Starting action to perform</param>
        /// <param name="finishAction">Finish action to perform</param>
        /// <param name="skipKey">The key witch with we can skip the cutscene</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual SkippableCutscene SpawnSkipableCutscene(SkippableCutscene cutscen, Cutscene.CutsceenTodo startAction, Cutscene.CutsceenTodo finishAction,Canvas canvas, KeyCode skipKey, Vector3 position, Vector3 rotation, Transform parent)
        {
            Quaternion rotation2 = Quaternion.Euler(rotation);

            var entitySpawned = Instantiate(cutscen, position, rotation2, parent);
            entitySpawned.StartAction = startAction;
            entitySpawned.FinishAction = finishAction;
            entitySpawned.skipKey = skipKey;
            entitySpawned.transform.localPosition = position;
            return entitySpawned;
        }
    }
    
}
