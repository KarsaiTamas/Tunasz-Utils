using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TunaszUtils
{
    public class GOPrefs : DefaultPrefs
    {
        public List<GameObject> prefabs;
        public static GOPrefs instance;
        private void Awake()
        {
            instance = this;
        }
#if UNITY_EDITOR

        protected override void OnValidate()
        {
            instance = this;
            base.OnValidate();
        }
#endif
        protected override void OrderList(List<GameObject> prefabsTemp)
        {
            prefabs = new List<GameObject>();

            foreach (var item in prefabsTemp)
            {
                prefabs.Add(item);
            }
        }
    }
}
