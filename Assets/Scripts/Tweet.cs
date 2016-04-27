using UnityEngine;
public class Tweet
{

    public readonly string TweetId;
    public readonly string StageTitle;
    public readonly string StageDescription;
    public readonly string StageText;
    private string url;

    string queryTitle = "title";
    string queryDescription = "description";
    string queryText = "text";

    ///<summary></summary>
    /// <param name="json">1ツイート単位のJSON。</param>
    public Tweet(JSONObject json)
    {
        TweetId = json.GetField("id_str").str;
        StageText = json.GetField("text").str;
        url = json.GetField("entities").GetField("urls").list[0].GetField("expanded_url").str;

        //urlから諸情報を抽出
        int equalindexOfTitle = url.IndexOf("=", 0);
        Debug.Log(equalindexOfTitle.ToString());
        int equalindexOfDescription = url.IndexOf("=", equalindexOfTitle + 1);
        Debug.Log(equalindexOfDescription.ToString());
        int equalindexOfText = url.IndexOf("=", equalindexOfDescription + 1);
        Debug.Log(equalindexOfText.ToString());
        StageTitle = WWW.UnEscapeURL(url.Substring(equalindexOfTitle + 1, (equalindexOfDescription - queryDescription.Length) - equalindexOfTitle - 2), System.Text.Encoding.UTF8);
        StageDescription = WWW.UnEscapeURL(url.Substring(equalindexOfDescription + 1, (equalindexOfText - queryText.Length) - equalindexOfDescription - 2), System.Text.Encoding.UTF8);
        StageText = url.Substring(equalindexOfText + 1);
    }
}
