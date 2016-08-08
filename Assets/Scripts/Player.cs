using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StageCreation")
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log("TRIGGER");
		if(col.gameObject.tag == "Goal"){
			Debug.Log("GOAL");

		}
	}
}
