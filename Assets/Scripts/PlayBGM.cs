using UnityEngine;
using System.Collections;

public class PlayBGM : MonoBehaviour {
    public AudioClip[] BGMs;
	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().clip = BGMs[System.DateTime.Now.Second % BGMs.Length];
        GetComponent<AudioSource>().Play();

    }
	
	// Update is called once per frame
	void Update () {
	}
}
