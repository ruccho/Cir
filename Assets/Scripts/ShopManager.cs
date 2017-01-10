using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//Please Attach to Shop Root Canvas
public class ShopManager : MonoBehaviour {

    public Text CoinText;
    public Text CurrentUndoNumText;
    //Max 15
    public Text WeightLevelTitleText;
    public Text SpinLevelTitleText;
    public Text WeightLevelPriceText;
    public Text SpinLevelPriceText;

    public Button UndoGetButton;
    public Button WeightLevelupGetButton;
    public Button SpinLevelupGetButton;

    public GameObject WeightLevelPanel;
    public GameObject SpinLevelPanel;

    int undoNum;
    int weightLev;
    int spinLev;
    public int coin;
    int weightPrice;
    int spinPrice;
    int undoprice = 150;

	// Use this for initialization
	void Start () {
        undoNum = PlayerPrefs.GetInt("UndoNumber");
        weightLev = PlayerPrefs.GetInt("WeightLevel");
        weightLev = 0;
        spinLev = PlayerPrefs.GetInt("SpinLevel");
        spinLev = 0;
        coin = PlayerPrefs.GetInt("Coin");
        coin = 1000000000;
        RefreshParams();
        
    }
	
    void RefreshParams()
    {
        CoinText.text = coin.ToString();
        CurrentUndoNumText.text = "現在の所持数：" + undoNum;
        if(weightLev == 10)
        {
            WeightLevelPanel.SetActive(false);
        }
        if (spinLev == 20)
        {
            SpinLevelPanel.SetActive(false);
        }
        WeightLevelTitleText.text = "ヘビー Lv" + (weightLev + 1).ToString();
        SpinLevelTitleText.text = "クイック Lv" + (spinLev + 1).ToString();
        int tempprice = 100;
        for(int i = 0; i < weightLev; i++)
        {
            tempprice = (int)(tempprice * 1.1);
        }
        tempprice = Mathf.FloorToInt(tempprice / 10) * 10;
        Debug.Log(tempprice.ToString());
        weightPrice = tempprice;
        tempprice = 100;
        for (int i = 0; i < spinLev; i++)
        {
            tempprice = (int)(tempprice * 1.1);
        }
        tempprice = Mathf.FloorToInt(tempprice / 10) * 10;
        Debug.Log(tempprice.ToString());
        spinPrice = tempprice;

        WeightLevelPriceText.text = weightPrice.ToString();
        SpinLevelPriceText.text = spinPrice.ToString();

        UndoGetButton.interactable = undoprice <= coin;
        WeightLevelupGetButton.interactable = weightPrice <= coin;
        SpinLevelupGetButton.interactable = spinPrice <= coin;

        PlayerPrefs.SetInt("UndoNumber", undoNum);
        PlayerPrefs.SetInt("WeightLevel", weightLev);
        PlayerPrefs.SetInt("SpinLevel", spinLev);
        PlayerPrefs.SetInt("Coin", coin);


    }
	// Update is called once per frame
	void Update () {
	
	}

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void GetUndo()
    {
        coin = coin - undoprice;
        undoNum++;
        RefreshParams();
    }

    public void WeightLevelUp()
    {
        coin = coin - weightPrice;
        weightLev++;
        RefreshParams();
    }
    public void SpinLevelUp()
    {
        coin = coin - spinPrice;
        spinLev++;
        RefreshParams();
    }
}
