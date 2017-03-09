using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleButton : Button, ICanvasRaycastFilter
{

    [SerializeField]
    float radius = 70f;
    public bool isCameraSpace;
    float scale = 0;

    protected override void Start()
    {
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        //if (scale == 0)
        {
            scale = GetComponentInParent<Canvas>().scaleFactor;
        }
        Vector2 localPos = Vector2.zero;


        if (isCameraSpace)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), transform.position, Camera.main, out localPos);
            Debug.Log("researching pos:(" + sp.ToString() + "),base pos:(" + (-(localPos * scale)).ToString());
            return Vector2.Distance(sp, (-(localPos * scale))) < radius * scale;
        }
        else
        {
            localPos = transform.position;

            Debug.Log("researching pos:(" + sp.ToString() + "),base pos:(" + localPos.ToString());
            return Vector2.Distance(sp, localPos) < radius * scale;
        }

        //return Vector2.Distance(sp, (-(localPos))) < radius;

        //RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), transform.position, Camera.main, out localPos);
    }
}