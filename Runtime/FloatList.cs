//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//namespace TunaszUtils
//{
//    [Serializable]
//    public class FloatList : List<float>
//    { 
//        [Method]
//        public string type;
//        public int GetInt(int index)
//        {
//            return (int)this[index];
//        }
//        public bool GetBool(int index)
//        {
//            return this[index]>0;
//        }
//        public void SetBool(int index,bool value)
//        {
//            this[index] = value ? 1 : 0;
//        }
//        public int GetInt(Enum index)
//        {
//            return (int)this[Convert.ToInt32(index)];
//        }
//        public bool GetBool(Enum index)
//        {
//            return this[Convert.ToInt32(index)] > 0;
//        }
//        public void SetBool(Enum index, bool value)
//        {
//            this[Convert.ToInt32(index)] = value ? 1 : 0;
//        }

//        public void Set(Enum index,float value)
//        {
//            this[Convert.ToInt32(index)]= value;
//        }
//    }
//}
