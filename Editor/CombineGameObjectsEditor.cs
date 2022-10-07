//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(CombineObjectsToOneBatch))]

//public class CombineGameObjectsEditor : Editor
//{
//    CombineObjectsToOneBatch batch;
//    private void OnEnable()
//    {
//        batch = target as CombineObjectsToOneBatch;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        if (GUILayout.Button("Combine Objects"))
//        {
//            batch.CombineObjectsEditor();
//        }
//        if (GUILayout.Button("Create Mesh"))
//        {
//            batch.CreateMesh();
//        }
//    }
//}
