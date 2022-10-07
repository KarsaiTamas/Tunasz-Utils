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
        //public delegate Type StateName();
        //public StateName stateName;
        public int entityType;
        //Anything goes here which we want to save from an entity so we can load it back 
        // This goes to save data if we want to save and load animations and wait timings
        //other than that no reason to save it   
        public bool isToofarAway;

        public float waitTimer;
        protected virtual void Awake()
        {
            StateUpdateAction = () => { print("Update Action"); };
            StateFixedUpdateAction = () => { print("Fixed Update Action"); };
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
