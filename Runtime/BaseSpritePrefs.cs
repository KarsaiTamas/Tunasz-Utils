
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    public class BaseSpritePrefs : MonoBehaviour
    {
        public string fileLoc; 
        //[LabeledArray(componentTypeName)]
        public List<Texture2D> prefabs;

        [MonoScript(type = typeof(System.Enum))]
        public string componentTypeName="";
        //
        public static BaseSpritePrefs instance;
        private void Awake()
        {
            instance = this;
        }
#if UNITY_EDITOR

        protected virtual void OnValidate()
        {

            List<Object> prefabsTemp = new List<Object>();
            var data = AssetLoading.LoadAllAssetsAtPath("Assets/_Project/Textures/" + fileLoc, "png","jpg");
            foreach (var item in data)
            {
                var go = item;
                prefabsTemp.Add(go);
            }
            OrderList(prefabsTemp);
        }
#endif
        protected virtual void OrderList(List<Object> prefabsTemp)
        {
            if (prefabsTemp.Count != prefabs.Count)
            {
                prefabs = new List<Texture2D>();

                foreach (var item in prefabsTemp)
                {
                    prefabs.Add(item as Texture2D);
                }
            }



            var inventoryType = EnumExtenstions.GetWithOrder(System.Type.GetType(componentTypeName)).ToList();
            for (int i = 0; i < inventoryType.Count; i++)
            {
                print(i+ inventoryType[i]);
            }
            for (int i = 0; i < prefabs.Count-1; i++)
            {
                var pref = prefabs[i];
                if (inventoryType[i] == pref.name) continue;
                
                for (int j = i+1; j < prefabs.Count; j++)
                {
                    if (inventoryType[i] == prefabs[j].name)
                    {
                        prefabs[i] = prefabs[j];
                        prefabs[j] = pref;
                        break;
                    }
                }
            }
            //for (int i = 0; i < prefabs.Count; i++)
            //{
            //    if (inventoryType.Count <= i + 1) continue;
                
            //    var itemSwitch = prefabs.FirstOrDefault(x => x.name.Equals(inventoryType[i + 1].ToString()));
            //    if (prefabs.IndexOf(itemSwitch) == -1) continue;

            //    prefabs[prefabs.IndexOf(itemSwitch)] = prefabs[i];
            //    prefabs[i] = itemSwitch;
            //} 
        }
    }
}
