//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;
//namespace TunaszUtils
//{
//    [CustomPropertyDrawer(typeof(MethodAttribute), false)]
//    public class MethodPropertyDrawer : PropertyDrawer
//    {
//        static Dictionary<string, GameObject> m_ScriptCache;
//        static Dictionary<string, MethodInfo> m_Methods;
//        static MethodPropertyDrawer()
//        {
//            m_ScriptCache = new Dictionary<string, GameObject>();
//            m_Methods = new Dictionary<string, MethodInfo>();
             
//            var scripts = Resources.FindObjectsOfTypeAll<GameObject>();
//            for (int i = 0; i < scripts.Length; i++)
//            {
//                var type = scripts[i] ;
//                if (type != null && !m_ScriptCache.ContainsKey(type.name))
//                {
//                    m_ScriptCache.Add(type.name, scripts[i]);
//                }
//            }
//        } 
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            if (property.propertyType == SerializedPropertyType.String)
//            {
//                Rect r = EditorGUI.PrefixLabel(position, label);
//                Rect labelRect = position;
//                labelRect.xMax = r.xMin;
//                position = r;
                 
//                GameObject script = null;
//                string typeName = property.stringValue;
//                if (!string.IsNullOrEmpty(typeName))
//                {
//                    m_ScriptCache.TryGetValue(typeName, out script);
//                    if (script == null)
//                        GUI.color = Color.red;
//                }

//                script = (GameObject)EditorGUI.ObjectField(position, script, typeof(GameObject), true);
//                MethodAttribute attr = (MethodAttribute)attribute;

//                if (GUI.changed)
//                {
//                    if (script != null)
//                    {
//                        var type = script;
//                        if (attr.methodsFromObject != null )
//                            type = null;
//                        if (type != null)
//                        {
//                            property.stringValue = type.name;

//                            var mbs = type.GetComponents<MonoBehaviour>();
//                            m_Methods = new Dictionary<string, MethodInfo>();
//                            foreach (var mb in mbs)
//                            {
//                                foreach (var met in mb.GetMethods())
//                                {
//                                    m_Methods.Add(met.,met);

//                                }
//                            }
//                        }
//                        else
//                            Debug.LogWarning("The script file " + script.name + " doesn't contain an assignable class");
//                    }
//                    else
//                        property.stringValue = "";
//                }
//                if (script!=null)
//                {
//                    MethodInfo curMethod = null;
//                    if (curMethod!=null)
//                    {
//                        m_Methods.TryGetValue(attr.methodInfo.Name, out curMethod);
//                        if (script == null)
//                            GUI.color = Color.red;
                        
//                    }
//                    List<string> content = new List<string>();
//                    foreach (var item in m_Methods)
//                    {
//                        content.Add(item.Key);
//                    }
//                    attr.position = content.IndexOf(attr.selectedMethod);
//                    attr.position = EditorGUI.Popup(new Rect(position.x, position.y+30, position.width, position.height), attr.position, content.ToArray());

//                    if (GUI.changed)
//                    {
//                        if (curMethod != null)
//                        {
//                            var type = curMethod;
//                            if (attr.methodsFromObject != null)
//                                type = null;
//                            if (type != null)
//                            {
//                                attr.selectedMethod = type.Name;
                                 
//                            }
//                            else
//                                Debug.LogWarning("The script file  doesn't contain an assignable class");
//                        }
//                        else
//                            property.stringValue = "";
//                    }
//                }
//            }
//            else
//            {
//                GUI.Label(position, "The MonoScript attribute can only be used on string variables");
//            }
//        }

//    }
//}
