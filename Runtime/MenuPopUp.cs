 
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TunaszUtils
{ 
    public class PopUpData
    {
        public UnityAction action;
        public string buttonText;

        public PopUpData(UnityAction action, string buttonText)
        {
            this.action = action;
            this.buttonText = buttonText;
        }
    } 
    [RequireComponent(typeof(Image))]
    public class MenuPopUp : MonoBehaviour
    { 
        public Text textMessage;
        public List< Button> buttons;
        public List<Text> texts;
        [Range(0,10)]
        public int buttonAmount;
        public bool setDefaultValues;
        public bool save;
        public bool coverScreen;
        private void OnValidate()
        {
            
            if (setDefaultValues)
            {
                var rect = gameObject.GetComponent<RectTransform>();
                rect.localPosition = Vector3.zero;
                rect.sizeDelta = new Vector2(1000f, 500f);
                rect.localScale = Vector3.one;
                
                setDefaultValues = false;
                
            }
            buttons = new List<Button>();
            texts = new List<Text>();
            foreach (Transform item in transform)
            {
                if (item.name.Contains("Message-Text")) continue;
                
                buttons.Add(item.GetComponent<Button>());
                if (item.childCount!=0)
                {
                    texts.Add(item.GetChild(0).GetComponent<Text>());

                }
            }


            //if (name.Contains("-Pop Up"))
            //{
            //    name = name.Substring(0, name.Length - 7) + "-Pop Up";
            //}
            //else
            //{
            //    name = name + "-Pop Up";

            //}
            ////create the default text message
            //if (textMessage == null)
            //{
            //    textMessage = new GameObject("Message-Text").AddComponent<Text>();
            //    textMessage.transform.parent = transform;
            //    textMessage.transform.localScale = new Vector3(1, 1, 1);
            //    var tran = textMessage.GetComponent<RectTransform>();
            //    tran.localPosition = new Vector3(0, 150, 0);
            //    tran.sizeDelta = new Vector2(300, 100);
            //    textMessage.resizeTextForBestFit = true;
            //    textMessage.alignment = TextAnchor.MiddleCenter;
            //    textMessage.text = "Default text";

            //}
            //switch (ePopUp)
            //{
            //    case PopUpType.YesNo:
            //        //if we changed popup type than clear current buttons and texts
            //        if (!PopUpClear("Yes-Button", "No-Button")) return;
            //        //Create new buttons

            //        ButtonSetupWithText("Yes-Button", "Yes-Text");
            //        buttons[0].transform.localPosition = new Vector3(300, 0, 0);

            //        ButtonSetupWithText("No-Button", "No-Text");
            //        buttons[1].transform.localPosition = new Vector3(-300, 0, 0);
            //        break;
            //    case PopUpType.Ok:
            //        if (!PopUpClear("Ok-Button")) return;


            //        ButtonSetupWithText("Ok-Button", "Ok-Text");
            //        buttons[0].transform.localPosition = new Vector3(0, 0, 0);
            //        break;
            //    default:
            //        break;
            //}

            //isChanged = true;
        }

        /// <summary>
        /// Destroys and clears the buttons when switching between popup types
        /// </summary>
        /// <param name="options"></param>
        /// <returns>if false shouldn't clear popups</returns>
        //private bool PopUpClear(params string[] options)
        //{
        //    if (buttons == null || buttons.Count == 0)
        //    { 
        //        buttons = new List<Button>();
        //        texts = new List<Text>();
        //        return true; 

        //    }
        //    var opList = options.ToList();
        //    buttons.Clear();
        //    texts.Clear();
        //    foreach (Transform item in transform)
        //    {
        //        if (item.name.Contains("Button"))
        //        {
        //            StartCoroutine(DeleteObject(item.gameObject));
        //        }

        //    }
        //    //foreach (var item in buttons)
        //    //{
        //    //    StartCoroutine(DeleteObject(item.gameObject));
        //    //    //DestroyImmediate(item.gameObject);
        //    //}

        //    return true;
        //}
        /// <summary>
        /// Makes a default button with a text and names when switching between popup types
        /// </summary> 
        /// <param name="buttonName"></param>
        /// <param name="textName"></param>
        //private void ButtonSetupWithText(string buttonName,string textName)
        //{
        //    Button button = new GameObject(buttonName,typeof(Image)).AddComponent<Button>();
        //    StageUtility.PlaceGameObjectInCurrentStage(button.gameObject);
        //    button.transform.parent = transform;
        //    var buttonTran = button.GetComponent<RectTransform>();
        //    buttonTran.transform.localScale = new Vector3(1, 1, 1);
        //    buttonTran.sizeDelta = new Vector2(300,100);
        //    Text text = new GameObject(textName).AddComponent<Text>();
        //    text.transform.parent = button.transform;
        //    text.transform.localScale = new Vector3(1, 1, 1);
        //    var okTran = text.gameObject.GetComponent<RectTransform>();
        //    okTran.anchorMin = new Vector2(0, 0);
        //    okTran.anchorMax = new Vector2(1, 1);
        //    okTran.localPosition = new Vector2(0, 0);

        //    text.resizeTextForBestFit = true;
        //    text.text = "Default Text";
        //    text.alignment = TextAnchor.MiddleCenter;

        //    text.color = Color.black;
        //    buttons.Add(button);
        //    texts.Add(text);
        //} 
        //private void ChangePopUp()
        //{


        //}
        //public IEnumerator DeleteObject(GameObject gameObject)
        //{
        //    yield return new WaitForSeconds(0);
        //    DestroyImmediate(gameObject); 
        //}
        /// <summary>
        /// We call this when we spawn down a popup
        /// </summary>
        /// <param name="message">Displays the main message</param>
        /// <param name="popUps">Data for each button includes button click action and button text</param>
        public void ShowPopUp(string message, params PopUpData[] popUps)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < popUps.Length; i++)
            {
                PopUpData item = popUps[i];
                buttons[i].onClick.AddListener(item.action);
                buttons[i].onClick.AddListener(() => { Destroy(gameObject); });
                texts[i].text = item.buttonText;
            }
            textMessage.text = message;

        }
    }
}
