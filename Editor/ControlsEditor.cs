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
            controlNames_Prop,
            defaultKeys_Prop,
            componentTypeName_Prop
            ;  
        List<SerializedProperty> optionPicked_Props;
        List<SerializedProperty> canBeDuplicate_Props;
        Controls controls;
        private void OnEnable()
        { 
            controls = target as Controls; 
            optionPicked_Props = new List<SerializedProperty>();
            canBeDuplicate_Props = new List<SerializedProperty>();
            Script_Prop = serializedObject.FindProperty("m_Script"); 
            uiControlsParent_Prop = serializedObject.FindProperty("uiControlsParent"); 
            controlUIPrefab_Prop = serializedObject.FindProperty("controlUIPrefab"); 
            controlSaveName_Prop = serializedObject.FindProperty("controlSaveName"); 
            controlNames_Prop= serializedObject.FindProperty("controlNames");
            defaultKeys_Prop = serializedObject.FindProperty("defaultKeys"); 
            componentTypeName_Prop = serializedObject.FindProperty("componentTypeName");
            
            for (int i = 0; i < controls.defaultKeys.saveKeys.Count; i++)
            { 
                optionPicked_Props.Add(serializedObject.FindProperty("defaultKeys").FindPropertyRelative("saveKeys").GetArrayElementAtIndex(i).FindPropertyRelative("optionPicked"));
                canBeDuplicate_Props.Add(serializedObject.FindProperty("defaultKeys").FindPropertyRelative("saveKeys").GetArrayElementAtIndex(i).FindPropertyRelative("canBeDuplicate"));
            } 


        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(Script_Prop);
            EditorGUILayout.PropertyField(controlNames_Prop);
            EditorGUILayout.PropertyField(defaultKeys_Prop);
            
            GUI.enabled = true;
            EditorGUILayout.PropertyField(uiControlsParent_Prop);
            EditorGUILayout.PropertyField(controlUIPrefab_Prop);
            EditorGUILayout.PropertyField(controlSaveName_Prop);
            EditorGUILayout.PropertyField(componentTypeName_Prop);
             
            EditorGUI.BeginChangeCheck();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var enumStringList = EnumExtenstions.GetWithOrder(System. Type.GetType(componentTypeName_Prop.stringValue)).ToArray();

            if (EditorGUI.EndChangeCheck() || controlNames_Prop.arraySize!= enumStringList.Length)
            {
                controlNames_Prop.ClearArray();
                for (int i = 0; i < enumStringList.Length; i++)
                {
                    controlNames_Prop.InsertArrayElementAtIndex(i);
                    controlNames_Prop.GetArrayElementAtIndex(i).stringValue = enumStringList[i];
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            if (enumStringList.Length==0)
            {
                return;
            }
            while (enumStringList.Length< controls.defaultKeys.saveKeys.Count)
            {
                optionPicked_Props.RemoveAt(controls.defaultKeys.saveKeys.Count - 1);
                canBeDuplicate_Props.RemoveAt(controls.defaultKeys.saveKeys.Count - 1);
                controls.defaultKeys.saveKeys.RemoveAt(controls.defaultKeys.saveKeys.Count - 1);
                 
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            for (int i = 0; i < enumStringList.Length; i++)
            {
                if (controls.defaultKeys.saveKeys.Count<= i)
                {
                    controls.defaultKeys.saveKeys.Add(new KeyHolder(i, 1));

                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    optionPicked_Props.Add(serializedObject.FindProperty("defaultKeys").FindPropertyRelative("saveKeys").GetArrayElementAtIndex(i).FindPropertyRelative("optionPicked"));
                canBeDuplicate_Props.Add(serializedObject.FindProperty("defaultKeys").FindPropertyRelative("saveKeys").GetArrayElementAtIndex(i).FindPropertyRelative("canBeDuplicate"));
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update(); 
                string item = enumStringList[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(item,GUILayout.Width(150));
                EditorGUILayout.LabelField("Can be Duplicate", GUILayout.Width(100));
                canBeDuplicate_Props[i].boolValue=EditorGUILayout.Toggle(canBeDuplicate_Props[i].boolValue);
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
