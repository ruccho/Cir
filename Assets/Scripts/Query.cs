using UnityEngine;
using System.Collections;

public class Query/* : MonoBehaviour*/
{
    enum StageTextError
    {
        StartPosition, KeyDoor
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

   /* public static string generateQuery(string text, string title, string description)
    {
        //int version = 1;
        string versionstring = "001";

        string textstring = WWW.EscapeURL(text);
        string titlestring = WWW.EscapeURL(title);
        string descriptionstring = WWW.EscapeURL(description);
        string returnQuery = "?v=" + versionstring + "&s=" + textstring + "&t=" + titlestring + "&d=" + descriptionstring + "&c=0";
        return returnQuery;
    }*/
    public static string generateQuery(string text, string title, string description, int turncount = 0)
    {
        string versionstring = "002";

        string textstring = WWW.EscapeURL(text);
        string titlestring = WWW.EscapeURL(title);
        string descriptionstring = WWW.EscapeURL(description);
        string turncountstring = turncount.ToString();
        string returnQuery = "?v=" + versionstring + "&s=" + textstring + "&t=" + titlestring + "&d=" + descriptionstring + "&c=" + turncountstring;
        return returnQuery;
    }

    public static string checkStageCorrection(StageStruct Stage, bool isEditMode)
    {
        if(Stage.isValid == false)
        {
            return "コードの構文に誤りがあります。コードが壊れている可能性があります。(isValid==false)";
        }
        if (Stage.StageBody.Length - Stage.StageBody.Replace('3'.ToString(), "").Length != 1)
        {
            switch (isEditMode)
            {
                case true:
                    return "プレイヤー開始地点が2つ以上あるかまたは1つもありません。";
                case false:
                    return "コードの構文に誤りがあります。コードが壊れている可能性があります。";
            }
        }
        if (Stage.StageBody.Length - Stage.StageBody.Replace('4'.ToString(), "").Length == 0)
        {
            switch (isEditMode)
            {
                case true:
                    return "ゴールが配置されていません。";
                case false:
                    return "コードの構文に誤りがあります。コードが壊れている可能性があります。";
            }
        }
        //5と6が、どっちもがないわけでも、どちらもあるわけでもない
        if ((!(Stage.StageBody.Length - Stage.StageBody.Replace('5'.ToString(), "").Length == 1 && Stage.StageBody.Length - Stage.StageBody.Replace('6'.ToString(), "").Length == 1)) && !(Stage.StageBody.Length - Stage.StageBody.Replace('5'.ToString(), "").Length == 0 && Stage.StageBody.Length - Stage.StageBody.Replace('6'.ToString(), "").Length == 0))
        {
            switch (isEditMode)
            {
                case true:
                    return "鍵とドアの配置に誤りがあります。鍵とドアは１組のみ配置でき、どちらかのみを配置することもできません。";
                case false:
                    return "コードの構文に誤りがあります。コードが壊れている可能性があります。";
            }

        }
        return "";
    }

}

public class StageStruct
{
    private bool allowCauseChange = true;
    public bool isValid;
    public int StageVersion;
    public string StageText;
    public string StageTitle;
    public string StageDescription;
    public int StageWidth;
    public int StageHeight;
    public int StageTurnCount;
    public string StageBody
    {
        get
        {
            return StageText.Substring(2);
        }
    }

    const int NEWEST_VERSION = 2;

    public StageStruct(string text, string title, string description, int turncount = 0)
    {
        isValid = true;
        StageVersion = 1;
        StageText = text;
        StageTitle = title;
        StageDescription = description;
        StageWidth = 0;
        StageHeight = 0;
        StageTurnCount = turncount;
        //StageBody = text.Substring(2);
        if (CalcWidthAndHeight(text) != null)
        {
            StageWidth = CalcWidthAndHeight(text)[0];
            StageHeight = CalcWidthAndHeight(text)[1];
        }
        else
        {
            isValid = false;
        }
    }

    public StageStruct(string url)
    {
        //initialize
        isValid = false;
        StageVersion = -1;
        StageText = "";
        StageTitle = "";
        StageDescription = "";
        StageWidth = 0;
        StageHeight = 0;
        StageTurnCount = 0;
        //StageBody = "";


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
                if (StageVersion >= 2)
                {
                    if (!(int.TryParse(extractQueryBody(url, 'c'), out parseresult)))
                    {
                        isValid = false;
                        return;
                    }else
                    {
                        StageTurnCount = parseresult;
                    }
                }
                //StageBody = StageText.Substring(2);
                if (StageText == null || StageText == "")
                {
                    isValid = false;
                    return;
                }
                if (CalcWidthAndHeight(StageText) != null)
                {
                    StageWidth = CalcWidthAndHeight(StageText)[0];
                    StageHeight = CalcWidthAndHeight(StageText)[1];
                }
                else
                {
                    isValid = false;
                }
                isValid = true;
            }
        }
    }
    private string extractQueryBody(string url, char QueryLetter)
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

    private int[] CalcWidthAndHeight(string StageText)
    {
        int StageWidth;
        int StageHeight;
        int parseresult;
        if (!(int.TryParse(StageText.Substring(0, 2), out parseresult))) return null;
        StageWidth = parseresult;

        //３文字目から最後までを切り取り、StageMapに格納
        StageText = StageText.Substring(2);

        //構造文字列の長さがWidthで割り切れるかチェックし、格納
        if (StageText.Length % StageWidth != 0) return null;
        StageHeight = StageText.Length / StageWidth;
        int[] returntext = new int[2];
        returntext[0] = StageWidth;
        returntext[1] = StageHeight;
        return returntext;
    }
}

