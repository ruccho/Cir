using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageBorderConstructor : MonoBehaviour
{
    public GameObject blockPrefab;
    /*string StageText;
    int StageWidth;
    int StageHeight;
    string StageBody;*/
    StageStruct Stage;
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

        stageouterwidth = Stage.StageWidth + 2;
        stageouterheight = Stage.StageHeight + 2;


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
        if (SceneManager.GetActiveScene().name == "StageCreation")
        {
            Stage = new StageStruct(PlayerPrefs.GetString("CurrentEditingStageQuery"));
        }
        else
        {
            Stage = new StageStruct(PlayerPrefs.GetString("CurrentStageQuery"));
            //Stage = new StageStruct(PlayerPrefs.GetString("CurrentStageText"), "", "");
        }
        if (Stage.isValid == false) return false;
        return true;
    }
}
