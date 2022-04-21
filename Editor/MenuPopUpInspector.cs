using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    [CustomEditor(typeof(MenuPopUp)), CanEditMultipleObjects]
    public class MenuPopUpInspector : Editor
    {
        public SerializedProperty
            Script_prop,
            buttons_prop,
            texts_prop,
            buttonAmount_Prop,
            assetPath_Prop,
            setDefaultValues_Prop,
            textMessage_Prop,
            coverScreen_Prop,
            save_Prop;
        //public List<SerializedObject> buttonObjects;
        //public List<SerializedObject> textObjects;
        private MenuPopUp menuPopUp;
        private string prefabLoc;
        private void OnEnable()
        {
            Script_prop = serializedObject.FindProperty("m_Script");//upgradeHandler
            buttons_prop = serializedObject.FindProperty("buttons");
            texts_prop = serializedObject.FindProperty("texts");
            setDefaultValues_Prop = serializedObject.FindProperty("setDefaultValues");
            save_Prop = serializedObject.FindProperty("save");
            textMessage_Prop = serializedObject.FindProperty("textMessage");
            coverScreen_Prop = serializedObject.FindProperty("coverScreen");
            
            menuPopUp = (MenuPopUp)target;
            prefabLoc = "Assets/_Project/Prefabs/PopUps/" + menuPopUp.name + ".prefab";
            buttonAmount_Prop = serializedObject.FindProperty("buttonAmount");
            assetPath_Prop = serializedObject.FindProperty("assetPath");
            
            //buttonObjects = new List<SerializedObject>();
            //textObjects = new List<SerializedObject>();
            //SetUpButtonProps();
            //if (buttonObjects.Count == 0)
            //{

            //    PopUpType ePopUp = (PopUpType)popUpType_Prop.intValue;
            //    switch (ePopUp)
            //    {
            //        case PopUpType.YesNo:
            //            SetUpYesNoPopUp();
            //            break;
            //        case PopUpType.Ok:
            //            SetOkPopUp();
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //SetUpButtonProps();


        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(Script_prop);
            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(save_Prop);
            if (EditorGUI.EndChangeCheck())
            {
                save_Prop.boolValue = false;
                prefabLoc = "Assets/_Project/Prefabs/PopUps/" + menuPopUp.name + ".prefab";
                var save = menuPopUp.gameObject;
                PrefabUtility.SaveAsPrefabAsset(save, prefabLoc);

            }
            EditorGUILayout.PropertyField(setDefaultValues_Prop);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(coverScreen_Prop);
            if (EditorGUI.EndChangeCheck())
            {
                var sourceSize = menuPopUp.gameObject.GetComponent<RectTransform>();
                if (coverScreen_Prop.boolValue)
                {
                    sourceSize.anchorMin = new Vector2(0f, 0f);
                    sourceSize.anchorMax = new Vector2(1f, 1f);
                    sourceSize.offsetMin = new Vector2(0, 0);
                    sourceSize.offsetMax = new Vector2(0, 0);
                }
                else
                {

                    sourceSize.anchorMin = new Vector2(.5f, .5f);
                    sourceSize.anchorMax = new Vector2(.5f, .5f);
                    float slearpT = float.IsNaN(menuPopUp.buttons.Count / 10) ? 0 : (float)menuPopUp.buttons.Count / (float)10;
                    sourceSize.sizeDelta = new Vector2(Mathf.Lerp(1000, 1500, slearpT), 500);
                }
            }
            EditorGUI.BeginChangeCheck();
            if ((!PrefabUtility.IsPartOfPrefabAsset(menuPopUp.gameObject) || StageUtility.GetCurrentStage().GetType() == typeof(PreviewSceneStage)) && !PrefabUtility.IsPartOfPrefabInstance(menuPopUp))
            { 
                EditorGUILayout.PropertyField(buttonAmount_Prop);
            }
            if (EditorGUI.EndChangeCheck())
            { 
                MakeButtons();

            }
            EditorGUILayout.PropertyField(textMessage_Prop);
            EditorGUILayout.PropertyField(buttons_prop);
            EditorGUILayout.PropertyField(texts_prop);
            


            serializedObject.ApplyModifiedProperties();
        }
        private void MakeButtons()
        {
            var source = menuPopUp.gameObject;
            int buttonAmount = buttonAmount_Prop.intValue;
            //if (PrefabUtility.IsPartOfPrefabAsset(source) && StageUtility.GetCurrentStage().GetType()!=typeof(PreviewSceneStage))
            //{
            //    Debug.Log("Can't change prefab in scene open it up.");
            //    return;
            //}

            if (menuPopUp.textMessage == null)
            {
                var textMessage = new GameObject("Message-Text").AddComponent<Text>();
                textMessage.transform.SetParent( source.transform);
                textMessage.transform.localScale = new Vector3(1, 1, 1);
                var tran = textMessage.GetComponent<RectTransform>();
                tran.localPosition = new Vector3(0, 150, 0);
                tran.sizeDelta = new Vector2(600, 150);
                textMessage.resizeTextForBestFit = true;
                textMessage.alignment = TextAnchor.MiddleCenter;
                textMessage.text = "Default text";
                textMessage.color = Color.black;
                menuPopUp.textMessage = textMessage;
            }

            for (int i = source.transform.childCount - 1; i >= buttonAmount+1; i--)
            {
                menuPopUp.texts.Remove(source.transform.GetChild(i).GetChild(0).GetComponent<Text>());
                menuPopUp.buttons.Remove(source.transform.GetChild(i).GetComponent<Button>());

                Object.DestroyImmediate(source.transform.GetChild(i).gameObject);

            }
            for (int i = source.transform.childCount-1; i < buttonAmount; i++)
            {
                var go = new GameObject("Option"+i+"-Button", typeof(Button), typeof(Image));
                var button = go.GetComponent<Button>();
                go.transform.SetParent( source.transform);
                var buttonTran = button.GetComponent<RectTransform>();
                buttonTran.transform.localScale = new Vector3(1, 1, 1);
                buttonTran.sizeDelta = new Vector2(300, 100);
                Text text = new GameObject("Option" + i + "-Text").AddComponent<Text>();
                text.transform.SetParent(button.transform);
                text.transform.localScale = new Vector3(1, 1, 1);
                var okTran = text.gameObject.GetComponent<RectTransform>();
                okTran.anchorMin = new Vector2(0, 0);
                okTran.anchorMax = new Vector2(1, 1);
                okTran.localPosition = new Vector2(0, 0);
                okTran.offsetMin = new Vector2(0, 0);
                okTran.offsetMax = new Vector2(0, 0);
                text.resizeTextForBestFit = true;
                text.text = "Option" + i;
                text.alignment = TextAnchor.MiddleCenter;

                text.color = Color.black;
                //menuPopUp.buttons.Add(button);
                //menuPopUp.texts.Add(text);
            }
            for (int i = 0; i < menuPopUp.buttons.Count; i++)
            {
                var but = menuPopUp.buttons[i];
                var buttonTran = but.GetComponent<RectTransform>();

                float learpT = float.IsNaN((float)i / (float)(menuPopUp.buttons.Count-1) ) ? 0: (float)i/ (float)(menuPopUp.buttons.Count-1) ; 
                float sizelearpT = float.IsNaN( menuPopUp.buttons.Count/10) ? 0: (float)menuPopUp.buttons.Count / (float)10;
                float poslearpT = float.IsNaN( menuPopUp.buttons.Count/10) ? 0: (float)menuPopUp.buttons.Count / (float)10;
                if (menuPopUp.buttons.Count==1)
                {
                    learpT = .5f; 
                    
                }
                Debug.Log(learpT);
                buttonTran.sizeDelta = new Vector2(Mathf.Lerp(300,100, sizelearpT),100);
                float xMinPos = Mathf.Lerp(-200, -650, poslearpT);
                float xMaxPos = Mathf.Lerp(200, 650, poslearpT);
                but.transform.localPosition = new Vector3(Mathf.Lerp(xMinPos,xMaxPos, learpT), -150, 0);
            }
            var sourceSize =source.GetComponent<RectTransform>();

            if (coverScreen_Prop.boolValue)
            {

                sourceSize.anchorMin = new Vector2(0f, 0f);
                sourceSize.anchorMax = new Vector2(1f, 1f);
                sourceSize.offsetMin = new Vector2(0, 0);
                sourceSize.offsetMax = new Vector2(0, 0);
            }
            else
            {
                float slearpT = float.IsNaN(menuPopUp.buttons.Count / 10) ? 0 : (float)menuPopUp.buttons.Count / (float)10;
                sourceSize.anchorMin = new Vector2(.5f, .5f);
                sourceSize.anchorMax = new Vector2(.5f, .5f);
                sourceSize.sizeDelta = new Vector2(Mathf.Lerp(1000, 1500, slearpT), 500);
            }
            //if (System.IO.File.Exists(prefabLoc))
            //{


            //    using (var editingScope = new PrefabUtility.EditPrefabContentsScope(prefabLoc))
            //    {
            //        var prefabRoot = editingScope.prefabContentsRoot;

            //        for (int i = prefabRoot.transform.childCount - 1; i >= buttonAmount; i--)
            //        {
            //            menuPopUp.buttons.Remove(prefabRoot.transform.GetChild(i).GetComponent<Button>());

            //            Object.DestroyImmediate(prefabRoot.transform.GetChild(i).gameObject);

            //        }
            //        Debug.Log(prefabRoot.transform.childCount);

            //        for (int i = prefabRoot.transform.childCount; i < buttonAmount; i++)
            //        {
            //            var go = new GameObject("Yes", typeof(Button), typeof(Image));
            //            var button = go.GetComponent<Button>();
            //            go.transform.parent = prefabRoot.transform;
            //            menuPopUp.buttons.Add(button);
            //        }
            //        //PrefabUtility.SaveAsPrefabAsset(source, prefabLoc);


            //    }

            //}


            //else
            //{
            //}


        }
        //    private void ClearContent()
        //    {
        //        foreach (var item in buttonObjects)
        //        {
        //            item.Update();

        //            DestroyImmediate(item.targetObject);
        //            item.ApplyModifiedProperties();
        //        }
        //    }
        //    private void SetUpYesNoPopUp()
        //    {
        //        ClearContent();

        //        ButtonSetupWithText("Yes-Button", "Yes-Text", new Vector3(300, 0, 0));

        //        ButtonSetupWithText("No-Button", "No-Text", new Vector3(-300, 0, 0));
        //        SetUpButtonProps();
        //    }

        //    private void SetOkPopUp()
        //    {
        //        ClearContent();

        //        ButtonSetupWithText("Ok-Button", "Ok-Text", new Vector3(0, 0, 0));

        //        SetUpButtonProps();
        //    }

        //    private void ButtonSetupWithText(string buttonName, string textName,Vector3 localPos= new Vector3())
        //    {
        //        Button button = new GameObject(buttonName, typeof(Image)).AddComponent<Button>(); 
        //        button.transform.parent = menuPopUp.transform;
        //        var buttonTran = button.GetComponent<RectTransform>();
        //        buttonTran.sizeDelta = new Vector2(300, 100);
        //        buttonTran.transform.localScale = new Vector3(1, 1, 1);
        //        Text text = new GameObject(textName).AddComponent<Text>();
        //        text.transform.parent = button.transform;
        //        text.transform.localScale = new Vector3(1, 1, 1);
        //        var okTran = text.gameObject.GetComponent<RectTransform>();
        //        okTran.anchorMin = new Vector2(0, 0);
        //        okTran.anchorMax = new Vector2(1, 1);
        //        okTran.localPosition = new Vector2(0, 0);

        //        text.resizeTextForBestFit = true;
        //        text.text = "Default Text";
        //        text.alignment = TextAnchor.MiddleCenter;

        //        text.color = Color.black;
        //        button.transform.localPosition = localPos;

        //    }
        //    private void SetUpButtonProps()
        //    {

        //        buttonObjects = new List<SerializedObject>();
        //        textObjects = new List<SerializedObject>();

        //        for (int i = 0; i < buttons_prop.arraySize; i++)
        //        {
        //            SerializedObject curElement = new SerializedObject(buttons_prop.GetArrayElementAtIndex(i).objectReferenceValue);
        //            buttonObjects.Add(curElement);
        //        }

        //        for (int i = 0; i < texts_prop.arraySize; i++)
        //        {
        //            SerializedObject curElement = new SerializedObject(texts_prop.GetArrayElementAtIndex(i).objectReferenceValue);
        //            textObjects.Add(curElement);
        //        }
        //    }
    }
}
