using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_ANDROID || UNITY_IOS)
using UnityEngine.Advertisements;
#endif
using UnityEngine.UI;

public class RewardCoinManager : MonoBehaviour {


    public PlayingManager PM;
    public Text RewardText;
    public Text CoinIndicateText;
    public Text BasicRewardText;
    public Text PerfectSolutionRewardText;
    public Text OnewayRewardText;
    public Text AdRewardText;
    public Button AdButton;

    int basicReward;
    int perfectSolutionReward;
    int onewayReward;
    bool rewardReceived = false;
    int reward_final;

    int beforeCoin;
    int afterCoin;

    void Awake()
    {
#if (UNITY_ANDROID || UNITY_IOS)
        Advertisement.Initialize("1152993");
#endif
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void sendScoreInfo(int turned_count, int fewest_turns, bool is_oneway)
    {
        beforeCoin = PlayerPrefs.GetInt("Coin");

        //基本報酬
        //最小手数ジャストの時の報酬
        int max_basic = fewest_turns * 8;
        //最小手数との差
        int diff = turned_count - fewest_turns;
        //0.9の(差分/3)乗を掛ける →3手多くて0.9倍になる
        basicReward = (int)(max_basic * Mathf.Pow(0.9f, (diff / 3f)));

        //ボーナス類
        //最短手数回答(1.2倍)
        bool is_perfect_solution = (turned_count == fewest_turns);
        perfectSolutionReward = is_perfect_solution ? (int)(basicReward * 0.2f) : 0;
        //無アンドゥかつ無コンテニュー→oneway(1.1倍)
        onewayReward = is_oneway ? (int)(basicReward * 0.1f) : 0;

        reward_final = basicReward + perfectSolutionReward + onewayReward;

        afterCoin = beforeCoin + reward_final;

        BasicRewardText.text = basicReward.ToString();
        PerfectSolutionRewardText.text = perfectSolutionReward.ToString();
        OnewayRewardText.text = onewayReward.ToString();
        AdRewardText.text = "0";
        RewardText.text = "+" + reward_final.ToString();
        CoinIndicateText.text = beforeCoin.ToString();
        PlayerPrefs.SetInt("Coin", afterCoin);

    }

    public void refreshCoinIndication()
    {
        CoinIndicateText.text = afterCoin.ToString();
    }

    public void Clear()
    {
        //クリアパネルでOK

#if (UNITY_ANDROID || UNITY_IOS)
        if (rewardReceived)
        {
            PM.Quit();
            return;
        }
        PlayerPrefs.SetInt("VideoCount", PlayerPrefs.GetInt("VideoCount") + 1);
        if ((PlayerPrefs.GetInt("VideoCount", 0) >= 3) && (PM.PlaySceneMode != PlayingManager.PlaySceneModeType.TEST))
        {
            if (Advertisement.IsReady("video"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResultNormal };
                Advertisement.Show("video", options);
            }
            else
            {
                Debug.Log("Advertisement is not ready");
            }
        }
        else
        {
            PM.Quit();
        }
#else
        
        PM.Quit();
#endif
    }


#if (UNITY_ANDROID || UNITY_IOS)

    public void RewardBonus()
    {
        if (PM.PlaySceneMode == PlayingManager.PlaySceneModeType.TEST)
        {
            return;
        }
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResultReward };
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Debug.Log("Advertisement is not ready");
        }
    }

    private void HandleShowResultReward(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                rewardReceived = true;
                AdRewardText.text = reward_final.ToString();
                reward_final = reward_final * 2;
                RewardText.text = "+" + reward_final.ToString();
                afterCoin += reward_final;
                CoinIndicateText.text = afterCoin.ToString();
                PlayerPrefs.SetInt("Coin", afterCoin);
                AdButton.interactable = false;
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
    private void HandleShowResultNormal(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                PlayerPrefs.SetInt("VideoCount", 0);
                PM.Quit();
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                PlayerPrefs.SetInt("VideoCount", 0);
                PM.Quit();
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                PM.Quit();
                break;
        }
    }
#endif
}
