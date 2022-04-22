using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TunaszUtils
{
    [CustomEditor(typeof(Controls))]
    [CanEditMultipleObjects]
    public class ControlsEditor : Editor
    {

        public SerializedProperty
            Script_Prop,
            uiControlsParent_Prop,
            controlUIPrefab_Prop,
            controlSaveName_Prop,
            search_Prop,
            selectionValue_Prop,
            controlNames_Prop,
            defaultKeys_Prop,
            selectionValueString_Prop
            ; 
        List<string> enumTypes ;
        List<Type> types;
        List<SerializedProperty> optionPicked_Props;
        Controls controls;
        private void OnEnable()
        { 
            controls = target as Controls;
            enumTypes = new List<string>();
            types = new List<Type>();
            optionPicked_Props = new List<SerializedProperty>();
            Script_Prop = serializedObject.FindProperty("m_Script"); 
            uiControlsParent_Prop = serializedObject.FindProperty("uiControlsParent"); 
            controlUIPrefab_Prop = serializedObject.FindProperty("controlUIPrefab"); 
            controlSaveName_Prop = serializedObject.FindProperty("controlSaveName");
            search_Prop = serializedObject.FindProperty("search");
            selectionValue_Prop = serializedObject.FindProperty("selectionValue");
            controlNames_Prop= serializedObject.FindProperty("controlNames");
            defaultKeys_Prop = serializedObject.FindProperty("defaultKeys");
            selectionValueString_Prop = serializedObject.FindProperty("selectionValueString");
            for (int i = 0; i < controls.defaultKeys.saveKeys.Count; i++)
            { 
                optionPicked_Props.Add(serializedObject.FindProperty("defaultKeys").FindPropertyRelative("saveKeys").GetArrayElementAtIndex(i).FindPropertyRelative("optionPicked"));
            }
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            { 
                foreach (var item in assembly.GetTypes())
                {
                    if (item.IsEnum)
                    {
                        if (item.Name.Contains(search_Prop.stringValue))
                        {
                            enumTypes.Add(item.FullName);

                            types.Add(item); 

                        }
                    }
                } 
            }


        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(Script_Prop);
            EditorGUILayout.PropertyField(controlNames_Prop);
            EditorGUILayout.PropertyField(defaultKeys_Prop);
            EditorGUILayout.PropertyField(selectionValueString_Prop);
            
            GUI.enabled = true;
            EditorGUILayout.PropertyField(uiControlsParent_Prop);
            EditorGUILayout.PropertyField(controlUIPrefab_Prop);
            EditorGUILayout.PropertyField(controlSaveName_Prop);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(search_Prop);
            if (EditorGUI.EndChangeCheck())
            {
                enumTypes = new List<string>();
                types = new List<Type>();

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    foreach (var item in assembly.GetTypes())
                    {
                        if (item.IsEnum)
                        {
                            if (item.FullName.Contains(search_Prop.stringValue))
                            {
                                enumTypes.Add(item.FullName);
                            types.Add(item); 

                            }
                        }
                    }
                }
            }
            EditorGUI.BeginChangeCheck();
            selectionValue_Prop.intValue = enumTypes.IndexOf(selectionValueString_Prop.stringValue);
            selectionValue_Prop .intValue= EditorGUILayout.Popup(selectionValue_Prop.intValue, enumTypes.ToArray());
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var enumStringList = EnumExtenstions.GetWithOrder(types[selectionValue_Prop.intValue]).ToArray();
            
            if (EditorGUI.EndChangeCheck())
            {
                controlNames_Prop.ClearArray();
                for (int i = 0; i < enumStringList.Length; i++)
                {
                    controlNames_Prop.InsertArrayElementAtIndex(i);
                    controlNames_Prop.GetArrayElementAtIndex(i).stringValue = enumStringList[i];
                }
                selectionValueString_Prop.stringValue = enumTypes[selectionValue_Prop.intValue];
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            if (enumStringList.Length==0)
            {
                return;
            }
            while (enumStringList.Length< controls.defaultKeys.saveKeys.Count)
            {
                controls.defaultKeys.saveKeys.RemoveAt(controls.defaultKeys.saveKeys.Count - 1);
                optionPicked_Props.RemoveAt(controls.defaultKeys.saveKeys.Count - 1);
            }
            for (int i = 0; i < enumStringList.Length; i++)
            {
                if (controls.defaultKeys.saveKeys.Count<= i)
                {
                    controls.defaultKeys.saveKeys.Add(new KeyHolder(i, 1));
                optionPicked_Props.Add(serializedObject.FindProperty("defaultKeys").FindPropertyRelative("saveKeys").GetArrayElementAtIndex(i).FindPropertyRelative("optionPicked"));
                    Debug.Log("asd");
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update(); 
                string item = enumStringList[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item);
                controls.defaultKeys.saveKeys[i].search= EditorGUILayout.TextField(controls.defaultKeys.saveKeys[i].search);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                var keyEnumList = EnumExtenstions.GetWithOrder(typeof(KeyCode)).Where(x=>x.Contains(controls.defaultKeys.saveKeys[i].search)).ToArray();
                if (keyEnumList==null || keyEnumList.Length==0)
                {
                    EditorGUILayout.LabelField("This enum doesn't contains "+ controls.defaultKeys.saveKeys[i].search);

                }
                else if (keyEnumList.Length< controls.defaultKeys.saveKeys[i].optionPicked)
                { 
                }
                else
                {
                    //int index = keyEnumList.ToList().IndexOf(controls.defaultKeys.saveKeys[i].optionValue);
                    //controls.defaultKeys.saveKeys[i].optionPicked= index==-1? 0  : index;
                       
                    optionPicked_Props[i].intValue = EditorGUILayout.Popup(optionPicked_Props[i].intValue, keyEnumList);
                      
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    controls.defaultKeys.saveKeys[i].keyBinding = Convert.ToInt32( 
                        Enum.Parse(
                            typeof(KeyCode), 
                            keyEnumList[optionPicked_Props[i].intValue]));
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update(); 

                }
                EditorGUILayout.EndHorizontal();

            }

            serializedObject.ApplyModifiedProperties();

        }
    }
}
