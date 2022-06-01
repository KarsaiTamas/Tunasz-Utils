using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TunaszUtils
{
    public static class ListEdit
    {
        /// <summary>
        /// Gives a list a default length based on the given enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToChange"></param>
        /// <param name="enumType"></param>
        /// <param name="defaultValue"></param>
        public static void DefaultListSize<T>(List<T> listToChange,Type enumType,T defaultValue)
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
        public static void DefaultListSize<T>(List<T> listToChange, int amount, T defaultValue)
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


    }
}
