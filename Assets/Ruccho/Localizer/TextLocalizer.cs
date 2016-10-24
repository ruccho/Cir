using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Text))]

/// <summary>
/// ジェネリックを隠すために継承してしまう
/// [System.Serializable]を書くのを忘れない
/// </summary>
[System.Serializable]
public class LocalizeTable : Serialize.TableBase<SystemLanguage, string, LocalizePair>
{


}

/// <summary>
/// ジェネリックを隠すために継承してしまう
/// [System.Serializable]を書くのを忘れない
/// </summary>
[System.Serializable]
public class LocalizePair : Serialize.KeyAndValue<SystemLanguage, string>
{

    public LocalizePair(SystemLanguage key, string value) : base(key, value)
    {

    }
}

public class TextLocalizer : MonoBehaviour
{
    public LocalizeSettings Settings;

    SystemLanguage lang;
    SystemLanguage tmpLang;
    public LocalizeTable table;
    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

        lang = Application.systemLanguage;
#if UNITY_EDITOR
        if(Settings.isDebugging)lang = Settings.LangForDebug;
#endif
        string outText;
        if (table.GetTable().TryGetValue(lang, out outText))
        {
            GetComponent<Text>().text = outText;
        }
        else
        {
            if (table.GetTable().TryGetValue(Settings.DefaultLanguage, out outText))
            {
                GetComponent<Text>().text = outText;
            }
#if UNITY_EDITOR
                if (tmpLang != Settings.LangForDebug)
            {
                Debug.LogWarning("Could not find text localized to current language");
            }
#endif
        }
#if UNITY_EDITOR
        tmpLang = Settings.LangForDebug;
#endif
    }
}
