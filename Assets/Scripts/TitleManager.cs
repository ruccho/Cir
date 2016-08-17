﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    FadeManager fadeManager;
    public float FadeTimeSecond;
    public GameObject MainCamera;
    public GameObject TitleCanvasObject;
    public GameObject ModeSelectCanvasObject;
    public AudioClip Enter;
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
        GetComponent<AudioSource>().PlayOneShot(Enter);
        if(MainCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime != 0) return;
        MainCamera.GetComponent<Animator>().SetTrigger("Forward");
        SetActiveCanvas(ModeSelectCanvas);
    }

    public void ButtonBack()
    {
        GetComponent<AudioSource>().PlayOneShot(Enter);
        if (MainCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime != 0) return;
        MainCamera.GetComponent<Animator>().SetTrigger("Backward");
        SetActiveCanvas(TitleCanvas);
    }

    public void ButtonNomalMode()
    {
        GetComponent<AudioSource>().PlayOneShot(Enter);
        Invoke("GoPlayPreset", FadeTimeSecond);
    }
    void GoPlayPreset()
    {
        fadeManager.LoadLevel("PresetStageSelection", FadeTimeSecond);
        //SceneManager.LoadScene("PresetStageSelection");
    }

    public void ButtonShareMode()
    {
        GetComponent<AudioSource>().PlayOneShot(Enter);
        fadeManager.LoadLevel("ShareModeMenu", FadeTimeSecond);
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
