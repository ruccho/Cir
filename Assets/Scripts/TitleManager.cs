using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    FadeManager fadeManager;
    public float FadeTimeSecond;
    public GameObject MainCamera;
    public GameObject TitleCanvasObject;
    public GameObject ModeSelectCanvasObject;
    Canvas TitleCanvas;
    Canvas ModeSelectCanvas;

    // Use this for initialization
    void Start()
    {
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        TitleCanvas = TitleCanvasObject.GetComponent<Canvas>();
        ModeSelectCanvas = ModeSelectCanvasObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonStart()
    {
        
        if(MainCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime != 0) return;
        MainCamera.GetComponent<Animator>().SetTrigger("Forward");
        SetActiveCanvas(ModeSelectCanvas);
    }

    public void ButtonBack()
    {
        if(MainCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime != 0) return;
        MainCamera.GetComponent<Animator>().SetTrigger("Backward");
        SetActiveCanvas(TitleCanvas);
    }

    public void ButtonNomalMode()
    {
        Invoke("GoPlayPreset", FadeTimeSecond);
    }
    void GoPlayPreset()
    {
        fadeManager.LoadLevel("PresetStageSelection", FadeTimeSecond);
        //SceneManager.LoadScene("PresetStageSelection");
    }


    public void ButtonCreate()
    {

    }
    public void ButtonInfo()
    {

    }

    public void SetActiveCanvas(Canvas ActiveCanvas)
    {
        TitleCanvas.enabled = false;
        ModeSelectCanvas.enabled = false;
        ActiveCanvas.enabled = true;
    }
}
