using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShareModeMenu : MonoBehaviour
{

    public GameObject CodePanel;
    public GameObject ErrorDialog;
    public GameObject CodeInput;
    public AudioClip ClickSound;
    public Animator Canvas;
    enum MenuState
    {
        Root, Code, Create, CreateNew
    }
    MenuState menuState;

    // Use this for initialization
    void Start()
    {
        menuState = MenuState.Root;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Back()
    {
        switch (menuState)
        {
            case MenuState.Root:
                GetComponent<AudioSource>().PlayOneShot(ClickSound);
                SceneManager.LoadScene("Title");
                break;

            case MenuState.Code:
                GetComponent<AudioSource>().PlayOneShot(ClickSound);
                Canvas.SetTrigger("CloseCode");
                menuState = MenuState.Root;
                break;
            case MenuState.Create:
                GetComponent<AudioSource>().PlayOneShot(ClickSound);
                Canvas.SetTrigger("CloseCreate");
                menuState = MenuState.Root;
                break;
            case MenuState.CreateNew:
                GetComponent<AudioSource>().PlayOneShot(ClickSound);
                Canvas.SetTrigger("CloseCreateNew");
                menuState = MenuState.Root;
                break;
        }
    }

    public void Play()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene("ShareStageSelector");
    }

    public void Create()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        menuState = MenuState.Create;
        Canvas.SetTrigger("GoCreate");
        //SceneManager.LoadScene("StageCreationMenu");
    }

    public void CreateNew()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        menuState = MenuState.CreateNew;
        Canvas.SetTrigger("GoCreateNew");
    }
    

    public void OpenCodePanel()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        menuState = MenuState.Code;
        Canvas.SetTrigger("GoCode");
        //CodePanel.SetActive(true);

    }


    public void EnterCode()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);

        if (Query.checkStageCorrection(new StageStruct(CodeInput.GetComponent<InputField>().text), true) != "")
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(Query.checkStageCorrection(new StageStruct(CodeInput.GetComponent<InputField>().text), false));
            return;
        }
        if (new UTJ.Board(new StageStruct(CodeInput.GetComponent<InputField>().text)).isSolvable() != null)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog(new UTJ.Board(new StageStruct(PlayerPrefs.GetString("CurrentStageQuery"))).isSolvable());
            return;
        }
        PlayerPrefs.SetString("CurrentStageQuery", CodeInput.GetComponent<InputField>().text);
        PlayerPrefs.SetString("CurrentStageInfo", "CODE");
        SceneManager.LoadScene("Play");
    }

}
