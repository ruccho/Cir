using UnityEngine;
using System.Collections;

public class ShareModeMenu : MonoBehaviour {

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
}
