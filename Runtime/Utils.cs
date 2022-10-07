using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TunaszUtils
{
    public static class Utils 
    { 
        public static T RandomOption<T>(params T[] options)
        {
            return options[Random.Range(0, options.Length)];
        }

        public static T RandomOption<T>(this List<T> options)
        {
            return options[Random.Range(0, options.Count)];
        }
    }
}
