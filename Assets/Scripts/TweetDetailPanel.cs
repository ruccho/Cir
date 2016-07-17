using UnityEngine;
using System.Collections;

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
        TitleText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.StageTitle;
        DescriptionText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.StageDescription;
        FavoritesText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.Favorites.ToString();
        CreatedAtText.GetComponent<UnityEngine.UI.Text>().text = selfTweet.CreatedAt;
    }
    public void CloseDetail()
    {
        gameObject.SetActive(false);
    }
}
