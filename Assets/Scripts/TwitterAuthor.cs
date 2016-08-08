using UnityEngine;
using System.Collections;
using System;

public class TwitterAuthor : MonoBehaviour {

    public GameObject AccessTokenKeeper;
    public string api_key = "";
    public string api_secret = "";
    public string callback_url;
    int unix_timestamp;
    string access_token_secret;
    string request_url = "https://api.twitter.com/oauth/request_token";

    string signature_key;
    // Use this for initialization
    void Start () {
        GameObject.DontDestroyOnLoad(AccessTokenKeeper);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void StartAuthorization()
    {
        unix_timestamp = (int)((DateTime.UtcNow - (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds);
        /*
        request_url = WWW.EscapeURL(request_url);
        access_token_secret = "";
        signature_key = WWW.EscapeURL(api_secret) + "&" + WWW.EscapeURL(access_token_secret);

        WWWForm form = new WWWForm();
        System.Collections.Generic.Dictionary<string, string> header = new System.Collections.Generic.Dictionary<string, string>();
        byte[] rawData = form.data;
        header["Authorization"] = "OAuth oauth_callback=\"" + WWW.EscapeURL(callback_url) + "\",oauth_consumer_key=\"" + WWW.EscapeURL(api_key) + "\",oauth_nonce=\"" + "NONCENONCENONCE" + "\",oauth_signature_method=\"HAMC-SHA1\",oauth_timestamp
    */}

}
