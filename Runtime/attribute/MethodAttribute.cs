using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace TunaszUtils
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MethodAttribute : PropertyAttribute
    {
        public GameObject methodsFromObject;
        public MethodInfo methodInfo;
        public int position;
        public string selectedMethod;
    }
}
