using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShareModeMenu : MonoBehaviour {

    public GameObject CodePanel;
    public GameObject ErrorDialog;
    public GameObject CodeInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ShareStageSelector");
    }

    public void Create()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageCreationMenu");
    }

    public void OpenCodePanel()
    {
        CodePanel.SetActive(true);

    }

    public void CloseCodePanel()
    {
        CodePanel.SetActive(false);
    }

    public void EnterCode()
    {
        
        if (Query.checkStageCorrection(new StageStruct(CodeInput.GetComponent<InputField>().text), true) != "")
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(Query.checkStageCorrection(new StageStruct(CodeInput.GetComponent<InputField>().text), false));
            return;
        }
        PlayerPrefs.SetString("CurrentStageQuery", CodeInput.GetComponent<InputField>().text);
        PlayerPrefs.SetString("CurrentStageInfo", "CODE");
        SceneManager.LoadScene("Play");
    }

}
