﻿using UnityEngine;
public class Tweet
{

    public readonly string UserId;
    public readonly string UserName;
    public readonly Texture2D UserImage;
    public readonly string TweetId;
    public readonly StageStruct Stage;
    public readonly long Favorites;
    public readonly string CreatedAt;
    private string url;

    ///<summary></summary>
    /// <param name="json">1ツイート単位のJSON。</param>
    public Tweet(JSONObject json)
    {
        url = json.GetField("entities").GetField("urls").list[0].GetField("expanded_url").str;
        Stage = new StageStruct(url); 
       
        //ID(名前)と画像を抽出
        UserId = json.GetField("user").GetField("screen_name").str;//@ID
        UserName = System.Text.RegularExpressions.Regex.Unescape(json.GetField("user").GetField("name").str);//ユーザー設定の名前
        TweetId = json.GetField("id_str").str;
        Favorites = json.GetField("favorite_count").i;

        string[] datetimearray = json.GetField("created_at").str.Split(' ');
        //月を整形
        int year = int.Parse(datetimearray[5]);
        int month = 0;
        int day = int.Parse(datetimearray[2]);
        string[] timearray = datetimearray[3].Split(':');
        int hour = int.Parse(timearray[0]);
        int minute = int.Parse(timearray[1]);
        switch (datetimearray[1])
        {
            case "Jan":
                month = 1;
                break;
            case "Feb":
                month = 2;
                break;
            case "Mar":
                month = 3;
                break;
            case "Apr":
                month = 4;
                break;
            case "May":
                month = 5;
                break;
            case "Jun":
                month = 6;
                break;
            case "Jul":
                month = 7;
                break;
            case "Aug":
                month = 8;
                break;
            case "Sep":
                month = 9;
                break;
            case "Oct":
                month = 10;
                break;
            case "Nov":
                month = 11;
                break;
            case "Dec":
                month = 12;
                break;
        }
        System.DateTime CreatedAtUTC = new System.DateTime(year, month, day, hour, minute, 0, System.DateTimeKind.Utc);
        System.DateTime CreatedAtDateTime = CreatedAtUTC + System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now);
        string tempHour = CreatedAtDateTime.Hour.ToString();
        if(tempHour.Length == 1)
        {
            tempHour = tempHour.Insert(0, "0");
        }
        string tempMinute = CreatedAtDateTime.Minute.ToString();
        if (tempMinute.Length == 1)
        {
            tempMinute = tempMinute.Insert(0, "0");
        }
        CreatedAt = CreatedAtDateTime.Year + "年" + CreatedAtDateTime.Month + "月" + CreatedAtDateTime.Day + "日 " + tempHour + ":" + tempMinute;

        //Debug.Log(json.GetField("user").GetField("profile_image_url").str);
        WWW www = new WWW(json.GetField("user").GetField("profile_image_url").str.Replace("\\/", "/").Replace("normal", "bigger"));
        while (!www.isDone) { }
        if(www.error == null)
        {
            //Debug.Log("Write texture");
            UserImage = www.textureNonReadable;
        }else
        {
            Debug.Log(www.error);
        }
    }
    
}
