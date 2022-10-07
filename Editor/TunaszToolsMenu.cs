using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    public static class TunaszToolsMenu
    {
        private static string packageRoot= "Packages/com.tunasz.utils/";
        [MenuItem("Tools/TunaszUtils/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            //Application.dataPath;
            CreateDirectories("_Project","Scripts","Scenes","Prefabs","Textures","Materials","Sounds","Shaders");
            CreateDirectories("_Project/Scripts", "Entities");
            CreateDirectories("_Project/Prefabs", "PopUps");

            Directory.CreateDirectory(Path.Combine(Application.dataPath, "_Project"));
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/TunaszUtils/Setup/Create Default Menu UI")]
        public static void CreateDefaultMenuUI()
        { 
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(packageRoot+"StartingUI/Default-UI.prefab", typeof(GameObject)));
        }
        //Entity scripts/
        public static void CopyEntityFiles(string root,params string[] dir)
        {
            string path= Path.Combine(packageRoot,root);
            foreach (var item in dir)
            {
                var fullPathFrom = Path.Combine(path, item);
                var fullPathTo = Path.Combine(path, item);
                FileUtil.CopyFileOrDirectory(fullPathFrom, fullPathTo);
            }

        }

        public static void CreateDirectories(string root,params string[]dir)
        {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var item in dir)
            {
                Directory.CreateDirectory(Path.Combine(fullPath, item));
            }
        }
          
        [MenuItem("GameObject/TunaszUtils/UI/Panel", priority = 7)] 
        public static void AddPanel()
        {
            var go = new GameObject("Default - Panel",typeof(MainMenuPanel),typeof(Image));
            go.transform.SetParent( Selection.activeTransform); 
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            var startingTransform= go.GetComponent<RectTransform>();
            startingTransform.anchorMin = new Vector2(0, 0);
            startingTransform.anchorMax = new Vector2(1, 1);
            startingTransform.pivot= new Vector2(0.5f, 0.5f);
            var goImage = go.GetComponent<Image>();
            goImage.color = new Color(100, 60, 40, 30);
            
            var panel = go.GetComponent<MainMenuPanel>();
            panel.childButtonHolder = new List<Transform>();
            panel.childButtonHolder.Add(go.transform);

        }
        [MenuItem("GameObject/TunaszUtils/UI/PopUp", priority = 9)] 
        public static void AddPopUp()
        {
            var go = new GameObject("Default - PopUp", typeof(Image), typeof(MenuPopUp));
             
            var target_component = go.GetComponent<MenuPopUp>();
            UnityEditorInternal.ComponentUtility.MoveComponentUp(target_component);
            UnityEditorInternal.ComponentUtility.MoveComponentUp(target_component);
            go.GetComponent<Image>().color = Color.cyan;
            go.transform.SetParent( Selection.activeTransform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
        }
        [MenuItem("GameObject/TunaszUtils/UI/Button", priority = 8)]
        public static void AddButton()
        {
            var go = new GameObject("Default - Button", typeof(MainMenuButton), typeof(Image),typeof(Button));
            var target_component = go.GetComponent<MainMenuButton>();
            var size = go.GetComponent<RectTransform>();

            size.anchorMin = new Vector2(.5f, .5f);
            size.anchorMax = new Vector2(.5f, .5f);
            size.sizeDelta = new Vector2(300, 200);
            var textMessage = new GameObject("Default Button-Text").AddComponent<Text>();
            textMessage.transform.SetParent(go.transform);
            textMessage.transform.localScale = new Vector3(1, 1, 1);
            var okTran = textMessage.gameObject.GetComponent<RectTransform>();
            okTran.anchorMin = new Vector2(0, 0);
            okTran.anchorMax = new Vector2(1, 1);
            okTran.localPosition = new Vector2(0, 0);
            okTran.offsetMin = new Vector2(0, 0);
            okTran.offsetMax = new Vector2(0, 0);
            textMessage.resizeTextForBestFit = true;
            textMessage.text = "Default Text";
            textMessage.alignment = TextAnchor.MiddleCenter;

            textMessage.color = Color.black;
            //menuPopUp.textMessage = textMessage;

            UnityEditorInternal.ComponentUtility.MoveComponentUp(target_component);
            UnityEditorInternal.ComponentUtility.MoveComponentUp(target_component);
            go.transform.SetParent( Selection.activeTransform);
            
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            target_component.OnValidate();
            if (Selection.activeTransform.TryGetComponent(out MainMenuPanel panel))
            {
                panel.OnValidate();
                
            }
        }
        public static MainMenuButton AddButton(Transform parent)
        {
            var go = new GameObject("Default - Button", typeof(MainMenuButton), typeof(Image), typeof(Button));
            var target_component = go.GetComponent<MainMenuButton>();
            var size = go.GetComponent<RectTransform>();

            size.anchorMin = new Vector2(.5f, .5f);
            size.anchorMax = new Vector2(.5f, .5f);
            size.sizeDelta = new Vector2(300, 200);
            var textMessage = new GameObject("Default Button-Text").AddComponent<Text>();
            textMessage.transform.SetParent(go.transform);
            textMessage.transform.localScale = new Vector3(1, 1, 1);
            var okTran = textMessage.gameObject.GetComponent<RectTransform>();
            okTran.anchorMin = new Vector2(0, 0);
            okTran.anchorMax = new Vector2(1, 1);
            okTran.localPosition = new Vector2(0, 0);
            okTran.offsetMin = new Vector2(0, 0);
            okTran.offsetMax = new Vector2(0, 0);
            textMessage.resizeTextForBestFit = true;
            textMessage.text = "Default Text";
            textMessage.alignment = TextAnchor.MiddleCenter;

            textMessage.color = Color.black;
            //menuPopUp.textMessage = textMessage;

            UnityEditorInternal.ComponentUtility.MoveComponentUp(target_component);
            UnityEditorInternal.ComponentUtility.MoveComponentUp(target_component);
            go.transform.SetParent(parent);

            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            target_component.OnValidate();
            if (parent.TryGetComponent(out MainMenuPanel panel))
            {
                panel.OnValidate();

            }
            return target_component;
        }
        [MenuItem("Tools/TunaszUtils/Script Reload", priority = 7)]
        public static void ForceScriptReload()
        { 
            CompilationPipeline.RequestScriptCompilation();
        }
        [MenuItem("Tools/TunaszUtils/Item Pinner/CreateSaveFile")]
        public static void CreateItemPinner()
        { 
            var path = EditorUtility.SaveFilePanelInProject(
            "Create Item Holder",
            "Default name",
            "asset",
            "Select Folder To save");
            string[] data = path.Split('/');
            string folderPath = "";
            for (int i = 1; i < data.Length-1; i++)
            {
                folderPath += data[i]+"/";
            }
            string objName = data[data.Length-1].Split('.')[0];

            PinData asset = ScriptableObject.CreateInstance<PinData>();
            asset = new PinData(new List<ItemPin> {new ItemPin("DefaultPin") });
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        }
        [MenuItem("Tools/TunaszUtils/Open Prefab Pin Window")] 
        public static void OpenItemPinner()
        {
            EditorWindow.CreateWindow<PrefabPinnerEditor>();
        }
    }
}
