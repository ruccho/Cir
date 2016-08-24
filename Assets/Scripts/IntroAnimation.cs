using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroAnimation : MonoBehaviour {
    public GameObject particleObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayBurst()
    {
        particleObject.GetComponent<ParticleSystem>().Play();
    }

    public void next()
    {
        SceneManager.LoadScene("Title");
    }

    public void howto()
    {
        Application.OpenURL("http://ruccho.github.io/cir/howto.html");
    }

    public void StopAudio()
    {
        GetComponent<AudioSource>().Stop();
    }
}
