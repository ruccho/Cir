using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Collections;

public class CreationMoveButton : MonoBehaviour
{

    public bool isMousePressed;
    public bool isPressed;
    public bool pointerOn;
    //bool isActive;
    [SerializeField]
    MoveDirectionType MoveDirection;
    enum MoveDirectionType
    {
        Up, Left, Down, Right
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isPressed = isMousePressed && pointerOn;

        if (isPressed)
        {
            Vector2 stagePos = GameObject.Find("Stage").transform.position;
            if (MoveDirection == MoveDirectionType.Up) GameObject.Find("Stage").transform.position = new Vector2(stagePos.x, stagePos.y - 0.1f);
            if (MoveDirection == MoveDirectionType.Down) GameObject.Find("Stage").transform.position = new Vector2(stagePos.x, stagePos.y + 0.1f);
            if (MoveDirection == MoveDirectionType.Left) GameObject.Find("Stage").transform.position = new Vector2(stagePos.x + 0.1f, stagePos.y);
            if (MoveDirection == MoveDirectionType.Right) GameObject.Find("Stage").transform.position = new Vector2(stagePos.x - 0.1f, stagePos.y);

        }
    }

    public void PointerDown()
    {

        pointerOn = true;
        isMousePressed = true;
    }
    public void PointerExit()
    {
        pointerOn = false;
    }
    public void PointerUp()
    {

        isMousePressed = false;
    }
    public void PointerEnter()
    {
        pointerOn = true;
    }
    public void Drag()
    {

    }


}
