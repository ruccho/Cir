﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PresetStageSelector : MonoBehaviour {

    public float FadeTimeSecond;
    FadeManager fadeManager;
	// Use this for initialization
	void Start () {
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void ButtonPlay(int StageNumber){
        PlayerPrefs.SetString("CurrentStageText", ((TextAsset)Resources.Load("Stages/Stage_" + StageNumber)).text);
        Invoke("GoPlay", FadeTimeSecond);
        
    }
    
    void GoPlay(){
        fadeManager.LoadLevel("Play", FadeTimeSecond);
        //SceneManager.LoadScene("Play");
    }
    
    public void ButtonBack(){
        fadeManager.LoadLevel("Title", FadeTimeSecond);
    }
    
    
}
