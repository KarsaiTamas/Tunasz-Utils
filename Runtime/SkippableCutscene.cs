using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TunaszUtils
{
    public class SkippableCutscene : Cutscene
    {
        public Material skipMat;
        float skipCutSceeneTimer;
        public Text skipText;
        public float skipCutSceeneTimerMax;
        public KeyCode skipKey;
        protected override void Start()
        {
            skipMat.SetFloat("_HideAmount", 0);
            skipCutSceeneTimer = skipCutSceeneTimerMax;
            if (skipKey.ToString().Equals("Return"))
                skipText.text = "Hold Enter to skip cutsceen";
            else
                skipText.text = "Hold " + skipKey.ToString() + " to skip cutsceen";
            base.Start();
        }
        protected override void Update()
        {
            if (Input.GetKey(skipKey))
            {
                skipMat.SetFloat("_HideAmount", skipCutSceeneTimer / skipCutSceeneTimerMax);
                if (skipCutSceeneTimer > 0)
                {
                    skipCutSceeneTimer -= Time.unscaledDeltaTime;
                    base.Update();
                    return;
                }
                timer = 0;
            }
            else
            {
                skipMat.SetFloat("_HideAmount", 0);
            }
            base.Update();
        }
    }
}
