using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabPlacer : MonoBehaviour
{
    [SerializeField]
    private bool snapToGrid;
    [SerializeField]
    private Vector3 snapping;
    private GameObject prefabPreview;
    [SerializeField]
    private GameObject prefabToShow;
    [SerializeField]
    private KeyCode xSnap=KeyCode.Q, ySnap=KeyCode.W, zSnap=KeyCode.E;
    private bool isXSnap,isYSnap,isZSnap;
    [SerializeField]
    private LayerMask prefabToPlaceOnto;
    [SerializeField]
    private Transform parent;
    // Update is called once per frame

    void OnSceneGUI(SceneView sceneView)
    {
        SceneView.RepaintAll();

        Event e = Event.current;
        if (Event.current.type == EventType.MouseDown && e.button == 0) Event.current.Use();
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        int cID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(cID))
        {
            case EventType.MouseDown:
                Event.current.Use();
                break;
            case EventType.MouseUp:
                if (e.button == 0)
                {
                    var pref = Instantiate(prefabToShow, parent);
                    pref.transform.position = prefabPreview.transform.position;
                }
                if (e.button == 1)
                {
                    
                } 
                Event.current.Use();
                break;
            default:
                break;
        }
        if (e.shift)
        {
            
            switch (e.type)
            {
                case EventType.MouseDown:
                    print("asd");
                    break; 
                case EventType.MouseMove:
                    break; 
                case EventType.KeyDown:

                    if (e.keyCode == xSnap) isXSnap = true;
                    if (e.keyCode == ySnap) isYSnap = true;
                    if (e.keyCode == zSnap) isZSnap = true;

                    



                    break;
                case EventType.KeyUp:

                    if (e.keyCode == xSnap) isXSnap = false;
                    if (e.keyCode == ySnap) isYSnap = false;
                    if (e.keyCode == zSnap) isZSnap = false; 
                    break;
                case EventType.ScrollWheel:
                    print(isXSnap);
                    Vector3 extra = Vector3.zero;
                    if (e.delta.y < 0)
                        extra = new Vector3(isXSnap ? 0.1f : 0, isYSnap ? 0.1f : 0, isZSnap ? 0.1f : 0);
                    else
                        extra = new Vector3(isXSnap ? -0.1f : 0, isYSnap ? -0.1f : 0, isZSnap ? -0.1f : 0);
                    snapping += extra;

                    break;

                default:
                    break;
            }
        }

        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
         
        if (Physics.Raycast(ray, out var hit, 100,prefabToPlaceOnto))
        {
            if (snapToGrid)
            {
                var snappingPos= new Vector3(
                    Mathf.Round( hit.point.x/snapping.x)*snapping.x,
                    Mathf.Round( hit.point.y/snapping.y)*snapping.y,
                    Mathf.Round( hit.point.z/snapping.z)*snapping.z);
                prefabPreview.transform.position = snappingPos;
            }
            else prefabPreview.transform.position = hit.point;
        }
        if (Event.current.type == EventType.ScrollWheel) Event.current.Use();

    }
    private void OnEnable()
    {
        if (!parent) parent = transform;
        
        isXSnap = false;
        isYSnap = false;
        isZSnap = false;
        prefabPreview=Instantiate(prefabToShow,transform);
        ChangeEveryLayer(prefabPreview.transform,0);
        SceneView.duringSceneGui += OnSceneGUI;
    } 
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        DestroyImmediate(prefabPreview);

    }
    //put this into a different class
    private void ChangeEveryLayer(Transform transform,int layer)
    {
        transform.gameObject.layer = layer;
        foreach (Transform item in transform)
        {
            item.gameObject.layer = layer;
            ChangeEveryLayer(item, layer);
        }
    }
}
