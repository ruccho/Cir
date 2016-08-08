using UnityEngine;
using System.Collections;

public class StageConstructor : MonoBehaviour {
	//ファイルから読み出し
	string StageText;
	//ステージの横幅（最初２文字）
	int StageWidth;
	//ステージの縦幅
	int StageHeight;
	//ステージの構造情報本体
	string StageMap;


	GameObject PlayerObject;


	//各種構造物のPrefab
	public GameObject FilledPrefab;
	public GameObject GoalPrefab;
	public GameObject PlayerPrefab;
    public GameObject EmptyPrefab;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {


	}


	public GameObject initialize(GameObject MainCamera){
		/// <summary>StageConstructorに登録されたステージファイルでステージを初期化し、プレイヤーのGameObjectを返します。</summary>
		if(!ReadStage()) return null;
        if(StageWidth < StageHeight){
            MainCamera.GetComponent<Camera>().orthographicSize = StageHeight + 2;
            
        }else{
            MainCamera.GetComponent<Camera>().orthographicSize = StageWidth + 2;
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StageCreation")
        {
            MainCamera.GetComponent<Camera>().orthographicSize = 5;
        }
        return ConstructStage();
	}

	GameObject ConstructStage()
    {
        float offsetx = -(StageWidth / 2.0f - 0.5f);
        float offsety = -(StageHeight / 2.0f - 0.5f);
        Vector2 targetPoint;
		int readcounter = 0;
        
        //後で名前をつけるときに使用
        GameObject currentObject;
		for(int j = 0; j < StageHeight; j++){
			for(int i = 0; i < StageWidth; i++){
				//Debug.Log("(i, j) = " + i.ToString() + ", " +  j.ToString());
				targetPoint = new Vector2(i + offsetx, j + offsety);
                currentObject = null;
				switch(StageMap[readcounter]){
				case ('0'):
					//null。無効

					break;
				case ('1'):
                        //empty。空白
                        if (EmptyPrefab != null)
                        {
                            currentObject = (GameObject)Instantiate(EmptyPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
                        }
                        break;
				case ('2'):
                        //filled。ブロック
                        currentObject = (GameObject)Instantiate(FilledPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
					break;
				case ('3'):
					//start。スタートポジション
					PlayerObject = (GameObject)Instantiate(PlayerPrefab, targetPoint, Quaternion.Euler(0,0,0));
					PlayerObject.transform.parent = this.transform;
                        PlayerObject.name = i + "," + j;
                        break;
				case ('4'):
                        //goal。ゴール
                        currentObject = (GameObject)Instantiate(GoalPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
					break;
				}
                if (currentObject != null)
                {
                    currentObject.transform.parent = this.transform;
                    currentObject.name = i + "," + j;
                }
				Debug.Log("座標:" + targetPoint.x.ToString() + ", " + targetPoint.y.ToString() + " 種類:" + StageMap[readcounter].ToString());
				readcounter++;
			}
		}
		readcounter = 0;
		return PlayerObject;
	}

	bool ReadStage(){
		StageText = PlayerPrefs.GetString("CurrentStageText");
		if(StageText == "") return false;

		//先頭二文字から読み出して先頭の空白を削除し、サイズの純粋な数字を取得
		int parseresult;
		if(!(int.TryParse(StageText.Substring(0, 2), out parseresult))) return false;
		StageWidth = parseresult;
		Debug.Log("StageWidth:「" + StageWidth.ToString() + "」");

		//３文字目から最後までを切り取り、StageMapに格納
		StageMap = StageText.Substring(2);
		Debug.Log("StageMap:" + StageMap);

		//構造文字列の長さがWidthで割り切れるかチェックし、格納
		if(StageMap.Length % StageWidth != 0) return false;
		StageHeight = StageMap.Length / StageWidth;

		return true;
	}
}
