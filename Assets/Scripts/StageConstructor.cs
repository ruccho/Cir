using UnityEngine;
using System.Collections;

public class StageConstructor : MonoBehaviour {
	//エディターから読み込み
	public TextAsset StageFile;
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


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


	}


	public GameObject initialize(){
		// <summary>StageConstructorに登録されたステージファイルでステージを初期化し、プレイヤーのGameObjectを返します。</summary>
		if(!ReadStage()) return null;
		return ConstructStage();
	}

	GameObject ConstructStage(){
		Vector2 targetPoint;
		int readcounter = 0;
		for(int j = 0; j > -(StageHeight); j--){
			for(int i = 0; i < StageWidth; i++){
				//Debug.Log("(i, j) = " + i.ToString() + ", " +  j.ToString());
				targetPoint = new Vector2((float)(i - (StageWidth / 2.0f - 0.5)), (float)(j + (StageHeight / 2.0f - 0.5)));

				switch(StageMap[readcounter]){
				case ('0'):
					//null。無効

					break;
				case ('1'):
					//empty。空白

					break;
				case ('2'):
					//filled。ブロック
					((GameObject)Instantiate(FilledPrefab, targetPoint, Quaternion.Euler(0,0,0))).transform.parent = this.transform;
					break;
				case ('3'):
					//start。スタートポジション
					PlayerObject = (GameObject)Instantiate(PlayerPrefab, targetPoint, Quaternion.Euler(0,0,0));
					PlayerObject.transform.parent = this.transform;
					break;
				case ('4'):
					//goal。ゴール
					((GameObject)Instantiate(GoalPrefab, targetPoint, Quaternion.Euler(0,0,0))).transform.parent = this.transform;
					break;
				}
				Debug.Log("座標:" + targetPoint.x.ToString() + ", " + targetPoint.y.ToString() + " 種類:" + StageMap[readcounter].ToString());
				readcounter++;
			}
		}
		readcounter = 0;
		return PlayerObject;
	}

	bool ReadStage(){
		
		if(!StageFile) return false;

		//テキストから情報を読み出し
		StageText = StageFile.text;
		if(StageText == "") return false;

		//先頭二文字から読み出して先頭の空白を削除し、サイズの純粋な数字を取得
		int parseresult;
		if(!(int.TryParse(StageText.Substring(0, 2).TrimStart(),out parseresult))) return false;
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
