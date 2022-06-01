using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{
    public class SubtitleHandler : MonoBehaviour
    {
        public static SubtitleHandler instance;
        [SerializeField]
        private bool isSubTitleTurnedOnByPlayer;
        public bool subtitleTurnedOn;
        public bool waitForKeyPress;
        public KeyCode keyToPress;
        public bool IsSubTitleTurnedOnByPlayer
        {
            get => isSubTitleTurnedOnByPlayer;
            set
            {
                isSubTitleTurnedOnByPlayer = value;
                subtitle.gameObject.SetActive(value);
            }
        }
        private float timer;
        public Text subtitle;
        
        private void Awake()
        {
            instance = this;
            IsSubTitleTurnedOnByPlayer = IsSubTitleTurnedOnByPlayer;
            gameObject.SetActive(false);
        }

        public void SetSubtitle(string text,float time,KeyCode key=KeyCode.Return)
        {
            if (!isSubTitleTurnedOnByPlayer) return;
            subtitleTurnedOn = true; 
            keyToPress = key;
            subtitle.text = text;
            timer = time;
            gameObject.SetActive(true);
        }

        private void Update()
        { 
            if (waitForKeyPress)
            {
                if (Input.GetKeyDown(keyToPress))
                {

                    subtitle.text = "";
                    gameObject.SetActive(false);
                    subtitleTurnedOn = false; 
                    return;
                }
            }
            else
            {
                timer -= Time.unscaledDeltaTime;
            }
            
            if (timer<=0)
            {
                subtitle.text = "";
                subtitleTurnedOn = false;
                gameObject.SetActive(false);

            }
        }
    }
}
