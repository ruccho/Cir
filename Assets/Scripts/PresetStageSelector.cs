using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PresetStageSelector : MonoBehaviour
{
    public AudioClip ClickSound;
    public float FadeTimeSecond;
    public GameObject ErrorDialog;
    public TextAsset[] StageTexts;
    public GameObject[] Buttons;
    FadeManager fadeManager;
    // Use this for initialization
    void Start()
    {
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        for(int i = 0; i < Buttons.Length - (PlayerPrefs.GetInt("ClearedPresetStageNumber") + 1); i++)
        {
            Buttons[i + PlayerPrefs.GetInt("ClearedPresetStageNumber") + 1].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonPlay(int StageNumber)
    {
        if(PlayerPrefs.GetInt("ClearedPresetStageNumber") + 1 < StageNumber)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog("まだプレイできません。1から順に進めてください。");
            return;
        }
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        PlayerPrefs.SetString("CurrentStageQuery", StageTexts[StageNumber - 1].text);
        PlayerPrefs.SetString("CurrentStageInfo", "PRESET");
        PlayerPrefs.SetInt("CurrentPresetStageNumber", StageNumber);
        Invoke("GoPlay", FadeTimeSecond);

    }

    void GoPlay()
    {
        fadeManager.LoadLevel("Play", FadeTimeSecond);
        //SceneManager.LoadScene("Play");
    }

    public void ButtonBack()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        fadeManager.LoadLevel("Title", FadeTimeSecond);
    }


}
