using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    private Color ColorToReplaceObj = new Color(0, 0, 0);
    private Color ColorToReplaceBg = new Color(1, 1, 1);
    private Color SelectingColor = new Color(0, 0, 0);

    public Image BGColorIndicator;
    public Image ObjColorIndicator;
    public Image SelColorIndicator;

    //Vivid(Default On) > Pastel > Dark > Shibumi1 > Shibumi2
    private bool[] PalettesAvailability = new bool[5] { true, false, false, false, false };
    private string[] PaletteNames = new string[5] { "Vivid", "Pastel", "Dark", "Shibumi1", "Shibumi2" };
    public Color[] VividPalette;
    public Color[] PastelPalette;
    public Color[] DarkPalette;
    public Color[] Shibumi1Palette;
    public Color[] Shibumi2Palette;
    public Image[] PaletteNodes;
    private int CurrentPalette = 1;

    public Text PaletteTitleText;
    public ErrorDialog errorDialog;

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("IsColorCustomized"))
        {
            if (PlayerPrefs.GetInt("IsColorCustomized") == 1)
            {
                //取得
                ColorToReplaceObj.r = PlayerPrefs.GetFloat("CObjRed");
                ColorToReplaceObj.g = PlayerPrefs.GetFloat("CObjGreen");
                ColorToReplaceObj.b = PlayerPrefs.GetFloat("CObjBlue");
                ColorToReplaceBg.r = PlayerPrefs.GetFloat("CBgRed");
                ColorToReplaceBg.g = PlayerPrefs.GetFloat("CBgGreen");
                ColorToReplaceBg.b = PlayerPrefs.GetFloat("CBgBlue");

                refreshPalette();

            }
        }
        BGColorIndicator.color = ColorToReplaceBg;
        ObjColorIndicator.color = ColorToReplaceObj;



    }

    void refreshPalette()
    {
        //アンロックしたパレットを確認
        for (int i = 0; i < PalettesAvailability.Length; i++)
        {
            if (PlayerPrefs.HasKey("Palette" + i.ToString()))
            {
                PalettesAvailability[i] = (PlayerPrefs.GetInt("Palette" + i.ToString()) == 1);
            }
            else
            {
                PalettesAvailability[i] = false;
            }
        }
        PalettesAvailability[0] = true;

        //現在のパレットで画面を更新
        for (int i = 0; i < PaletteNodes.Length; i++)
        {
            switch (CurrentPalette)
            {
                case 1:
                    PaletteNodes[i].color = VividPalette[i];
                    break;
                case 2:
                    PaletteNodes[i].color = PastelPalette[i];
                    break;
                case 3:
                    PaletteNodes[i].color = DarkPalette[i];
                    break;
                case 4:
                    PaletteNodes[i].color = Shibumi1Palette[i];
                    break;
                case 5:
                    PaletteNodes[i].color = Shibumi2Palette[i];
                    break;

            }
            PaletteTitleText.text = PaletteNames[CurrentPalette - 1];
            SelColorIndicator.color = SelectingColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Close()
    {
        if (ColorToReplaceBg == ColorToReplaceObj)
        {
            errorDialog.OpenDialog("背景と物体に同じ色が設定されています。違う色を設定してください。");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SelectColor(Image ColorNode)
    {
        SelectingColor = ColorNode.color;
        SelColorIndicator.color = SelectingColor;
    }

    public void ApplyBG()
    {
        ColorToReplaceBg = SelectingColor;
        BGColorIndicator.color = ColorToReplaceBg;
        PlayerPrefs.SetFloat("CBgRed", ColorToReplaceBg.r);
        PlayerPrefs.SetFloat("CBgGreen", ColorToReplaceBg.g);
        PlayerPrefs.SetFloat("CBgBlue", ColorToReplaceBg.b);
        PlayerPrefs.SetInt("IsColorCustomized", 1);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ColorReplace>().refreshColor();
    }

    public void ApplyObj()
    {
        ColorToReplaceObj = SelectingColor;
        ObjColorIndicator.color = ColorToReplaceObj;
        PlayerPrefs.SetFloat("CObjRed", ColorToReplaceObj.r);
        PlayerPrefs.SetFloat("CObjGreen", ColorToReplaceObj.g);
        PlayerPrefs.SetFloat("CObjBlue", ColorToReplaceObj.b);
        PlayerPrefs.SetInt("IsColorCustomized", 1);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ColorReplace>().refreshColor();
    }

    public void NextPalette()
    {
        while (true)
        {
            if (CurrentPalette == 5)
            {
                //最終ページの場合
                CurrentPalette = 1;
            }
            else
            {
                CurrentPalette++;
            }
            if (PalettesAvailability[CurrentPalette - 1]) break;
        }
        refreshPalette();
    }

    public void PreviousPalette()
    {
        while (true)
        {
            if (CurrentPalette == 1)
            {
                //最終ページの場合
                CurrentPalette = 5;
            }
            else
            {
                CurrentPalette--;
            }
            if (PalettesAvailability[CurrentPalette - 1]) break;
        }
        refreshPalette();
    }
}
