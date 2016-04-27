using UnityEngine;
using System.Collections;


public class TweetGet : MonoBehaviour
{
    string TWEET_HASH = "CirPuzzle";
    string bearerToken;
    string searchResponce;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(TokenGet());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TokenGet()
    {
        string url = "https://api.twitter.com/oauth2/token";
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "client_credentials");
        System.Collections.Generic.Dictionary<string, string> header = new System.Collections.Generic.Dictionary<string, string>();
        byte[] rawData = form.data;
        header["Authorization"] = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("wbjh7T6ppiHQvUOEQOZS0v7B3:3Bpeyzfbk9Lt0Fd6MdDtzQmAlRJPqHGhGS8QA2HMjGQGjbqPvl"));
        header["Content-Type"] = "application/x-www-form-urlencoded;charset=UTF-8";
        WWW www = new WWW(url, rawData, header);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
            bearerToken = JsonUtility.FromJson<TwitterToken>(www.text).access_token;
            StartCoroutine(TweetSearch());
        } else{
            Debug.Log(www.error);
        }
       
    }
    
    IEnumerator TweetSearch(){
        string url = "https://api.twitter.com/1.1/search/tweets.json?count=5&q=" + WWW.EscapeURL("#" + TWEET_HASH, System.Text.Encoding.UTF8) + "";
        System.Collections.Generic.Dictionary<string, string> header = new System.Collections.Generic.Dictionary<string, string>();
        header["Authorization"] = "Bearer " + bearerToken;
        WWW www = new WWW(url, null, header);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
            searchResponce = www.text;
            ExtractTweet();
        }else Debug.Log(www.error);
    }
    
    void ExtractTweet(){
        JSONObject searchedJson = new JSONObject(searchResponce);
        Debug.Log(searchedJson.GetField("statuses").list[0].GetField("text").str);
        Tweet tweet_1 = new Tweet(searchedJson.GetField("statuses").list[0]);
        Debug.Log(tweet_1.TweetId);
        Debug.Log(tweet_1.StageTitle);
        Debug.Log(tweet_1.StageDescription);
        Debug.Log(tweet_1.StageText);
    }
}
