using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class Extensions
{
    public static List<MethodInfo> GetMethods(this MonoBehaviour mb, Type returnType, Type[] paramTypes, BindingFlags flags)
    {
        return mb.GetType().GetMethods(flags)
            .Where(m => m.ReturnType == returnType)
            .Select(m => new { m, Params = m.GetParameters() })
            .Where(x =>
            {
                return paramTypes == null ? // in case we want no params
                 x.Params.Length == 0 :
                 x.Params.Length == paramTypes.Length &&
                 x.Params.Where(p => paramTypes.Contains( p.ParameterType)).ToArray().Length==paramTypes.Length;
            })
            .Select(x => x.m)
            .ToList();
    }
    public static List<MethodInfo> GetMethods(this MonoBehaviour mb)
    {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default; 

        return mb.GetType().GetMethods(flags)
            .Select(m => new { m, Params = m.GetParameters() })
            .Select(x => x.m)
            .ToList();
    }

    public static List<MethodInfo> GetMethods(this GameObject go, Type returnType, Type[] paramTypes, BindingFlags flags)
    {
        var mbs = go.GetComponents<MonoBehaviour>();
        List<MethodInfo> list = new List<MethodInfo>();
        foreach (var mb in mbs)
        {
            list.AddRange(mb.GetMethods(returnType, paramTypes, flags));
        }
        return list;
    }
    public static List<MethodInfo> GetMethods(this GameObject go)
    {
        var mbs = go.GetComponents<MonoBehaviour>();
        List<MethodInfo> list = new List<MethodInfo>();
        foreach (var mb in mbs)
        {
            list.AddRange(mb.GetMethods());
        }
        return list;
    }
    public static void RepaintInspector(System.Type t)
    {
        Editor[] ed = (Editor[])Resources.FindObjectsOfTypeAll<Editor>();
        for (int i = 0; i < ed.Length; i++)
        {
            if (ed[i].GetType() == t)
            {
                ed[i].Repaint();
                return;
            }
        }
    }
}