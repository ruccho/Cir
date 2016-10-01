﻿using UnityEngine;
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
    public GameObject TurnCountText;
    public AudioClip ClickSound;
    public GameObject BrushPanel;

    public Sprite[] CreationSprites;
    /*int StageWidth;
    int StageHeight;*/
    string[,] StageMap;
    private StageStruct Stage;

    string ClickedName = "";
    GameObject obj;

    BrushModeType BrushMode = BrushModeType.Empty;
    //この順で！
    enum BrushModeType
    {
        Null, Empty, Filled, Start, Goal, Key, Door, Oneway_Up, Oneway_Left, Oneway_Down, Oneway_Right
    }

    // Use this for initialization
    void Start()
    {
        StageObject.GetComponent<StageConstructor>().initialize(MainCamera);
        MainCamera.GetComponent<Camera>().orthographicSize = 10;
        StageBorderObject.GetComponent<StageBorderConstructor>().Construct();
        string StageQuery = PlayerPrefs.GetString("CurrentEditingStageQuery");
        Stage = new StageStruct(StageQuery);
        

        //(0,0)が左下
        StageMap = new string[Stage.StageWidth, Stage.StageHeight];
        int readcounter = 0;
        for (int i = 0; i < Stage.StageHeight; i++)
        {
            for (int j = 0; j < Stage.StageWidth; j++)
            {
                StageMap[j, i] = Stage.StageBody.Substring(readcounter, 1);
                readcounter++;
            }
        }
        BrushMode = BrushModeType.Empty;
        RefreshBrushSwitch();

        TitleText.GetComponent<Text>().text = Stage.StageTitle;
        DescriptionText.GetComponent<Text>().text = Stage.StageDescription;
        TitleInputField.GetComponent<InputField>().text = Stage.StageTitle;
        DescriptionInputField.GetComponent<InputField>().text = Stage.StageDescription;
        if(Stage.StageTurnCount == 0)
        {
            TurnCountText.GetComponent<InputField>().text = "";
        }else
        {
            TurnCountText.GetComponent<InputField>().text = Stage.StageTurnCount.ToString();
        }
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


                StageMap[x, y] = ((int)BrushMode).ToString();
                if ((int)BrushMode >= 10){
                    switch ((int)BrushMode){
                        case 10:
                            StageMap[x, y] = "a";
                            break;
                    }
                }

                obj.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                obj.GetComponent<SpriteRenderer>().sprite = CreationSprites[(int)BrushMode];
                if ((int)BrushMode >= 7 && (int)BrushMode <= 10)
                {
                    //Onewayは共通Prefab/ Spriteなので条件で回転させる。
                    obj.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 90 * ((int)BrushMode - 7));
                }
                SaveStage();
            }
        }

    }

    /*public void SwitchBrush()
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
    }*/

    public void RefreshBrushSwitch()
    {
        BrushButtonImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        BrushButtonImage.GetComponent<UnityEngine.UI.Image>().sprite = CreationSprites[(int)BrushMode];
        if((int)BrushMode >= 7 && (int)BrushMode <= 10)
        {
            //Onewayは共通Prefab/ Spriteなので条件で回転させる。
            BrushButtonImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90 * ((int)BrushMode - 7));
        }
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

    public void OpenBrushMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        BrushPanel.SetActive(true);
    }
    public void CloseBrushMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        BrushPanel.SetActive(false);
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
                    saveStageText += StageMap[j, i];
            }
        }
        Stage.StageText = saveStageText;
        PlayerPrefs.SetString("CurrentEditingStageQuery", Query.generateQuery(saveStageText, Stage.StageTitle, Stage.StageDescription, Stage.StageTurnCount));
        //PlayerPrefs.SetInt("isTested", 0);
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

    public void SaveStageInfo()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        if (TurnCountText.GetComponent<InputField>().text == "")
        {
            Stage.StageTurnCount = 0;
        }
        else
        {
            if (int.Parse(TurnCountText.GetComponent<InputField>().text) == 0)
            {
                ErrorDialog.GetComponent<ErrorDialog>().OpenDialog("回転回数は設定する場合1以上の数値を設定してください。");
                return;
            }
            Stage.StageTurnCount = int.Parse(TurnCountText.GetComponent<InputField>().text);
        }
        Stage.StageTitle = TitleInputField.GetComponent<InputField>().text;
        Stage.StageDescription = DescriptionInputField.GetComponent<InputField>().text;
        TitleText.GetComponent<Text>().text = Stage.StageTitle;
        DescriptionText.GetComponent<Text>().text = Stage.StageDescription;
        SaveStage();
        InfoPanel.SetActive(false);
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
        /*
        if (PlayerPrefs.GetInt("isTested") == 0)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog("最後にもう一度テストプレイでクリアしてください。");
            return;
        }
        */
        UTJ.Board board = new UTJ.Board(Stage);
        if (board.isSolvable() != null)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(board.isSolvable());
            return;
        }

        SceneManager.LoadScene("Publish");

    }

    public void SetBrush(int ID)
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        BrushMode = (BrushModeType)ID;
        RefreshBrushSwitch();
        BrushPanel.SetActive(false);
    }

}
