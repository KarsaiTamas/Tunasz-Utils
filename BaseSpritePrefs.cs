
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
        public List<Texture2D> prefabs;

        [MonoScript(type = typeof(System.Enum))]
        public string componentTypeName;
        //
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
        protected virtual void OrderList(List<Object> prefabsTemp)
        {
            if (prefabsTemp.Count!=prefabs.Count)
            {
                prefabs = new List<Texture2D>();

                foreach (var item in prefabsTemp)
                {
                    prefabs.Add(item as Texture2D);
                }
            }
            
             
            var spriteType = EnumExtenstions.GetWithOrder(System. Type.GetType(componentTypeName)).ToList();
            for (int i = 0; i < prefabs.Count; i++)
            {
                prefabs[i].name = spriteType[i];
            }
        }
    }
}
