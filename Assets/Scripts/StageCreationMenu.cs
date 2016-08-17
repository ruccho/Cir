using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageCreationMenu : MonoBehaviour {

    public GameObject NewStagePanel;
    public GameObject StageWidthText;
    public GameObject StageHeightText;
    public GameObject StageSizeErrorText;

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
            StageWidth = int.Parse(StageWidthText.GetComponent<Text>().text);
            StageHeight = int.Parse(StageHeightText.GetComponent<Text>().text);
            Debug.Log(StageWidth.ToString() + "x" + StageHeight.ToString());
        }catch (System.Exception)
        {
            return;
        }
        if(StageWidth < 2 || StageHeight < 2)
        {
            StageSizeErrorText.SetActive(true);
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
        string query = Query.generateQuery(StageText, "名前未設定", "説明未設定");
        Debug.Log(query);
        PlayerPrefs.SetString("CurrentEditingStageQuery", query);
        //PlayerPrefs.SetString("CurrentStageText", StageText);
        SceneManager.LoadScene("StageCreation");
    }
}
