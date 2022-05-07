using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TunaszUtils
{
    public class DefaultPrefs : MonoBehaviour
    {
        public string fileLoc;
        //public List<YourPrefClass> prefabs;
        //

        [MonoScript(type = typeof(System.Enum))]
        public string componentTypeName;
        protected virtual void OnValidate()
        {

            List<GameObject> prefabsTemp = new List<GameObject>();

            var data = AssetLoading.LoadAllAssetsAtPath("Assets/_Project/Prefabs/"+fileLoc, "prefab");
            foreach (var item in data)
            {
                var go = item as GameObject;
                prefabsTemp.Add(go);
            }
            OrderList(prefabsTemp);
        }
        protected virtual void OrderList(List<GameObject> prefabsTemp)
        {
            //prefabsTemp=prefabsTemp.OrderBy(x => (int)x.GetComponent<YourPrefClass>().ordertype).ToList();  
            //prefabs = new List<YourPrefClass>();

            //foreach (var item in prefabsTemp)
            //{
            //    prefabs.Add(item.GetComponent<YourPrefClass>());
            //}


            //var spriteType = EnumExtenstions.GetWithOrder(System.Type.GetType(componentTypeName)).ToList();
             
        }
    }
}
