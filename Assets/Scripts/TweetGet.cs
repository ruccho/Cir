using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TweetGet : MonoBehaviour
{
    string TWEET_HASH = "CirPuzzle";
    string bearerToken;
    string searchResponce;
    List<Tweet> tweetQueue = new List<Tweet>();
    bool searchEnd;
    int pagenumber;

    public GameObject[] ItemPanels;

    public GameObject CoverPanel;
    public GameObject NextPageButton;
    public GameObject PreviousPageButton;
    public GameObject PageNumberText;
    public AudioClip ClickSound;


    enum searchmode
    {
        RECENT, POPULAR
    }
    private searchmode SearchMode;


    int ELEMENTS_ONONEPAGE = 5;
    string lastId;
    // Use this for initialization
    void Start()
    {
        SearchMode = searchmode.RECENT;
        ELEMENTS_ONONEPAGE = ItemPanels.Length;
        pagenumber = 1;
        searchEnd = false;
        StartCoroutine(TokenGet());
        StartCoroutine(FillQueue());
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
        }
        else
        {
            Debug.Log(www.error);
        }

    }

    void ShowItems()
    {
        //現在のページにあたるツイートをキューから取り出して表示を更新
        for(int i = 0; i < ELEMENTS_ONONEPAGE; i++)
        {
            int targetArrayNumber = i + ((pagenumber - 1) * ELEMENTS_ONONEPAGE);
            if (targetArrayNumber <= tweetQueue.Count - 1)
            {
                ItemPanels[i].GetComponent<TweetPanel>().SetTweet(tweetQueue[targetArrayNumber]);
            }else
            {
                ItemPanels[i].GetComponent<TweetPanel>().ClearTweet();
            }
        }
        RefreshButtonState();
    }

    public void nextPage()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        pagenumber++;
        

        //表示を更新

        ShowItems();
    }

    public void previousPage()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        //移動先のページが一ページ目ならpreviouspageボタンを無効にする
        pagenumber--;

        //表示を更新

        ShowItems();

    }


    /*IEnumerator TweetSearch(int count)
    {
        string url = "https://api.twitter.com/1.1/search/tweets.json?count=5&q=" + WWW.EscapeURL("#" + TWEET_HASH, System.Text.Encoding.UTF8) + "";
        System.Collections.Generic.Dictionary<string, string> header = new System.Collections.Generic.Dictionary<string, string>();
        header["Authorization"] = "Bearer " + bearerToken;
        WWW www = new WWW(url, null, header);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
            searchResponce = www.text;

        }
        else Debug.Log(www.error);
    }*/

    //現在の次のページまでFillする
    IEnumerator FillQueue()
    {
        while (!searchEnd)
        {
           
                //検索の余地があり、かつキューが満たされていない
                CoverPanel.SetActive(true);
                yield return null;
                while (bearerToken == null)
                {
                    yield return null;
                }
                string url;
                if (SearchMode == searchmode.RECENT)
                {
                    if (lastId == null)
                    {
                        url = "https://api.twitter.com/1.1/search/tweets.json?result_type=recent&count=20&q=" + WWW.EscapeURL("#" + TWEET_HASH, System.Text.Encoding.UTF8) + "";
                    }
                    else
                    {
                        url = "https://api.twitter.com/1.1/search/tweets.json?result_type=recent&count=20&q=" + WWW.EscapeURL("#" + TWEET_HASH, System.Text.Encoding.UTF8) + "&max_id=" + lastId;
                    }
                }
                else
                {
                    if (lastId == null)
                    {
                        url = "https://api.twitter.com/1.1/search/tweets.json?result_type=popular&count=20&q=" + WWW.EscapeURL("#" + TWEET_HASH, System.Text.Encoding.UTF8) + "";
                    }
                    else
                    {
                        url = "https://api.twitter.com/1.1/search/tweets.json?result_type=popular&count=20&q=" + WWW.EscapeURL("#" + TWEET_HASH, System.Text.Encoding.UTF8) + "&max_id=" + lastId;
                    }
                }
                System.Collections.Generic.Dictionary<string, string> header = new System.Collections.Generic.Dictionary<string, string>();
                header["Authorization"] = "Bearer " + bearerToken;
                WWW www = new WWW(url, null, header);
                yield return www;
                if (www.error == null)
                {
                    Debug.Log(www.text);
                    searchResponce = www.text;
                }
                else
                {
                    Debug.Log(www.error);
                    CoverPanel.SetActive(false);
                    yield break;
                }
                JSONObject searchedJson = new JSONObject(searchResponce);
                if (searchedJson.GetField("statuses").Count == 0)
                {
                    //結果がみつからなかった
                    Debug.Log("結果0");
                    searchEnd = true;
                    //CoverPanel.SetActive(false);
                    //yield break;
                }
                for (int i = 0; i < searchedJson.GetField("statuses").Count; i++)
                {
                    Tweet tempTweet = new Tweet(searchedJson.GetField("statuses").list[i]);
                if (tempTweet.Stage.isValid == true)
                    {
                        tweetQueue.Add(tempTweet);
                    //lastId = (Double.Parse(tempTweet.TweetId) - 1).ToString();
                }
                lastId = (Double.Parse(tempTweet.TweetId) - 1).ToString();
            }
                if (tweetQueue.Count >= 20) searchEnd = true;
                CoverPanel.SetActive(false);
            
        }
    
        ShowItems();
        yield break;

    }

    public void ShowPopular()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        tweetQueue.Clear();
        lastId = null;
        searchEnd = false;
        SearchMode = searchmode.POPULAR;
        pagenumber = 1;
        StartCoroutine(FillQueue());
    }

    public void ShowRecent()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        tweetQueue.Clear();
        lastId = null;
        searchEnd = false;
        SearchMode = searchmode.RECENT;
        pagenumber = 1;
        StartCoroutine(FillQueue());
    }

    private void RefreshButtonState()
    {
        //次のページに表示されるべきアイテムがあるかチェック
        if (pagenumber * ELEMENTS_ONONEPAGE >= tweetQueue.Count)
        {
            //現在のページが最終
            NextPageButton.SetActive(false);
        }else
        {
            NextPageButton.SetActive(true);
        }
        if(pagenumber == 1)
        {
            PreviousPageButton.SetActive(false);
        }else
        {
            PreviousPageButton.SetActive(true);
        }
        PageNumberText.GetComponent<UnityEngine.UI.Text>().text = pagenumber.ToString();
    }

    public void backToShareMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(ClickSound);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ShareModeMenu");
    }

}
