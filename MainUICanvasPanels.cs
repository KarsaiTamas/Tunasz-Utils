using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TunaszUtils
{
    public class MainUICanvasPanels : MonoBehaviour
    {
        public static MainUICanvasPanels instance;
        public List<MainMenuPanel> panels;
        private int selectedPanel;
        //I can put events here
        public int SelectedPanel 
        {
            get => selectedPanel;
            set
            {
                panels[selectedPanel].gameObject.SetActive(false);
                panels[value].gameObject.SetActive(true);
                selectedPanel = value;

            }
        }

        private void Awake()
        {
            instance = this;

        }
        private void OnValidate()
        {
            instance = this;
            panels = new List<MainMenuPanel>();
            foreach (Transform item in transform)
            {
                panels.Add(item.GetComponent< MainMenuPanel>());
                    panels[panels.Count - 1].id = panels.Count - 1;
            }
        }
        public MainMenuPanel GetPanel(Enum panel)
        {
            return panels[Convert.ToInt32(panel)];
        
        }
        public void PanelSetActive(Enum panel,bool active)
        {
            panels[Convert.ToInt32(panel)].gameObject.SetActive(active);
        }

        public void ChangePanel(Enum panel)
        {
            SelectedPanel = Convert.ToInt32(panel);
        }
        public void ChangePanelCloseRest(Enum panel)
        {
            foreach (var item in panels)
            {
                item.gameObject.SetActive(false);
            }
            SelectedPanel = Convert.ToInt32(panel);

        }

    }
}
