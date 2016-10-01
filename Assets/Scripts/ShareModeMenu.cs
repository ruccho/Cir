using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShareModeMenu : MonoBehaviour {

    public GameObject CodePanel;
    public GameObject ErrorDialog;
    public GameObject CodeInput;
    public AudioClip ClickSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Back()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene("Title");
    }

    public void Play()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene("ShareStageSelector");
    }

    public void Create()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene("StageCreationMenu");
    }

    public void OpenCodePanel()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        CodePanel.SetActive(true);

    }

    public void CloseCodePanel()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        CodePanel.SetActive(false);
    }

    public void EnterCode()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);

        if (Query.checkStageCorrection(new StageStruct(CodeInput.GetComponent<InputField>().text), true) != "")
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(Query.checkStageCorrection(new StageStruct(CodeInput.GetComponent<InputField>().text), false));
            return;
        }
        if (new UTJ.Board(new StageStruct(PlayerPrefs.GetString("CurrentStageQuery"))).isSolvable() != null)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(new UTJ.Board(new StageStruct(PlayerPrefs.GetString("CurrentStageQuery"))).isSolvable());
            return;
        }
        PlayerPrefs.SetString("CurrentStageQuery", CodeInput.GetComponent<InputField>().text);
        PlayerPrefs.SetString("CurrentStageInfo", "CODE");
        SceneManager.LoadScene("Play");
    }

}
