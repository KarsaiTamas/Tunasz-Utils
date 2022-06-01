using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TunaszUtils
{
    public class SerializedMenuData
    {
        public SerializedProperty
            serialized_prop;
        public SerializedObject
            serialized_Obj;
        public List<SerializedMenuData> childMenus;
    }
    [CustomEditor(typeof(MenuDataHolder)), CanEditMultipleObjects]
    public class MenuEditor : Editor
    {
        public SerializedMenuData menuData;
        
    }
}
