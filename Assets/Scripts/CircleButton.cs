using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleButton : Button, ICanvasRaycastFilter
{

    
    public float radius = 60f;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return Vector2.Distance(sp, transform.position) < radius;
    }
}