using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    public class MainMenuPanel : MonoBehaviour
    {
        public int id;
        private int count;
        public List<MainMenuButton> childButtons;
        public List<Transform> childButtonHolder;

        public void OnValidate()
        {
            if (childButtonHolder==null || childButtonHolder.Count==0)
            {
                childButtonHolder.Add(transform);
            }
            if (childButtons==null)
            {
            childButtons = new List<MainMenuButton>();

            }
            else
            { 
                childButtons.Clear();
                count = 0;
            }
            
            foreach (Transform holder in childButtonHolder)
            {
                if (holder == null) continue; 
                foreach (Transform item in holder)
                {
                    if (item.TryGetComponent(out MainMenuButton button))
                    {
                        childButtons.Add(button);
                        count++;
                    }

                }
                
            }
        }

    }
}
