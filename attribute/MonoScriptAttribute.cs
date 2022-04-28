using System;
using UnityEngine;
namespace TunaszUtils
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        public System.Type type;
    }
}