using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace TunaszUtils
{
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

    public static int GetInt(this List<float> list,int index)
    {
        return (int)list[index];
    } 

    public static bool GetBool(this List<float> list,int index)
    {
        return list[index] > 0;
    }
    public static void SetBool(this List<float> list,int index, bool value)
    {
        list[index] = value ? 1 : 0;
    }
    public static int GetInt(this List<float> list,Enum index)
    {
        return (int)list[Convert.ToInt32(index)];
    }
    public static T Get<T>(this List<T> list, Enum index)
    {
        return list[Convert.ToInt32(index)];
    }
    public static bool GetBool(this List<float> list,Enum index)
    {
        return list[Convert.ToInt32(index)] > 0;
    }
    public static void SetBool(this List<float> list,Enum index, bool value)
    {
        list[Convert.ToInt32(index)] = value ? 1 : 0;
    }

    public static void Set<T>(this List<T> list,Enum index, T value)
    {
        list[Convert.ToInt32(index)] = value;
    }
    /// <summary>
    /// Gives a list a default length based on the given enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listToChange"></param>
    /// <param name="enumType"></param>
    /// <param name="defaultValue"></param>
    public static void DefaultListSize<T>(this List<T> listToChange, Type enumType, T defaultValue)
    {

        var listLength = EnumExtenstions.GetWithOrder(enumType).ToList();
        if (listToChange.Count == listLength.Count) return;

        if (listToChange.Count > listLength.Count)
        {
            while (listToChange.Count > listLength.Count)
            {
                listToChange.RemoveAt(listLength.Count - 1);
            }
            return;
        }
        while (listToChange.Count < listLength.Count)
        {
            listToChange.Add(defaultValue);
        }
    }
    public static void DefaultListSize<T>(this List<T> listToChange, int amount, T defaultValue)
    {

        if (listToChange.Count == amount) return;

        if (listToChange.Count > amount)
        {
            while (listToChange.Count > amount)
            {
                listToChange.RemoveAt(amount - 1);
            }
            return;
        }
        while (listToChange.Count < amount)
        {
            listToChange.Add(defaultValue);
        }
    }

}}