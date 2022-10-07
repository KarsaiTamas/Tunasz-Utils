using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger 
{
    public static bool shouldLog;
    public static void Log(string message)
    {
        if (shouldLog)
        {
            Debug.Log(message);
        }
    }

}
