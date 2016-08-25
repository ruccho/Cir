using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PresetStageSelector : MonoBehaviour
{
    public AudioClip ClickSound;
    public float FadeTimeSecond;
    public GameObject ErrorDialog;
    FadeManager fadeManager;
    // Use this for initialization
    void Start()
    {
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
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
        PlayerPrefs.SetString("CurrentStageQuery", Query.generateQuery(((TextAsset)Resources.Load("Stages/Stage_" + StageNumber)).text, "未設定", "未設定"));
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
