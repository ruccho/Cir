﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageCreationMenu : MonoBehaviour {

    public GameObject NewStagePanel;
    public GameObject StageWidthText;
    public GameObject StageHeightText;
    public GameObject ErrorDialog;

    // Use this for initialization
    void Start ()
    {
        NewStagePanel.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenNewStagePanel()
    {
        NewStagePanel.SetActive(true);
    }

    public void CloseNewStagePanel()
    {
        NewStagePanel.SetActive(false);
    }

    public void GoContinue()
    {
        if (PlayerPrefs.GetString("CurrentEditingStageQuery") == "" || PlayerPrefs.GetString("CurrentEditingStageQuery") == null) return;
        SceneManager.LoadScene("StageCreation");
    }

    public void Back()
    {
        SceneManager.LoadScene("ShareModeMenu");
    }

    public void CreateNewStage()
    {
        int StageWidth;
        int StageHeight;
        try
        {
            StageWidth = int.Parse(StageWidthText.GetComponent<InputField>().text);
            StageHeight = int.Parse(StageHeightText.GetComponent<InputField>().text);
            Debug.Log(StageWidth.ToString() + "x" + StageHeight.ToString());
        }catch (System.Exception)
        {
            return;
        }
        if(StageWidth < 2 || StageHeight < 2 || StageWidth > 20 || StageHeight > 20)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog("各辺は2～20である必要があります");
            return;
        }
        //Generate Stage Text(Query-based)
        string StageText = "";
        if (StageWidth.ToString().Length == 1)
        {
            StageText += "0";
            StageText += StageWidth.ToString();
        }else
        {
            StageText += StageWidth.ToString();
        }
        for(int i = 0; i < StageWidth * StageHeight; i++)
        {
            StageText += "1";
        }
        string query = Query.generateQuery(StageText, "名前未設定", "説明未設定", 0);
        Debug.Log(query);
        PlayerPrefs.SetString("CurrentEditingStageQuery", query);
        //PlayerPrefs.SetString("CurrentStageText", StageText);
        SceneManager.LoadScene("StageCreation");
    }
}
