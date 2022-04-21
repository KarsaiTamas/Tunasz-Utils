using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetLoading 
{
    // Got this code from here https://forum.unity.com/threads/loadallassetsatpath-not-working-or-im-using-it-wrong.110326/#post-7489573

    /// <summary>
    /// Loads in assets from a folder location
    /// </summary>
    /// <param name="path">The path to your assets</param>
    /// <param name="filter">Type here the file extensions which you are looking for f.e. "prefab"</param>
    /// <returns></returns>
    public static List<Object>  LoadAllAssetsAtPath(string path, params string[] filter)
    {

        List<Object> objects = new List<Object>();
        if (Directory.Exists(path))
        {
            string[] assets = Directory.GetFiles(path);
            foreach (string assetPath in assets)
            {
                foreach (var item in filter)
                {

                
                    if (assetPath.Contains(string.Concat(".",item)) && !assetPath.Contains(".meta"))
                    {
                        objects.Add(AssetDatabase.LoadMainAssetAtPath(assetPath));
                        Debug.Log("Loaded " + assetPath);
                    }
                }
            }
        }
        return objects;
    }
}
