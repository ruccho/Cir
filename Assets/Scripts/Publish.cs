using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Publish : MonoBehaviour {
    public GameObject URLText;
    public GameObject URLPanel;
    public AudioClip ClickSound;


    const string text = "リンクを開いてプレイ";
    const string url = "http://ruccho.github.io/cir/gateway.html";
    string query;
	// Use this for initialization
	void Start () {
        //クエリ文字列を？＋～の形に整形
        query = PlayerPrefs.GetString("CurrentEditingStageQuery");
        if(query.IndexOf("?") == -1)
        {
            query = "?" + query;
        }

        query = query.Substring(query.IndexOf("?"));
        URLText.GetComponent<InputField>().text = url + query;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Twitter()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Application.OpenURL("https://twitter.com/intent/tweet?hashtags=CirPuzzle&text=" + WWW.EscapeURL(text) + "&url=" + WWW.EscapeURL(url) + WWW.EscapeURL(query));
    } 

    public void Line()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Application.OpenURL("line://msg/text/" + WWW.EscapeURL(url) + WWW.EscapeURL(query));
    }

    public void Facebook()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Application.OpenURL("https://www.facebook.com/sharer/sharer.php?u=" + WWW.EscapeURL(url) + WWW.EscapeURL(query));
    }

    public void Googleplus()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        string hostUrl = url.Substring(7);
        Application.OpenURL("https://plus.google.com/share?url=" + WWW.EscapeURL(hostUrl));
    }

    public void Copy()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        GUIUtility.systemCopyBuffer = url + query;
    }

    public void OpenURLPanel()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        URLPanel.SetActive(true);
    }

    public void CloseURLPanel()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        URLPanel.SetActive(false);
    }

    public void Back()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene("StageCreation");
    }
}
