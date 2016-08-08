using UnityEngine;
using System.Collections;

public class StageCreation : MonoBehaviour {
    public GameObject StageObject;
    public GameObject StageBorderObject;
    public GameObject MainCamera;
    public Sprite[] CreationSprites;
    int StageWidth;
    int StageHeight;
    int[,] StageMap;

    string ClickedName = "";
    GameObject obj;

    BrushModeType BrushMode = BrushModeType.Empty;
    //この順で！
    enum BrushModeType
    {
        Null, Empty, Filled, Start, Goal
    }

	// Use this for initialization
	void Start () {
        StageObject.GetComponent<StageConstructor>().initialize(MainCamera);
        StageBorderObject.GetComponent<StageBorderConstructor>().Construct();

        string StageText = PlayerPrefs.GetString("CurrentStageText");
        if (StageText == "") return;

        //先頭二文字から読み出して先頭の空白を削除し、サイズの純粋な数字を取得
        int parseresult;
        if (!(int.TryParse(StageText.Substring(0, 2), out parseresult))) return;
        StageWidth = parseresult;

        //構造文字列の長さがWidthで割り切れるかチェックし、格納
        if (StageText.Substring(2).Length % StageWidth != 0) return;
        StageHeight = StageText.Substring(2).Length / StageWidth;

        //(0,0)が左下
        StageMap = new int[StageWidth, StageHeight];
        int readcounter = 0;
        for(int i = 0; i < StageHeight; i++)
        {
            for(int j = 0;j < StageWidth; j++)
            {
                StageMap[j, i] = int.Parse(StageText.Substring(readcounter + 2, 1));
                readcounter++;
            }
        }
        return;
    }
	
	// Update is called once per frame
	void Update () {
        //クリックしたオブジェクト名の取得
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 aTapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

            if (aCollider2d)
            {
                obj = aCollider2d.transform.gameObject;
                ClickedName = obj.name;
                if (ClickedName == "") return;
                //オブジェクト名から座標を取得
                string[] tmpStr = ClickedName.Split(',');
                if (tmpStr.Length != 2) return;
                int x = int.Parse(tmpStr[0]);
                int y = int.Parse(tmpStr[1]);
                StageMap[x, y] = (int)BrushMode;
                obj.GetComponent<SpriteRenderer>().sprite = CreationSprites[(int)BrushMode];
            }
        }
        
    }

    public void SwitchBrush()
    {

    }

}
