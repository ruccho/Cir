using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TweetDetailPanel : MonoBehaviour {


    public GameObject UserImage;
    public GameObject UserID;
    public GameObject NameText;
    public GameObject TitleText;
    public GameObject DescriptionText;
    public GameObject FavoritesText;
    public GameObject CreatedAtText;

    private Tweet selfTweet;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OpenDetail(Tweet tweet)
    {
        selfTweet = tweet;
        UserImage.GetComponent<UnityEngine.UI.RawImage>().texture = selfTweet.UserImage;
        UserID.GetComponent<UnityEngine.UI.Text>().text = "@" + selfTweet.UserId;
        NameText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.UserName;
        TitleText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.Stage.StageTitle;
        DescriptionText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.Stage.StageDescription;
        FavoritesText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.Favorites.ToString();
        CreatedAtText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.CreatedAt;
    }
    public void CloseDetail()
    {
        gameObject.SetActive(false);
    }

    public void openInTwitter()
    {
        Application.OpenURL("https://twitter.com/" + selfTweet.UserId + "/status/" + selfTweet.TweetId);
    }

    public void play()
    {
        PlayerPrefs.SetString("CurrentStageQuery", Query.generateQuery(selfTweet.Stage.StageText, selfTweet.Stage.StageTitle, selfTweet.Stage.StageDescription));
        PlayerPrefs.SetString("CurrentStageText", selfTweet.Stage.StageText);
        SceneManager.LoadScene("Play");
    }
}
