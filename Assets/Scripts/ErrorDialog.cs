using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ErrorDialog : MonoBehaviour {

    public GameObject ErrorMessageText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenDialog(string Message)
    {
        ErrorMessageText.GetComponent<Text>().text = Message;
        gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        StartCoroutine(DeactiveAfterOneFrame());
    }

    IEnumerator DeactiveAfterOneFrame()
    {
        yield return null;
        gameObject.SetActive(false);
    }
}
