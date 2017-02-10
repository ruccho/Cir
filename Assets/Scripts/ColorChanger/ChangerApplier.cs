using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChangerApplier {

    public static void TurnOn()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionCurves>().enabled = true;
    }
    public static void TurnOff()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionCurves>().enabled = false;
    }
}
