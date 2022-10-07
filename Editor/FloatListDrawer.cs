//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;
//namespace TunaszUtils
//{
//    [CustomPropertyDrawer(typeof(FloatList))]
//    public class FloatListDrawer : PropertyDrawer
//    {

//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return EditorGUI.GetPropertyHeight(property, label);
//        }

//        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
//        {

//            EditorGUI.BeginProperty(rect, label, property);
//            try
//            {
//                var path = property.propertyPath;
//                int pos = int.Parse(path.Split('[').LastOrDefault().TrimEnd(']'));
//                EditorGUI.PropertyField(rect, property, new GUIContent("asd"), true);
//            }
//            catch
//            {
//                EditorGUI.PropertyField(rect, property, label, true);
//            }
//            EditorGUI.EndProperty();
//        }
//    }
//}
