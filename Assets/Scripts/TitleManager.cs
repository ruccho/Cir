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
    public GameObject ErrorDialog;
    public AudioClip Enter;
    public GameObject InfoButton;
    Canvas TitleCanvas;
    Canvas ModeSelectCanvas;

    // Use this for initialization
    void Start()
    {
        if(Screen.height < 800)
        {
            ErrorDialog.GetComponent<ErrorDialog>().OpenDialog("スクリーンサイズが小さいため表示が崩れる場合があります。");
        }
        if (PlayerPrefs.HasKey("isFirstPlay") == false)
        {
            InfoButton.SetActive(false);
            PlayerPrefs.SetInt("ClearedPresetStageNumber", 0);
        }
        if (!PlayerPrefs.HasKey("UndoNumber"))
        {
            PlayerPrefs.SetInt("UndoNumber", 0);
        }
        if (!PlayerPrefs.HasKey("WeightLevel"))
        {
            PlayerPrefs.SetInt("WeightLevel", 0);
        }
        if (!PlayerPrefs.HasKey("SpinLevel"))
        {
            PlayerPrefs.SetInt("SpinLevel", 0);
        }
        if (!PlayerPrefs.HasKey("Coin"))
        {
            PlayerPrefs.SetInt("Coin", 0);
        }
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
        if (PlayerPrefs.HasKey("isFirstPlay") == false)
        {
            PlayerPrefs.SetInt("isFirstPlay", 1);
            SceneManager.LoadScene("Introduction");
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(Enter);
            if (MainCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime != 0) return;
            MainCamera.GetComponent<Animator>().SetTrigger("Forward");
            SetActiveCanvas(ModeSelectCanvas);
        }
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
        Application.OpenURL("http://ruccho.github.io/cir/howto.html");

    }

    public void SetActiveCanvas(Canvas ActiveCanvas)
    {
        TitleCanvas.enabled = false;
        ModeSelectCanvas.enabled = false;
        ActiveCanvas.enabled = true;
    }
}
