using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//Please Attach to Shop Root Canvas
public class ShopManager : MonoBehaviour
{

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

    public Button[] PaletteGetButtons;

    public GameObject WeightLevelPanel;
    public GameObject SpinLevelPanel;
    public GameObject[] PalettePanels;

    int undoNum;
    int weightLev;
    int spinLev;
    public int coin;
    int weightPrice;
    int spinPrice;
    int undoprice = 150;

    // Use this for initialization
    void Start()
    {
        undoNum = PlayerPrefs.GetInt("UndoNumber");
        weightLev = PlayerPrefs.GetInt("WeightLevel");
        //weightLev = 0;
        spinLev = PlayerPrefs.GetInt("SpinLevel");
        //spinLev = 0;
        coin = PlayerPrefs.GetInt("Coin");
        //coin = 10000;

        /*for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetInt("Palette" + (i + 1).ToString(), 0);
        }*/

        RefreshParams();


    }

    void RefreshParams()
    {
        CoinText.text = coin.ToString();

        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            CurrentUndoNumText.text = "現在の所持数：" + undoNum;
        }
        else
        {
            CurrentUndoNumText.text = "Current having：" + undoNum;
        }
        
        if (weightLev == 5)
        {
            WeightLevelPanel.SetActive(false);
        }
        if (spinLev == 20)
        {
            SpinLevelPanel.SetActive(false);
        }
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            WeightLevelTitleText.text = "ヘビー Lv" + (weightLev + 1).ToString();
            SpinLevelTitleText.text = "クイック Lv" + (spinLev + 1).ToString();
        }
        else
        {
            WeightLevelTitleText.text = "Heavy Lv" + (weightLev + 1).ToString();
            SpinLevelTitleText.text = "Quick Lv" + (spinLev + 1).ToString();
        }

        int tempprice = 100;
        for (int i = 0; i < weightLev; i++)
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
        PaletteGetButtons[0].interactable = 1000 <= coin;
        PaletteGetButtons[1].interactable = 1000 <= coin;
        PaletteGetButtons[2].interactable = 2000 <= coin;
        PaletteGetButtons[3].interactable = 2000 <= coin;

        for (int i = 0; i < 4; i++)
        {
            PalettePanels[i].SetActive((PlayerPrefs.GetInt("Palette" + (i + 1).ToString()) == 0));
            if((PlayerPrefs.GetInt("ClearedPresetStageNumber") >= 15) == false)
            {
                PalettePanels[i].SetActive(false);
            }
        }



        PlayerPrefs.SetInt("UndoNumber", undoNum);
        PlayerPrefs.SetInt("WeightLevel", weightLev);
        PlayerPrefs.SetInt("SpinLevel", spinLev);
        PlayerPrefs.SetInt("Coin", coin);


    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void GetUndo()
    {
        coin = coin - undoprice;
        undoNum++;
        RefreshParams();
    }

    public void GetPalette(int num)
    {
        switch (num)
        {
            case 0:
            case 1:
                coin -= 1000;
                break;
            case 2:
            case 3:
                coin -= 2000;
                break;
        }
        PlayerPrefs.SetInt("Palette" + (num + 1).ToString(), 1);
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
