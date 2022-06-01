using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    public class Cutscene : MonoBehaviour
    {
        //this will be on cutsceene prefabs
        //cutsceene to show
        //sound
        public delegate void CutsceenTodo();
        public CutsceenTodo StartAction;
        public CutsceenTodo FinishAction;
        public Canvas canvas;
        protected float timer;
        public float timerMax;
        protected virtual void Start()
        {
            if (StartAction==null)
            {
                StartAction = () => { Debug.Log("Started The cutScene"); };
            }
            if (FinishAction==null)
            {
                FinishAction = ()=> { Debug.Log("Finished Action"); };
            }
            canvas.worldCamera = Camera.main;
            timer = timerMax;
            StartAction();
        }
        protected virtual void Update()
        {
            if (timer>0)
            { 
                timer -= Time.unscaledDeltaTime;
                    return;
            }
            FinishAction();  
            // do action here
            Destroy(gameObject);
        }
    }
}
