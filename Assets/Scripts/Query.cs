using UnityEngine;
using System.Collections;

public class Query/* : MonoBehaviour*/ {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static string generateQuery(string text, string title, string description)
    {
        //バージョン1のオーバーロードメソッド。
        //新たなバージョンにはオーバーロードを追加して対応。
        //int version = 1;
        string versionstring = "001";

        string textstring = WWW.EscapeURL(text);
        string titlestring = WWW.EscapeURL(title);
        string descriptionstring = WWW.EscapeURL(description);
        string returnQuery = "?v=" + versionstring + "&s=" + textstring + "&t=" + titlestring + "&d=" + descriptionstring;
        return returnQuery;
    }

}

public struct StageStruct
{
    public bool isValid;
    public int StageVersion;
    public string StageText;
    public string StageTitle;
    public string StageDescription;

    const int NEWEST_VERSION = 1;

    public StageStruct(string text, string title, string description)
    {
        isValid = true;
        StageVersion = 1;
        StageText = text;
        StageTitle = title;
        StageDescription = description;
    }

    public StageStruct(string url)
    {
        //initialize
        isValid = false;
        StageVersion = -1;
        StageText = "";
        StageTitle = "";
        StageDescription = "";

        if (url.IndexOf("v=") == -1)
        {
            isValid = false;
        }
        else
        {
            //バージョンは001,002,のように表記されている
            string VerStr = extractQueryBody(url, 'v');
            //Version = int.Parse(VerStr);
            VerStr = VerStr.TrimStart('0');
            int parseresult;
            if (!(int.TryParse(VerStr, out parseresult)))
            {
                //バージョン指定が適当でない
                isValid = false;
            }
            else
            {
                StageVersion = parseresult;
                if (NEWEST_VERSION < StageVersion)
                {
                    //未対応バージョンなら読み込まない
                    isValid = false;
                    return;
                }

                //タイトル
                StageTitle = WWW.UnEscapeURL(extractQueryBody(url, 't'));
                StageDescription = WWW.UnEscapeURL(extractQueryBody(url, 'd'));
                StageText = extractQueryBody(url, 's');
                isValid = true;
            }
        }
    }
    private string extractQueryBody(string url,  char QueryLetter)
    {
        int QueryIndex = url.IndexOf(QueryLetter + "=");

        if (QueryIndex == -1)
        {
            return "";
        }
        int nextQueryIndex = url.IndexOf("&", QueryIndex);
        if (nextQueryIndex < 0)
        {
            //末尾のクエリの場合
            nextQueryIndex = url.Length;
        }
        return url.Substring(QueryIndex + 2, nextQueryIndex - QueryIndex - 2);
    }
}

