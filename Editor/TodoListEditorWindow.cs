using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TunaszUtils
{
    [System.Serializable]
    public class ToDoListItem
    {  
        public Object asset;
        public string assetPath;
        public string content;
        public bool isDone;
        public bool isFolded;
        public List<ToDoListItem> listOfItems;
        public string currentlyTypingText;

        public ToDoListItem(string content)
        {
            this.content = content;
            isDone = false;
            isFolded = true;
            listOfItems = new List<ToDoListItem>();
        }
    }
    [System.Serializable]
    public class TodoWindowData
    {
        public List<ToDoListItem> listOfItems = new List<ToDoListItem>();

    }
    [System.Serializable]
    public class TodoWindowSaveData
    {
        public TodoWindowData data=new TodoWindowData();
    }
    public class TodoListEditorWindow : EditorWindow
    {
        static TodoWindowSaveData data=new TodoWindowSaveData();
        Vector2 scrollPos;
        string currentlyTypingText;
        [MenuItem("Window/To do list")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TodoListEditorWindow),false,"To Do List");  
            data = SLHandler.LoadEditor(data,"Todo");
        }
        private void OnGUI()
        { 

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel=0;
            RecursingTodo(data.data.listOfItems);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("New Item", GUILayout.Width(75));
            currentlyTypingText = EditorGUILayout.TextField(currentlyTypingText);
            if (GUILayout.Button("Add"))
            {
                if (currentlyTypingText!=null)
                {
                    data.data.listOfItems.Add(new ToDoListItem(currentlyTypingText));
                }
            }
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                SLHandler.SaveEditor(data, "Todo");

            }
            EditorGUILayout.EndScrollView();
        }
        private void OnEnable()
        { 
            data = SLHandler.LoadEditor(data,"Todo");
        }
        private void OnDisable()
        {
            SLHandler.SaveEditor(data, "Todo");
        }
        private void RecursingTodo(List<ToDoListItem> listOfItems,int indentLevel=0)
        {
            foreach (var item in listOfItems)
            {
                EditorGUI.indentLevel = indentLevel;
                item.isFolded = EditorGUILayout.Foldout(item.isFolded,item.content,true);
                GUILayout.Space(20);
                if (item.isFolded)
                {

                
                    if (item.assetPath!="" && item.asset==null && item.assetPath!=null)
                    {
                        item.asset = MonoScript.FindObjectOfType(System.Type.GetType(item.assetPath));
                    }
                    item.asset=EditorGUILayout.ObjectField(item.asset, typeof(MonoScript), true);
                    if (item.asset!=null)
                    {
                    MonoScript script = item.asset as MonoScript;

                    item.assetPath = script.GetClass().AssemblyQualifiedName;
                    }
                    EditorGUILayout.BeginHorizontal();
                    item.isDone = EditorGUILayout.Toggle(item.isDone,GUILayout.Width(50));
                    item.content = EditorGUILayout.TextArea(item.content,GUILayout.Height(100));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();

                    item.currentlyTypingText = EditorGUILayout.TextField(item.currentlyTypingText);
                    if (GUILayout.Button("Add"))
                    {
                        if (item.currentlyTypingText != null)
                        {
                            item.listOfItems.Add(new ToDoListItem(item.currentlyTypingText));
                         
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(5);
                    if (item.content=="" && item.listOfItems.Count==0)
                    {
                        listOfItems.Remove(item);
                        return;
                    }
                RecursingTodo(item.listOfItems, indentLevel+1);
                }
            }
            
        }
    }
}
