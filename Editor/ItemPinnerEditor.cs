using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace TunaszUtils
{

    public class PrefabPinnerEditor: EditorWindow
    {
        public PinData pinData;

        void OpenItemList()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "asset");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                pinData = AssetDatabase.LoadAssetAtPath(relPath, typeof(PinData)) as PinData;
                if (pinData.itemPins == null)
                    pinData.itemPins = new List<ItemPin>();
                if (pinData)
                {
                    EditorPrefs.SetString(EditorDataPath(), relPath);
                }
            }
        }
        protected  string EditorDataPath()
        {
            return "PrefabPinnerEditorPath";
        }
        protected  void AddItems()
        { 
            if (GUILayout.Button("Add prefabs to this Pin"))
            {
              string path=  EditorUtility.OpenFolderPanel("Select location of prefabs", "", "");
                if (path == "") return;
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                    if (file.EndsWith(".prefab"))
                        pinData.itemPins[pinData.selectedPin].pinContent.Add(file);
            }
        } 
        protected  void OnGUI()
        {
            if (pinData == null)
            {
                if (GUILayout.Button("Load Pin Data"))
                {
                    OpenItemList();
                }

                return;
            }
            if (GUILayout.Button("Add Pin"))
            {
                pinData.itemPins.Add(new ItemPin("pin " + pinData.itemPins.Count));
            }
            GUI.enabled = pinData.selectedPin != 0;

            if (GUILayout.Button("Remove Pin"))
            {
                pinData.itemPins.RemoveAt(pinData.selectedPin);

            }
            GUI.enabled = true;

            if (GUILayout.Button("Clear Pin"))
            {
                pinData.itemPins[pinData.selectedPin].pinContent.Clear();

            }

            pinData.selectedPin =
                EditorGUILayout.Popup(
                pinData.selectedPin,
                pinData.itemPins.Select(x => x.pinName).ToArray());

            pinData.itemPins[pinData.selectedPin].pinName =
                EditorGUILayout.TextField(pinData.itemPins[pinData.selectedPin].pinName, GUILayout.Width(75));
            //EditorGUILayout.LabelField(pin.pinName, GUILayout.Width(75));

            AddItems();
            foreach (var item in pinData.itemPins[pinData.selectedPin].pinContent)
            {
                EditorGUILayout.LabelField(item, GUILayout.Width(75));
                //show visuals
            }
             
        }
    }
    public class ItemPinnerEditor : EditorWindow
    {
        
        public PinData pinData; 
        protected virtual string EditorDataPath()
        {
            return "ObjectPath";
        }
        protected virtual void OnEnable()
        {
            
            if (EditorPrefs.HasKey(EditorDataPath()))
            {
                string objectPath = EditorPrefs.GetString(EditorDataPath());
                pinData = AssetDatabase.LoadAssetAtPath(objectPath, typeof(PinData)) as PinData;
            }

        }
        protected virtual void OnGUI()
        {
            if (pinData==null)
            {
                if(GUILayout.Button("Load Pin Data"))
                {
                    OpenItemList();
                }

                return;
            } 
            if (GUILayout.Button("Add Pin"))
            {
                pinData.itemPins.Add(new ItemPin("pin " + pinData.itemPins.Count)); 
            }
            GUI.enabled = pinData.selectedPin != 0;
             
            if (GUILayout.Button("Remove Pin"))
            {
                pinData.itemPins.RemoveAt(pinData.selectedPin); 

            }
            GUI.enabled = true;

            if (GUILayout.Button("Clear Pin"))
            {
                pinData.itemPins[pinData.selectedPin].pinContent.Clear(); 

            }

            pinData.selectedPin = 
                EditorGUILayout.Popup(
                pinData.selectedPin, 
                pinData.itemPins.Select(x=>x.pinName).ToArray());

            pinData.itemPins[pinData.selectedPin].pinName = 
                EditorGUILayout.TextField(pinData.itemPins[pinData.selectedPin].pinName, GUILayout.Width(75));
            //EditorGUILayout.LabelField(pin.pinName, GUILayout.Width(75));

            AddItems();
            foreach (var item in pinData.itemPins[pinData.selectedPin].pinContent)
            {
                EditorGUILayout.LabelField(item, GUILayout.Width(75));
                //show visuals
            }
              

        }
        void OpenItemList()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "asset");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                pinData = AssetDatabase.LoadAssetAtPath(relPath, typeof(PinData)) as PinData;
                if (pinData.itemPins == null)
                    pinData.itemPins = new List<ItemPin>();
                if (pinData)
                {
                    EditorPrefs.SetString(EditorDataPath(), relPath);
                }
            }
        }
        protected virtual void AddItems()
        {
            
        }
    }
}
