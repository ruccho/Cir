using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageBorderConstructor : MonoBehaviour
{
    public GameObject blockPrefab;
    string StageText;
    int StageWidth;
    int StageHeight;
    string StageBody;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Construct()
    {
        int stageouterwidth;
        int stageouterheight;

        if (!ReadStage()) return;

        stageouterwidth = StageWidth + 2;
        stageouterheight = StageHeight + 2;


        //create bottom-border
        for (int i = 0; i < (stageouterwidth); i++)
        {
            putBlock(stageouterwidth, stageouterheight, i, 0);
        }

        //create left-border
        for (int i = 0; i < (stageouterheight - 2); i++)
        {
            putBlock(stageouterwidth, stageouterheight, 0, i + 1);
        }

        //create right-border
        for (int i = 0; i < (stageouterheight - 2); i++)
        {
            putBlock(stageouterwidth, stageouterheight, stageouterwidth - 1, i + 1);
        }

        //create top-border
        for (int i = 0; i < (stageouterwidth); i++)
        {
            putBlock(stageouterwidth, stageouterheight, i, stageouterheight - 1);
        }

    }

    void putBlock(int stageouterwidth, int stageouterheight, int x, int y)
    {
        //引数x,yは左下が(0,0)
        float offsetx = -(stageouterwidth / 2.0f - 0.5f);
        float offsety = -(stageouterheight / 2.0f - 0.5f);
        Vector2 targetPoint = new Vector2(offsetx + x, offsety + y);
        GameObject currentObject = (GameObject)Instantiate(blockPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
        currentObject.transform.parent = transform;
        //currentObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

    }

    bool ReadStage()
    {
        if (SceneManager.GetActiveScene().name == "StageCreaion")
        {
            StageText = (new StageStruct(PlayerPrefs.GetString("CurrentEditingStageQuery"))).StageText;
        }
        else
        {
            StageText = PlayerPrefs.GetString("CurrentStageText");
        }
        if (StageText == "") return false;

        //先頭二文字から読み出して先頭の空白を削除し、サイズの純粋な数字を取得
        int parseresult;
        if (!(int.TryParse(StageText.Substring(0, 2), out parseresult))) return false;
        StageWidth = parseresult;
        Debug.Log("StageWidth:「" + StageWidth.ToString() + "」");

        //３文字目から最後までを切り取り、StageMapに格納
        StageBody = StageText.Substring(2);
        Debug.Log("StageBody:" + StageBody);

        //構造文字列の長さがWidthで割り切れるかチェックし、格納
        if (StageBody.Length % StageWidth != 0) return false;
        StageHeight = StageBody.Length / StageWidth;

        return true;
    }
}
