﻿using UnityEngine;

[RequireComponent (typeof(UnityStandardAssets.ImageEffects.ColorCorrectionCurves))]
public class ColorReplace : MonoBehaviour
{
    private Color ColorToReplaceObj = new Color(0, 0, 0);
    private Color ColorToReplaceBg = new Color(1, 1, 1);
    UnityStandardAssets.ImageEffects.ColorCorrectionCurves ColorCorrectionCurveScript;

    void Awake()
    {
        ColorCorrectionCurveScript = GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionCurves>();
        refreshColor();
    }

    void Update()
    {

    }


    public void refreshColor()
    {
        if (PlayerPrefs.HasKey("IsColorCustomized"))
        {
            if (PlayerPrefs.GetInt("IsColorCustomized") == 1)
            {
                //取得
                ColorToReplaceObj.r = PlayerPrefs.GetFloat("CObjRed");
                ColorToReplaceObj.g = PlayerPrefs.GetFloat("CObjGreen");
                ColorToReplaceObj.b = PlayerPrefs.GetFloat("CObjBlue");
                ColorToReplaceBg.r = PlayerPrefs.GetFloat("CBgRed");
                ColorToReplaceBg.g = PlayerPrefs.GetFloat("CBgGreen");
                ColorToReplaceBg.b = PlayerPrefs.GetFloat("CBgBlue");
            }
        }else
        {

        }
        //適用
        Debug.Log("Applying Color");
        Debug.Log("BG-RED: " + ColorToReplaceBg.r.ToString());
        ColorCorrectionCurveScript.redChannel = new AnimationCurve(new Keyframe(0f, ColorToReplaceObj.r), new Keyframe(1f, ColorToReplaceBg.r));
        ColorCorrectionCurveScript.greenChannel = new AnimationCurve(new Keyframe(0f, ColorToReplaceObj.g), new Keyframe(1f, ColorToReplaceBg.g));
        ColorCorrectionCurveScript.blueChannel = new AnimationCurve(new Keyframe(0f, ColorToReplaceObj.b), new Keyframe(1f, ColorToReplaceBg.b));
        ColorCorrectionCurveScript.UpdateParameters();
    }
}