using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageCreation : MonoBehaviour
{
    public GameObject StageObject;
    public GameObject StageBorderObject;
    public GameObject MainCamera;
    public GameObject BrushButtonImage;
    public GameObject MenuPanel;
    public GameObject InfoPanel;
    public GameObject TitleText;
    public GameObject DescriptionText;
    public GameObject TitleInputField;
    public GameObject DescriptionInputField;
    public GameObject ErrorDialog;
    public AudioClip ClickSound;

    public Sprite[] CreationSprites;
    /*int StageWidth;
    int StageHeight;*/
    int[,] StageMap;
    private StageStruct Stage;

    string ClickedName = "";
    GameObject obj;

    BrushModeType BrushMode = BrushModeType.Empty;
    //この順で！
    enum BrushModeType
    {
        Null, Empty, Filled, Start, Goal, Key, Door
    }

    // Use this for initialization
    void Start()
    {
        StageObject.GetComponent<StageConstructor>().initialize(MainCamera);
        StageBorderObject.GetComponent<StageBorderConstructor>().Construct();
        string StageQuery = PlayerPrefs.GetString("CurrentEditingStageQuery");
        Stage = new StageStruct(StageQuery);
        

        //(0,0)が左下
        StageMap = new int[Stage.StageWidth, Stage.StageHeight];
        int readcounter = 0;
        for (int i = 0; i < Stage.StageHeight; i++)
        {
            for (int j = 0; j < Stage.StageWidth; j++)
            {
                StageMap[j, i] = int.Parse(Stage.StageBody.Substring(readcounter, 1));
                readcounter++;
            }
        }
        BrushMode = BrushModeType.Empty;
        RefreshBrushSwitch();

        TitleText.GetComponent<Text>().text = Stage.StageTitle;
        DescriptionText.GetComponent<Text>().text = Stage.StageDescription;
        TitleInputField.GetComponent<InputField>().text = Stage.StageTitle;
        DescriptionInputField.GetComponent<InputField>().text = Stage.StageDescription;
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
                SaveStage();
            }
        }

    }

    public void SwitchBrush()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        if (((int)BrushMode) < 6)
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
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        MenuPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        MenuPanel.SetActive(false);
    }

    public void OpenInfoMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        InfoPanel.SetActive(true);
    }
    public void CloseInfoMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        InfoPanel.SetActive(false);
    }

    private void SaveStage()
    {
        string saveStageText = "";
        if (Stage.StageWidth.ToString().Length == 1)
        {
            saveStageText = "0" + Stage.StageWidth.ToString();

        }
        else
        {
            saveStageText = Stage.StageWidth.ToString();
        }
        for (int i = 0; i < Stage.StageHeight; i++)
        {
            for (int j = 0; j < Stage.StageWidth; j++)
            {
                saveStageText += StageMap[j, i].ToString();
            }
        }
        Stage.StageText = saveStageText;
        PlayerPrefs.SetString("CurrentEditingStageQuery", Query.generateQuery(saveStageText, Stage.StageTitle, Stage.StageDescription));
    }
    public void TestPlay()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        if (Query.checkStageCorrection(Stage, true) != "")
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(Query.checkStageCorrection(Stage, true));
            return;
        }
        PlayerPrefs.SetString("CurrentStageQuery", PlayerPrefs.GetString("CurrentEditingStageQuery"));
        //PlayerPrefs.SetString("CurrentStageText", Stage.StageText);
        PlayerPrefs.SetString("CurrentStageInfo", "TEST");
        SceneManager.LoadScene("Play");
    }

    public void RefreshStageTitleAndDescription()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Stage.StageTitle = TitleInputField.GetComponent<InputField>().text;
        Stage.StageDescription = DescriptionInputField.GetComponent<InputField>().text;
        TitleText.GetComponent<Text>().text = Stage.StageTitle;
        DescriptionText.GetComponent<Text>().text = Stage.StageDescription;
        SaveStage();
    }

    public void backToMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene("ShareModeMenu");
    }

    public void PublishButton()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        if (Query.checkStageCorrection(Stage, true) != "")
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(Query.checkStageCorrection(Stage, true));
            return;
        }
        if(Stage.StageTitle == "" || Stage.StageTitle == null)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog("タイトルもしくは説明が未入力です。");
            return;
        }
        SceneManager.LoadScene("Publish");

    }

}
