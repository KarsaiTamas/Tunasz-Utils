using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace TunaszUtils
{ 
    [System.Serializable]
    public class ButtonActionValue
    {
        public int selectedValue;
        public string value;
    }
    [System.Serializable]
    public class ButtonAction
    {

        public GameObject objectScript;
        public string search;
        public int selectedScript;
        public bool showButtonAction;
        public string actionName;
        //public int selectedAction;
        public List<ButtonActionValue> values;
        public List<MethodInfo> methodInfos; 
        public ButtonAction()
        {
            search = "";
            selectedScript = 0;
            objectScript = null;
            values = new List<ButtonActionValue>();
            methodInfos = new List<MethodInfo>();
        }

    }
    public class MainMenuButton : MonoBehaviour
    {
        public List<ButtonAction> actionsToPerform;
        public Button button;
        public Text buttonText;
        public string buttonName;
        public string textName;
        public string textString;
        public bool showButton;
        public Rect rect=new Rect(Vector2.zero,new Vector2( 300,100));
        public FontData textFont;

        private void Awake()
        { 
            for (int i = 0; i < actionsToPerform.Count; i++)
            {
                actionsToPerform[i].methodInfos = new List<MethodInfo>();
                if (actionsToPerform[i].objectScript != null)
                {
                    var mbs = (actionsToPerform[i].objectScript).GetComponents<MonoBehaviour>();
                    foreach (var mb in mbs)
                    { 
                        actionsToPerform[i].methodInfos.AddRange(mb.GetMethods().Where(x => x.Name.Contains(actionsToPerform[i].search)).ToList());
                    }
                }
            } 

            for (int j = 0; j < actionsToPerform.Count; j++)
            {
                var filteredMethods = actionsToPerform[j].methodInfos; 
                var methodInfo = filteredMethods[actionsToPerform[j].selectedScript];
                var par = filteredMethods[actionsToPerform[j].selectedScript].GetParameters();
                List<object> datas=new List<object>();
                for (int i1 = 0; i1 < par.Length; i1++)
                { 
                    ParameterInfo item = par[i1];
                    if (item.ParameterType == typeof(int))
                    {
                        datas.Add(Convert.ToInt32(actionsToPerform[j].values[i1].value));
                    }
                    else if (item.ParameterType.IsEnum)
                    {
                        var enumStringList = EnumExtenstions.GetWithOrder(item.ParameterType).ToArray();
                        var enumValue = Enum.Parse(item.ParameterType, actionsToPerform[j].values[i1].value);
                        datas.Add(enumValue);

                    }
                    else if (item.ParameterType == typeof(bool))
                    {
                        datas.Add(Convert.ToBoolean(actionsToPerform[j].values[i1].value));
                    }
                    else
                    {
                        datas.Add(actionsToPerform[j].values[i1].value);

                    }
                } 
                int selectedScript = j;
                button.onClick.AddListener(() => { InvokeSelectedScript(
                    actionsToPerform[selectedScript].objectScript.GetComponent(methodInfo.ReflectedType),
                    methodInfo.Name,
                    datas.ToArray()) ;
                });


            } 

        }
        public void AddScriptToButtonListener(object target,string methodName,params object[] parameters)
        {
            button.onClick.AddListener(() => { InvokeSelectedScript(target, methodName, parameters); });
        }

        public static void InvokeSelectedScript(object target, string methodName, params object[] parameters)
        {
            var mi = target.GetType().GetMethod(methodName); 
            if (mi == null) throw new ArgumentException("No method named '" + methodName + "' found on target.");
            var pinfos = mi.GetParameters();
            if (parameters.Length != pinfos.Length) throw new ArgumentException("Argument count mismatch.");

            for (int i = 0; i < pinfos.Length; i++)
            {
                if (!pinfos[i].ParameterType.IsInstanceOfType(parameters[i]))
                    parameters[i] = Convert.ChangeType(parameters[i], pinfos[i].ParameterType);
            }

            mi.Invoke(target, parameters);
        }
        public void OnValidate()
        {
            if (button==null)
            {
                button = GetComponent<Button>();
                buttonName = name;
                if (gameObject.TryGetComponent(out RectTransform rt2))
                {

                    rect.Set(rt2.rect.x, rt2.rect.y, rt2.rect.width, rt2.rect.height);
                
                }
            }
                 if (gameObject.TryGetComponent(out RectTransform rt))
                {
                    rt.sizeDelta= new Vector2(rect.width, rect.height);
                rt.anchoredPosition = new Vector2(rect.x, rect.y);
                }

                if (buttonText==null)
            {

                if(transform.childCount<=0 || !transform.GetChild(0).TryGetComponent(out buttonText)) return;
                if (buttonText == null || buttonText.font==null)
                {
                    return;
                }
                if (textFont==null)
                {
                    textFont = new FontData();
                }
                textFont.font = buttonText.font;
                textFont.bestFit = buttonText.resizeTextForBestFit;
                textFont.minSize = buttonText.resizeTextMinSize;
                textFont.maxSize = buttonText.resizeTextMaxSize;
                textFont.fontSize = buttonText.fontSize;
                textFont.lineSpacing = buttonText.lineSpacing;
                textFont.alignment = TextAnchor.MiddleCenter;
                if (textString=="")
                {
                    textString = name.Replace("-Button", "");
                    textString = name.Replace(" - Button", "");
                }
                textString = buttonText.text;
                textName= buttonText.name;


            }



            buttonText.text = textString;
            buttonText.font=textFont.font;
            buttonText.resizeTextForBestFit=textFont.bestFit;
            buttonText.resizeTextMinSize=textFont.minSize;
            buttonText.resizeTextMaxSize=textFont.maxSize;

            buttonText.fontSize=textFont.fontSize;
            buttonText.lineSpacing=textFont.lineSpacing;
            buttonText.fontStyle=textFont.fontStyle;
            
            buttonText.alignment= textFont.alignment;
            
            if (textName!="")
            {
                buttonText.name = textName.Replace(" - Label", "") + " - Label";

            }
            if (buttonName!="")
            {
                name = buttonName.Replace(" - Button","") +" - Button";

            }





            if (actionsToPerform==null)
            {
                actionsToPerform = new List<ButtonAction>();
            }
        }
    }
}
