using System.IO; 
using UnityEngine;
namespace TunaszUtils
{
    /// <summary>
    /// Save Load Handler
    /// </summary>
    public static class SLHandler
{ 
        //Change this to where you want to save
        static string  path = Application.persistentDataPath ;
     
        /// <summary>
        /// Saves a json file to the path's location
        /// </summary>
        /// <typeparam name="T">Save type</typeparam>
        /// <param name="dataToSave">Value to save</param>
        /// <param name="saveName">Save file's name</param>
        public static void Save<T>(T dataToSave,string saveName,string saveFolder= "\\SaveFiles\\")
        {
            string write=  JsonUtility.ToJson(dataToSave);
            try
            {
                //if a directory is missing than creat the missing path
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Debug.Log("File saved at: "+ path + saveFolder + saveName + ".json");
                File.WriteAllText(path+saveFolder +saveName + ".json", write);
            }
            catch (IOException)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves a json file to the path's location
        /// </summary>
        /// <typeparam name="T">Save type</typeparam>
        /// <param name="dataToSave">Value to save</param>
        /// <param name="saveName">Save file's name</param>
        /// <param name="exceptionMessage">Returns log messages</param>
        public static void Save<T>(T dataToSave, string saveName, out string exceptionMessage, string saveFolder = "\\SaveFiles\\")
        {
            string write = JsonUtility.ToJson(dataToSave);
            try
            {
                //if a directory is missing than creat the missing path
                if (!Directory.Exists(path+ saveFolder))
                {
                    Directory.CreateDirectory(path+ saveFolder);
                }
                File.WriteAllText(path+ saveFolder + saveName + ".json", write); 
                    exceptionMessage = saveName + " saved successfully";
            }
            catch (IOException e)
            {
                exceptionMessage = e.Message;
            }
        } 
        /// <summary>
        /// Loads a json file from the path's location
        /// </summary>
        /// <typeparam name="T">Load type</typeparam>
        /// <param name="defaultData">Default value in case of an error</param>
        /// <param name="saveName">Load file's name</param>
        /// <returns>Save data from file</returns>
        public static T Load<T>(T defaultData, string saveName, string saveFolder = "\\SaveFiles\\")
        {
            
            try
            {
                string jsonValue= File.ReadAllText(path+ saveFolder + saveName + ".json");
                if (jsonValue.Equals("{}") || jsonValue.Equals("{ }"))
                {
                    return defaultData;
                } 
                T value = JsonUtility.FromJson<T>(jsonValue); 
                    if (value==null)
                    {
                        return defaultData;
                    }
                return value;
                }
            catch (IOException)
            {
                return defaultData;
            }
        }
        /// <summary>
        /// Saves a json file to the path's location
        /// </summary>
        /// <typeparam name="T">Save type</typeparam>
        /// <param name="dataToSave">Value to save</param>
        /// <param name="saveName">Save file's name</param>
        public static void SaveEditor<T>(T dataToSave, string saveName)
        {
            string write = JsonUtility.ToJson(dataToSave);
            try
            {
                //if a directory is missing than creat the missing path
                 
                File.WriteAllText(Application.dataPath+"\\" + saveName + ".json", write);
            }
            catch (IOException)
            {
                throw;
            }
        }
        /// <summary>
        /// Saves a json file to the path's location
        /// </summary>
        /// <typeparam name="T">Save type</typeparam>
        /// <param name="dataToSave">Value to save</param>
        /// <param name="saveName">Save file's name</param>
        public static void SaveEditor<T>(T dataToSave, string saveName, string saveFolder = "")
        {
            string write = JsonUtility.ToJson(dataToSave);
            try
            {
                //if a directory is missing than creat the missing path

                File.WriteAllText(Application.dataPath + "\\"+ saveFolder+"\\" + saveName + ".json", write);
            }
            catch (IOException)
            {
                throw;
            }
        }
        /// <summary>
        /// Loads a json file from the path's location
        /// </summary>
        /// <typeparam name="T">Load type</typeparam>
        /// <param name="defaultData">Default value in case of an error</param>
        /// <param name="saveName">Load file's name</param>
        /// <returns>Save data from file</returns>
        public static T LoadEditor<T>(T defaultData, string saveName)
        {

            try
            {
                string jsonValue = File.ReadAllText(Application.dataPath+"\\" + saveName + ".json");
                if (jsonValue.Equals("{}") || jsonValue.Equals("{ }"))
                {
                    return defaultData;
                }
                T value = JsonUtility.FromJson<T>(jsonValue);
                if (value == null)
                {
                    return defaultData;
                }
                return value;
            }
            catch (IOException)
            {
                return defaultData;
            }
        }
        /// <summary>
        /// Loads a json file from the path's location
        /// </summary>
        /// <typeparam name="T">Load type</typeparam>
        /// <param name="defaultData">Default value in case of an error</param>
        /// <param name="saveName">Load file's name</param>
        /// <returns>Save data from file</returns>
        public static T LoadEditor<T>(T defaultData, string saveName, string saveFolder = "")
        {

            try
            {
                string jsonValue = File.ReadAllText(Application.dataPath + "\\"+ saveFolder + "\\" + saveName + ".json");
                if (jsonValue.Equals("{}") || jsonValue.Equals("{ }"))
                {
                    return defaultData;
                }
                T value = JsonUtility.FromJson<T>(jsonValue);
                if (value == null)
                {
                    return defaultData;
                }
                return value;
            }
            catch (IOException)
            {
                return defaultData;
            }
        }
        /// <summary>
        /// Loads a json file from the path's location
        /// </summary>
        /// <typeparam name="T">Load type</typeparam>
        /// <param name="defaultData">Default value in case of an error</param>
        /// <param name="saveName">Load file's name</param>
        /// <param name="exceptionMessage">Returns log messages</param>
        /// <returns>Save data from file</returns>
        public static T Load<T>(T defaultData, string saveName,out string exceptionMessage, string saveFolder = "\\SaveFiles\\")
        {
            try
            {

                string jsonValue = File.ReadAllText(path+ saveFolder + saveName + ".json");
                if (jsonValue.Equals("{}") || jsonValue.Equals("{ }"))
                {
                    exceptionMessage = saveName + " The file was empty so using default data.";

                    return defaultData;
                }
                T value = JsonUtility.FromJson<T>(jsonValue);
                exceptionMessage = saveName+" Loaded SuccessFully"; 
                    if (value == null)
                    {
                        exceptionMessage = saveName+" The data was null so using default data"; 
                        return defaultData;
                }
                    return value;
                }
            catch (IOException e)
            {
                exceptionMessage = e.Message;
                return defaultData;
            }
        }
    
        /// <summary>
        /// <br>Always be cautious when you delete files. Make sure everything is right when you decide to delete something.</br>
        /// <br>Deletes a json save file from the path's location</br>
        /// </summary>
        /// <param name="saveName">Name of the file which we want to delete</param>
        public static void DeleteSaveSlot(string saveName, string saveFolder = "\\SaveFiles\\")
        {
            try
            {
                File.Delete(path+ saveFolder + saveName + ".json"); 
            }
            catch (IOException)
            {
                throw;
            }
        }
    }
}
