using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class StageCreation : MonoBehaviour
{
    public GameObject StageObject;
    public GameObject StageBorderObject;
    public GameObject MainCamera;
    public GameObject BrushButtonImage;
    public GameObject MenuPanel;
    public GameObject TitleText;
    public GameObject DescriptionText;
    public Sprite[] CreationSprites;
    int StageWidth;
    int StageHeight;
    int[,] StageMap;
    private StageStruct Stage;

    string ClickedName = "";
    GameObject obj;

    BrushModeType BrushMode = BrushModeType.Empty;
    //この順で！
    enum BrushModeType
    {
        Null, Empty, Filled, Start, Goal
    }

    // Use this for initialization
    void Start()
    {
        StageObject.GetComponent<StageConstructor>().initialize(MainCamera);
        StageBorderObject.GetComponent<StageBorderConstructor>().Construct();
        string StageQuery = PlayerPrefs.GetString("CurrentEditingStageQuery");
        Stage = new StageStruct(PlayerPrefs.GetString("CurrentEditingStageQuery"));
        string StageText = Stage.StageText;
        if (StageText == "") return;

        //先頭二文字から読み出して先頭の空白を削除し、サイズの純粋な数字を取得
        int parseresult;
        if (!(int.TryParse(StageText.Substring(0, 2), out parseresult))) return;
        StageWidth = parseresult;

        //構造文字列の長さがWidthで割り切れるかチェックし、格納
        if (StageText.Substring(2).Length % StageWidth != 0) return;
        StageHeight = StageText.Substring(2).Length / StageWidth;

        //(0,0)が左下
        StageMap = new int[StageWidth, StageHeight];
        int readcounter = 0;
        for (int i = 0; i < StageHeight; i++)
        {
            for (int j = 0; j < StageWidth; j++)
            {
                StageMap[j, i] = int.Parse(StageText.Substring(readcounter + 2, 1));
                readcounter++;
            }
        }
        BrushMode = BrushModeType.Empty;
        RefreshBrushSwitch();

        TitleText.GetComponent<Text>().text = Stage.StageTitle;
        TitleText.GetComponent<Text>().text = Stage.StageDescription;

        return;
    }

    // Update is called once per frame
    void Update()
    {
        //クリックしたオブジェクト名の取得
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            Vector3 aTapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

            if (aCollider2d)
            {
                obj = aCollider2d.transform.gameObject;
                ClickedName = obj.name;
                Debug.Log(ClickedName);
                if (ClickedName == "") return;
                //オブジェクト名から座標を取得
                string[] tmpStr = ClickedName.Split(',');
                if (tmpStr.Length != 2) return;
                int x = int.Parse(tmpStr[0]);
                int y = int.Parse(tmpStr[1]);
                StageMap[x, y] = (int)BrushMode;
                obj.GetComponent<SpriteRenderer>().sprite = CreationSprites[(int)BrushMode];
            }
        }

    }

    public void SwitchBrush()
    {
        if (((int)BrushMode) < 4)
        {
            BrushMode++;
        }
        else
        {
            BrushMode = BrushModeType.Empty;
        }
        RefreshBrushSwitch();
    }

    public void RefreshBrushSwitch()
    {
        BrushButtonImage.GetComponent<UnityEngine.UI.Image>().sprite = CreationSprites[(int)BrushMode];
    }



    private bool IsPointerOverUIObject()
    {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f 
        // the ray cast appears to require only eventData.position. 
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void OpenMenu()
    {
        MenuPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        MenuPanel.SetActive(false);
    }
}
