using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TunaszUtils
{
    public class PopUpPrefabs : DefaultPrefs
    {
    public static PopUpPrefabs instance; 
        public List<MenuPopUp> prefabs; 
        private void Awake()
        {
            instance = this;
        }
        protected override void OrderList(List<GameObject> prefabsTemp)
        {
            //prefabsTemp = prefabsTemp.OrderBy(x => (int)x.GetComponent<MenuPopUp>().ePopUp).ToList();
            prefabs = new List<MenuPopUp>();

            foreach (var item in prefabsTemp)
            {
                prefabs.Add(item.GetComponent<MenuPopUp>());
                //prefabs[prefabs.Count - 1].assetPath = "Assets/_Project/Prefabs/" + fileLoc+"/" + prefabs[prefabs.Count - 1].name+ ".prefab";
            }
        }
    }
}
