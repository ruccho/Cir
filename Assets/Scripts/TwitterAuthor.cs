using UnityEngine;
using System.Collections;

public class TwitterAuthor : MonoBehaviour {

    public GameObject AccessTokenKeeper;

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(AccessTokenKeeper);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartAuthorization()
    {

    }

}
