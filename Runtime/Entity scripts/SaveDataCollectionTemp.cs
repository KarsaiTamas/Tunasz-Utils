//using System.Collections;
//using System.Collections.Generic;
//using TunaszUtils;
//using UnityEngine;
 
//public class SaveDataCollection : MonoBehaviour
//{
//    /// <summary>
//    /// Call this instead of requesting the gameobject for it
//    /// </summary>
//    public static SaveDataCollection instance;
    
//    /// <summary>
//    /// This should contain every data which you wish to save and load
//    /// </summary>
//    public SaveData dataToSave;

//    private void Awake()
//    {
//        instance = this;
//    }

         
//    public void SaveData(string saveSlotName)
//    {
//        SLHandler.Save(dataToSave,saveSlotName);
//    }
//    public void LoadData(string saveSlotName)
//    {
//        dataToSave= SLHandler.Load(dataToSave,saveSlotName);

//    }

     
//}
