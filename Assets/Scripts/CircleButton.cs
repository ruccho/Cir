using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleButton : Button, ICanvasRaycastFilter
{

    [SerializeField]
    float radius = 70f;
    
    protected override void Start()
    {
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {

        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), transform.position, Camera.main, out localPos);
        

        Debug.Log("researching pos:(" + sp.ToString() + "),base pos:(" + (-(localPos)).ToString());
        return Vector2.Distance(sp, (-(localPos))) < radius;
    }
}