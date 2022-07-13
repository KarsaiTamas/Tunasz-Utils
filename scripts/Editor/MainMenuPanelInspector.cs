using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    public class SerializedButtonActionValue
    {

        public SerializedProperty selectedValue_Prop;
        public SerializedProperty value_Prop;

        public SerializedButtonActionValue(SerializedProperty selectedValue_Prop, SerializedProperty value_Prop)
        {
            this.selectedValue_Prop = selectedValue_Prop;
            this.value_Prop = value_Prop;
        }

        public SerializedButtonActionValue()
        {
        }
    }
    public class SerializedButtonAction
    {
        public SerializedProperty
            objectScript_Prop,
            selectedScript_Prop,
            values_Prop,
            search_Prop;
        public List<SerializedButtonActionValue> values;
        public List<MethodInfo> methods;
        public SerializedButtonAction()
        {
            this.methods = new List<MethodInfo>();
        }
        public SerializedButtonAction(SerializedProperty objectScript_Prop, SerializedProperty search_Prop, SerializedProperty selectedScript_Prop, SerializedProperty values_Prop)
        {
            this.objectScript_Prop = objectScript_Prop;
            this.search_Prop = search_Prop;
            this.selectedScript_Prop = selectedScript_Prop;
            this.values_Prop = values_Prop;
            this.values = new List<SerializedButtonActionValue>();
            this.methods = new List<MethodInfo>();
        }
    }
    public class SerializedButton
    {
        public MainMenuButton buttonData;
        public SerializedObject button_Obj;
        public SerializedProperty actionsToPerform_Prop,
            textFont_Prop,
            textString_Prop,
            buttonName_Prop,
            textName_Prop;
        public List<SerializedButtonAction> actionsContent;

        //public List<MethodInfo> methods;
        //public SerializedProperty search_Prop, selectedScript_Prop, selectedAction_Prop; 
        public SerializedButton(SerializedObject buttonObject)
        {
            actionsContent = new List<SerializedButtonAction>();
            this.button_Obj = buttonObject;
            buttonData = buttonObject.targetObject as MainMenuButton;

            actionsToPerform_Prop = button_Obj.FindProperty(nameof(MainMenuButton.actionsToPerform));
            textFont_Prop = button_Obj.FindProperty(nameof(MainMenuButton.textFont));
            textString_Prop = button_Obj.FindProperty(nameof(MainMenuButton.textString));
            buttonName_Prop = button_Obj.FindProperty(nameof(MainMenuButton.buttonName));
            textName_Prop = button_Obj.FindProperty(nameof(MainMenuButton.textName));

            for (int i = 0; i < actionsToPerform_Prop.arraySize; i++)
            {
                var buttonAction = actionsToPerform_Prop.GetArrayElementAtIndex(i);
                actionsContent.Add(
                    new SerializedButtonAction(
                        buttonAction.FindPropertyRelative("objectScript"),
                        buttonAction.FindPropertyRelative("search"),
                        buttonAction.FindPropertyRelative("selectedScript"),
                        buttonAction.FindPropertyRelative("values")
                        ));

                var actionValues = buttonAction.FindPropertyRelative("values");
                for (int j = 0; j < actionValues.arraySize; j++)
                {
                    var curValue = actionValues.GetArrayElementAtIndex(j);
                    actionsContent[i].values.Add(new SerializedButtonActionValue(
                        curValue.FindPropertyRelative("selectedValue"),
                        curValue.FindPropertyRelative("value")
                        ));
                }
                //,
                //        buttonAction.FindPropertyRelative("selectedScript"),
                //        buttonAction.FindPropertyRelative("selectedAction")
                ////search_Prop = button_Obj.FindProperty("search");
                //selectedScript_Prop = button_Obj.FindProperty("selectedScript");
                //selectedAction_Prop = button_Obj.FindProperty("selectedAction");  
                if (actionsContent[i].objectScript_Prop.objectReferenceValue != null)
                {
                    var mbs = ((GameObject)actionsContent[i].objectScript_Prop.objectReferenceValue).GetComponents<MonoBehaviour>();
                    foreach (var mb in mbs)
                    {
                        actionsContent[i].methods.AddRange(mb.GetMethods().Where(x => x.Name.Contains(actionsContent[i].search_Prop.stringValue)).ToList());
                    }
                }
            }


        }
    }
    [CustomEditor(typeof(MainMenuPanel))]
    [CanEditMultipleObjects]
    public class MainMenuPanelInspector : Editor
    {
        public SerializedProperty
            Script_Prop,
            id_Prop,
            childButtons_Prop,
            childButtonHolder_Prop;
        public List<SerializedButton> buttons;
        public MainMenuPanel targetPanel;
        private void OnEnable()
        {
            serializedObject.Update();

            targetPanel = (MainMenuPanel)target;

            Script_Prop = serializedObject.FindProperty("m_Script");//upgradeHandler
            id_Prop = serializedObject.FindProperty("id");
            childButtonHolder_Prop = serializedObject.FindProperty("childButtonHolder");
            childButtons_Prop = serializedObject.FindProperty("childButtons");
            targetPanel.OnValidate();
            buttons = new List<SerializedButton>();
            for (int i = 0; i < childButtons_Prop.arraySize; i++)
            {
                //Debug.Log(childButtons_Prop.GetArrayElementAtIndex(i).type);
                var obj = childButtons_Prop.GetArrayElementAtIndex(i);
                if (obj.objectReferenceValue == null)
                {
                    continue;
                }
                SerializedObject curElement = new SerializedObject(obj.objectReferenceValue);

                buttons.Add(new SerializedButton(curElement));
                for (int j = 0; j < buttons[i].actionsContent.Count; j++)
                {
                    buttons[i].actionsContent[j].selectedScript_Prop.intValue =
                    buttons[i].actionsContent[j].methods.IndexOf(buttons[i].actionsContent[j].methods.FirstOrDefault(x =>
                    x.Name == buttons[i].buttonData.actionsToPerform[j].actionName));
                }


                
            }
            ReloadButtons();
             
            serializedObject.ApplyModifiedProperties();

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            GUI.enabled = false;
            EditorGUILayout.PropertyField(Script_Prop);
            EditorGUILayout.PropertyField(id_Prop);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(childButtonHolder_Prop);
            EditorGUILayout.PropertyField(childButtons_Prop);
            if (GUILayout.Button("Add Button", GUILayout.MaxWidth(400), GUILayout.MaxHeight(30)))
            {
                var but = TunaszToolsMenu.AddButton(targetPanel.childButtonHolder[0]);
                but.button = null;
                but.buttonText = null;
                but.OnValidate();
                targetPanel.OnValidate();
                serializedObject.Update();

            }
            if (buttons == null) return;
            for (int i = 0; i < buttons.Count; i++)
            {
                SerializedButton button = buttons[i];

                button.button_Obj.Update();
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                button.buttonData.showButton = EditorGUILayout.Foldout(button.buttonData.showButton, button.button_Obj.targetObject.name);
                if (GUILayout.Button("Remove Button", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
                {
                    DestroyImmediate(button.buttonData.button.gameObject);

                    serializedObject.ApplyModifiedProperties();

                    targetPanel.OnValidate();
                    serializedObject.Update();

                    continue;
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (button.buttonData.showButton)
                {
                    //float xPos, yPos, zPos;
                    float zPos;

                    //float width, height;
                    //GUILayout.BeginHorizontal();
                    //GUILayout.Space(150);
                    //GUILayout.Label("Position");
                    //GUILayout.FlexibleSpace();
                    //GUILayout.EndHorizontal();



                    EditorGUILayout.PropertyField(button.buttonName_Prop);

                    GUILayout.BeginHorizontal();
                    var buttonTransform = button.buttonData.GetComponent<RectTransform>();
                    button.buttonData.rect = EditorGUILayout.RectField(button.buttonData.rect);
                    EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(10));

                    zPos = EditorGUILayout.FloatField(buttonTransform.anchoredPosition3D.z, GUILayout.MaxWidth(100));
                    if (!button.buttonData.transform.parent.TryGetComponent(out LayoutGroup fitter))
                    {
                        buttonTransform.anchoredPosition3D = new Vector3(button.buttonData.rect.x, button.buttonData.rect.y, zPos);
                    }

                    buttonTransform.sizeDelta = new Vector2(button.buttonData.rect.width, button.buttonData.rect.height);

                    GUILayout.EndHorizontal();
                    //
                    GUILayout.Space(30);

                    EditorGUILayout.PropertyField(button.textName_Prop);
                    EditorGUILayout.PropertyField(button.textString_Prop);
                    EditorGUILayout.PropertyField(button.textFont_Prop);

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Action", GUILayout.MaxWidth(400), GUILayout.MaxHeight(30)))
                    {


                        //button.actionsToPerform_Prop = button.button_Obj.FindProperty("actionsToPerform");

                        button.actionsToPerform_Prop.InsertArrayElementAtIndex(button.actionsToPerform_Prop.arraySize);
                        var buttonA = button.actionsToPerform_Prop.GetArrayElementAtIndex(button.actionsToPerform_Prop.arraySize - 1);
                        button.actionsContent.Add(
                        new SerializedButtonAction(
                            buttonA.FindPropertyRelative("objectScript"),
                            buttonA.FindPropertyRelative("search"),
                            buttonA.FindPropertyRelative("selectedAction"),
                            buttonA.FindPropertyRelative("values")
                            ));
                        var actionValues = buttonA.FindPropertyRelative("values");
                        for (int j = 0; j < actionValues.arraySize; j++)
                        {
                            var curValue = actionValues.GetArrayElementAtIndex(j);
                            button.actionsContent[i].values.Add(new SerializedButtonActionValue(
                            curValue.FindPropertyRelative("selectedValue"),
                            curValue.FindPropertyRelative("value")
                            ));
                        }
                        button.buttonData.actionsToPerform.Add(new ButtonAction());
                        button.actionsContent[button.buttonData.actionsToPerform.Count - 1].objectScript_Prop.objectReferenceValue = null;
                        button.actionsContent[button.buttonData.actionsToPerform.Count - 1].search_Prop.stringValue = "";
                        //OnEnable();
                        button.button_Obj.ApplyModifiedProperties();
                        button.button_Obj.Update();
                        serializedObject.ApplyModifiedProperties();

                        serializedObject.Update();

                    }
                    if (button.actionsContent.Count != 0)
                    {

                    }

                    GUILayout.EndHorizontal();
                    GUILayout.Space(20);

                    for (int j = 0; j < button.actionsContent.Count; j++)
                    {
                        var buttonAction = button.actionsContent[j];
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);

                        button.buttonData.actionsToPerform[j].showButtonAction =
                            EditorGUILayout.Foldout(button.buttonData.actionsToPerform[j].showButtonAction, button.button_Obj.targetObject.name + "Action: " + j + " " + button.buttonData.actionsToPerform[j].actionName);
                        if (GUILayout.Button("Remove Action", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20)))
                        {
                            //button.actionsToPerform_Prop.DeleteArrayElementAtIndex(button.actionsToPerform_Prop.arraySize-1);
                            button.buttonData.actionsToPerform.RemoveAt(j);
                            button.actionsContent.RemoveAt(j);

                            //OnEnable();
                            //button.button_Obj.ApplyModifiedProperties();
                            serializedObject.ApplyModifiedProperties();

                            serializedObject.Update();
                            continue;

                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(20);

                        if (button.buttonData.actionsToPerform[j].showButtonAction)
                        {

                            EditorGUILayout.PropertyField(buttonAction.objectScript_Prop);


                            if (button.buttonData.actionsToPerform[j].objectScript != null)
                            {
                                List<string> options = new List<string>();
                                foreach (var item in buttonAction.methods)
                                {
                                    options.Add(item.Name);
                                }
                                EditorGUI.BeginChangeCheck();
                                EditorGUILayout.PropertyField(buttonAction.search_Prop);
                                 
                                var scriptSelect = options.Where(x => x.Contains(buttonAction.search_Prop.stringValue) || buttonAction.search_Prop.stringValue == "").ToArray();
                                 
                                if (EditorGUI.EndChangeCheck())
                                {
                                    int val =
                                        scriptSelect.ToList().FindIndex(x => x.Equals(button.buttonData.actionsToPerform[j].actionName));
                                    buttonAction.selectedScript_Prop.intValue = val != -1?val:0;
                                     
                                    button.button_Obj.ApplyModifiedProperties();
                                    button.button_Obj.Update();
                                    if (scriptSelect.Length > 0)
                                    {
                                        button.buttonData.actionsToPerform[j].actionName = scriptSelect[buttonAction.selectedScript_Prop.intValue];
                                         
                                    } 
                                    button.button_Obj.ApplyModifiedProperties();
                                    button.button_Obj.Update();

                                }
                                 
                                serializedObject.ApplyModifiedProperties();

                                serializedObject.Update();

                                if (scriptSelect != null && scriptSelect.Length != 0)
                                { 
                                    //if (!button.buttonData.actionsToPerform[j].actionName.Equals(scriptSelect[buttonAction.selectedScript_Prop.intValue]))
                                    //{

                                    //    int val2 =
                                    //        scriptSelect.ToList().FindIndex(x => x.Equals(button.buttonData.actionsToPerform[j].actionName));
                                    //    buttonAction.selectedScript_Prop.intValue = val2 != -1 ? val2 : 0; 
                                    //}
                                    EditorGUI.BeginChangeCheck();
                                    if (button.actionsContent[j].methods.IndexOf(button.actionsContent[j].methods.FirstOrDefault(x =>
                                        x.Name == buttons[i].buttonData.actionsToPerform[j].actionName))!= buttonAction.selectedScript_Prop.intValue && buttonAction.selectedScript_Prop.intValue!=-1)
                                    {
                                        buttonAction.selectedScript_Prop.intValue =
                                            button.actionsContent[j].methods.IndexOf(button.actionsContent[j].methods.FirstOrDefault(x =>
                                            x.Name == buttons[i].buttonData.actionsToPerform[j].actionName));
                                    }
                                    if (buttonAction.selectedScript_Prop.intValue==-1)
                                    {
                                        buttonAction.selectedScript_Prop.intValue = 0;
                                    }
                                    button.button_Obj.ApplyModifiedProperties();
                                    button.button_Obj.Update();
                                    
                                    buttonAction.selectedScript_Prop.intValue =
                                    EditorGUILayout.Popup("Function", buttonAction.selectedScript_Prop.intValue,
                                    scriptSelect);
                                    //button.buttonData.actionsToPerform[j].actionName = scriptSelect[buttonAction.selectedScript_Prop.intValue];
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        buttonAction.values_Prop.ClearArray();
                                        buttonAction.values.Clear();
                                        button.button_Obj.ApplyModifiedProperties();
                                        button.button_Obj.Update();

                                         
                                        button.buttonData.actionsToPerform[j].actionName = scriptSelect[buttonAction.selectedScript_Prop.intValue];

                                        button.button_Obj.ApplyModifiedProperties();
                                        button.button_Obj.Update();
                                    } 
                                    var filteredMethods = buttonAction.methods.Where(x => x.Name.Contains(buttonAction.search_Prop.stringValue)).ToList();
                                    var par = filteredMethods[buttonAction.selectedScript_Prop.intValue].GetParameters();
                                    for (int i1 = 0; i1 < par.Length; i1++)
                                    {
                                        if (par.Length > buttonAction.values_Prop.arraySize)
                                        {
                                            buttonAction.values_Prop.InsertArrayElementAtIndex(buttonAction.values_Prop.arraySize);
                                            var curValue = buttonAction.values_Prop.GetArrayElementAtIndex(i1);
                                            buttonAction.values.Add(new SerializedButtonActionValue(
                                                curValue.FindPropertyRelative("selectedValue"),
                                                curValue.FindPropertyRelative("value")
                                                ));
                                        }

                                        if (par[0].ParameterType == typeof(MainUICanvasPanels))
                                        {
                                            continue;
                                        }
                                        ParameterInfo item = par[i1];
                                        if (item.ParameterType == typeof(int))
                                        {
                                            EditorGUILayout.PropertyField(buttonAction.values[i1].selectedValue_Prop);
                                            buttonAction.values[i1].value_Prop.stringValue = buttonAction.values[i1].selectedValue_Prop.intValue.ToString();

                                        }
                                        else if (item.ParameterType.IsEnum)
                                        {
                                            var enumStringList = EnumExtenstions.GetWithOrder(item.ParameterType).ToArray();

                                            buttonAction.values[i1].selectedValue_Prop.intValue = EditorGUILayout.Popup(item.ParameterType.Name, buttonAction.values[i1].selectedValue_Prop.intValue, enumStringList);
                                            buttonAction.values[i1].value_Prop.stringValue = enumStringList[buttonAction.values[i1].selectedValue_Prop.intValue];


                                        }
                                        else if (item.ParameterType == typeof(bool))
                                        {
                                            buttonAction.values[i1].selectedValue_Prop.intValue = Convert.ToInt32(EditorGUILayout.Toggle(item.Name, Convert.ToBoolean(buttonAction.values[i1].selectedValue_Prop.intValue)));
                                            buttonAction.values[i1].value_Prop.stringValue = buttonAction.values[i1].selectedValue_Prop.boolValue.ToString();

                                        }

                                    }

                                    if (par.Length == 2 && par[0].ParameterType == typeof(MainUICanvasPanels) && par[1].ParameterType == typeof(int))
                                    {  
                                        string[] panelsList= targetPanel.relatedCanvas.panels.Select(x=>x.name).ToArray();
                                         buttonAction.values[1].selectedValue_Prop.intValue = EditorGUILayout.Popup("Panel To Show", buttonAction.values[1].selectedValue_Prop.intValue, panelsList);
                                        buttonAction.values[1].value_Prop.stringValue = buttonAction.values[1].selectedValue_Prop.intValue.ToString();
                                    }
                                    
                                    if (par.Length == 3 && par[0].ParameterType == typeof(MainUICanvasPanels) && par[1].ParameterType == typeof(int) && par[2].ParameterType==typeof(bool))
                                    {
                                        string[] panelsList = targetPanel.relatedCanvas.panels.Select(x => x.name).ToArray();
                                        buttonAction.values[1].selectedValue_Prop.intValue = EditorGUILayout.Popup("Panel To Activate", buttonAction.values[1].selectedValue_Prop.intValue, panelsList);
                                        buttonAction.values[1].value_Prop.stringValue = panelsList[buttonAction.values[1].selectedValue_Prop.intValue];
                                        buttonAction.values[2].selectedValue_Prop.intValue = Convert.ToInt32(EditorGUILayout.Toggle("Activate Panel", Convert.ToBoolean(buttonAction.values[2].selectedValue_Prop.intValue)));
                                        buttonAction.values[2].value_Prop.stringValue = buttonAction.values[2].selectedValue_Prop.boolValue.ToString();


                                    }



                                }
                            }


                            GUILayout.Space(20);

                        }


                    }
                } 

                button.button_Obj.ApplyModifiedProperties();

            }

            if (EditorGUI.EndChangeCheck())
            {
                ReloadButtons();
                //OnEnable();
                ReloadScript();

            }
            serializedObject.ApplyModifiedProperties();

        }
        private void ReloadScript()
        {

            var curObj = Selection.activeObject;
            var tempGO = new GameObject();
            Selection.SetActiveObjectWithContext(tempGO, tempGO);
            Selection.SetActiveObjectWithContext(curObj, curObj);
            DestroyImmediate(tempGO);
        }
        private void ReloadButtons()
        {
            buttons = new List<SerializedButton>();
            for (int i = 0; i < childButtons_Prop.arraySize; i++)
            {
                var obj = childButtons_Prop.GetArrayElementAtIndex(i);
                if (obj.objectReferenceValue == null)
                {
                    continue;
                }
                SerializedObject curElement = new SerializedObject(obj.objectReferenceValue);

                buttons.Add(new SerializedButton(curElement));
            }
        }
    }
}
