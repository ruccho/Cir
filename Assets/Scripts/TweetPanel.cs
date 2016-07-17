using UnityEngine;
using System.Collections;

public class TweetPanel : MonoBehaviour {

    private Tweet selfTweet;
    public GameObject UserImage;
    public GameObject UserName;
    public GameObject Title;
    public GameObject Favorites;
    public GameObject detailPanel;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTweet(Tweet tweet)
    {
        selfTweet = tweet;
        refresh();
    }

    public void ClearTweet()
    {
        selfTweet = null;
        UserImage.GetComponent<UnityEngine.UI.RawImage>().texture = null;
        UserName.GetComponent<UnityEngine.UI.Text>().text = "…";
        Title.GetComponent<UnityEngine.UI.Text>().text = "…";
        Favorites.GetComponent<UnityEngine.UI.Text>().text = "…";
    }

    private void refresh()
    {
        //格納されたツイート情報に基づいて、表示を更新
        UserImage.GetComponent<UnityEngine.UI.RawImage>().texture = selfTweet.UserImage;
        UserName.GetComponent<UnityEngine.UI.Text>().text = selfTweet.UserName;
        Title.GetComponent<UnityEngine.UI.Text>().text = selfTweet.StageTitle;
        Favorites.GetComponent<UnityEngine.UI.Text>().text = selfTweet.Favorites.ToString();
    }

    public void OpenDetail()
    {
        if (selfTweet == null) return;
        detailPanel.SetActive(true);
        detailPanel.GetComponent<TweetDetailPanel>().OpenDetail(selfTweet);
    }
}
