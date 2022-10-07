/**
 * LabeledArrayAttribute.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 28/12/17 (dd/mm/yy)
 * Reference from John Avery: https://forum.unity.com/threads/how-to-change-the-name-of-list-elements-in-the-inspector.448910/
 */

using UnityEngine;
using System;
using System.Reflection;

namespace TunaszUtils
{
    public class LabeledArrayAttribute : PropertyAttribute
    {
        public readonly string[] names;
        public LabeledArrayAttribute(string[] names) { this.names = names; }
        public LabeledArrayAttribute(Type enumType) { names = Enum.GetNames(enumType); }
        //public LabeledArrayAttribute(string enumType) { names = Enum.GetNames(Type.GetType(enumType)); }
        //public LabeledArrayAttribute(Type ClassType, string method)
        //{

        //    MethodInfo curMethod = ClassType.GetMethod(method);

        //    Type returnValue = (Type)curMethod.Invoke(ClassType, null);
        //    Debug.Log(returnValue);

        //    names = Enum.GetNames(returnValue.GetType());
        //}

    }
}