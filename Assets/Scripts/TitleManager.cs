using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public GameObject FadeImage;
    public float FadeTimeSecond;
    public GameObject MainCamera;
    public GameObject TitleCanvasObject;
    public GameObject ModeSelectCanvasObject;
    Canvas TitleCanvas;
    Canvas ModeSelectCanvas;

    // Use this for initialization
    void Start()
    {
        TitleCanvas = TitleCanvasObject.GetComponent<Canvas>();
        ModeSelectCanvas = ModeSelectCanvasObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonStart()
    {
        MainCamera.GetComponent<Animator>().SetTrigger("Forward");
        SetActiveScene(ModeSelectCanvas);
    }

    void GoPlayScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void ButtonBack()
    {
        
    }

    void FadeAndGoPlay()
    {
        FadeImage.GetComponent<Fade>().FadeIn(FadeTimeSecond);
        Invoke("GoPlayScene", FadeTimeSecond);
    }

    public void ButtonPlayPreset(int StageNumber)
    {
        //Resources/StagesからテキストファイルをロードしPlayerPrefsのCurrentStageTextにセット
        string stageText = ((TextAsset)Resources.Load("Stages/Stage_" + StageNumber.ToString())).text;
        PlayerPrefs.SetString("CurrentStageText", stageText);
        FadeAndGoPlay();
    }

    public void ButtonNomalMode()
    {

    }


    public void ButtonCreate()
    {

    }
    public void ButtonInfo()
    {

    }

    public void SetActiveScene(Canvas ActiveCanvas)
    {
        TitleCanvas.enabled = false;
        ModeSelectCanvas.enabled = false;
        ActiveCanvas.enabled = true;
    }
}
