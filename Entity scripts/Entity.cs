using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TunaszUtils
{
    /// <summary>
    /// Goes onto any gameobject which has a presence on the map
    /// </summary>

    //Remove comment from the required component
    //For 3d entities
    //[RequireComponent(typeof(Rigidbody))]
    //For 2d entities
    //[RequireComponent(typeof(Rigidbody2D))]
    public class Entity : MonoBehaviour
    {
        public delegate void StateScript();
        public StateScript StateUpdateAction;
        public StateScript StateFixedUpdateAction;
        public delegate Type StateName();
        public StateName stateName;
        public int entityType;
        //Anything goes here which we want to save from an entity so we can load it back 
        // This goes to save data if we want to save and load animations and wait timings
        //other than that no reason to save it   
        public virtual List<float> Stats { get; set; }
        public bool isToofarAway;

        public float waitTimer; 
        protected virtual void Awake()
        {
            StateUpdateAction = () => { print("Update Action"); };
            StateFixedUpdateAction = () => { print("Fixed Update Action"); };
        } 

        protected virtual void OnValidate()
        {
            var estats = StatsLength(StatEnumType()); 
            if (Stats.Count == estats.Count) return;

            if (Stats.Count > estats.Count)
            {
                while (Stats.Count > estats.Count)
                {
                    Stats.RemoveAt(Stats.Count - 1);
                }
                return;
            }
            while (Stats.Count < estats.Count)
            {
                Stats.Add(0);
            }
        }

        public float GetStat(Enum stat)
        { 
            return Stats[Convert.ToInt32(stat)];
        }
        public bool GetStatBool(Enum stat)
        {
            return Mathf.Approximately(Mathf.Min(Stats[Convert.ToInt32(stat)],1),1);
        }
        public int GetStatInt(Enum stat)
        {
            return Mathf.RoundToInt( Stats[Convert.ToInt32(stat)]);

        }
        public void SetStat(Enum stat, float value)
        {
            Stats[Convert.ToInt32(stat)] = value;
        }

        public void SetStatBool(Enum stat, bool value)
        {
            Stats[Convert.ToInt32(stat)] = value?1:0; 
        }
        protected List<string> StatsLength(Type e)
        { 
            return EnumExtenstions.GetWithOrder(e).ToList();
        }
        protected virtual Type StatEnumType()
        { 
            return null;
        }

        protected virtual void Die(bool isDestroy = false)
        {
            if (isDestroy)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void DoInteraction()
        {
            print("Did a default interaction");
        }
         
    }
}
