using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageConstructor : MonoBehaviour
{
    /*//ファイルから読み出し
    string StageText;
    //ステージの横幅（最初２文字）
    int StageWidth;
    //ステージの縦幅
    int StageHeight;
    //ステージの構造情報本体
    string StageMap;*/

    StageStruct Stage;


    GameObject PlayerObject;


    //各種構造物のPrefab
    public GameObject FilledPrefab;
    public GameObject GoalPrefab;
    public GameObject PlayerPrefab;
    public GameObject EmptyPrefab;
    public GameObject KeyPrefab;
    public GameObject DoorPrefab;
    public GameObject OnewayUpPrefab;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }


    public GameObject initialize(GameObject MainCamera)
    {
        /// <summary>StageConstructorに登録されたステージファイルでステージを初期化し、プレイヤーのGameObjectを返します。</summary>
        if (!ReadStage()) return null;
        if (Stage.StageWidth < Stage.StageHeight)
        {
            MainCamera.GetComponent<Camera>().orthographicSize = Stage.StageHeight + 2;

        }
        else
        {
            MainCamera.GetComponent<Camera>().orthographicSize = Stage.StageWidth + 2;
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "StageCreation")
        {
            MainCamera.GetComponent<Camera>().orthographicSize = 5;
        }
        return ConstructStage();
    }

    GameObject ConstructStage()
    {
        float offsetx = -(Stage.StageWidth / 2.0f - 0.5f);
        float offsety = -(Stage.StageHeight / 2.0f - 0.5f);
        Vector2 targetPoint;
        int readcounter = 0;

        //後で名前をつけるときに使用
        GameObject currentObject;
        for (int j = 0; j < Stage.StageHeight; j++)
        {
            for (int i = 0; i < Stage.StageWidth; i++)
            {
                //Debug.Log("(i, j) = " + i.ToString() + ", " +  j.ToString());
                targetPoint = new Vector2(i + offsetx, j + offsety);
                currentObject = null;
                switch (Stage.StageBody[readcounter])
                {
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
                        PlayerObject = (GameObject)Instantiate(PlayerPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
                        PlayerObject.transform.parent = this.transform;
                        PlayerObject.name = i + "," + j;
                        break;
                    case ('4'):
                        //goal。ゴール
                        currentObject = (GameObject)Instantiate(GoalPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
                        break;
                    case ('5'):
                        //key。キー
                        currentObject = (GameObject)Instantiate(KeyPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
                        break;
                    case ('6'):
                        //door。ドア
                        currentObject = (GameObject)Instantiate(DoorPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
                        break;
                    case ('7'):
                        //oneway-up。上向き方向の一方通行
                        currentObject = (GameObject)Instantiate(OnewayUpPrefab, targetPoint, Quaternion.Euler(0, 0, 0));
                        break;
                    case ('8'):
                        //oneway-left。左向き方向の一方通行
                        currentObject = (GameObject)Instantiate(OnewayUpPrefab, targetPoint, Quaternion.Euler(0, 0, 90));
                        break;
                    case ('9'):
                        //oneway-down。下向き方向の一方通行
                        currentObject = (GameObject)Instantiate(OnewayUpPrefab, targetPoint, Quaternion.Euler(0, 0, 180));
                        break;
                    case ('a'):
                        //oneway-right。右向き方向の一方通行
                        currentObject = (GameObject)Instantiate(OnewayUpPrefab, targetPoint, Quaternion.Euler(0, 0, 270));
                        break;
                }
                if (currentObject != null)
                {
                    currentObject.transform.parent = this.transform;
                    currentObject.name = i + "," + j;
                }
                Debug.Log("座標:" + targetPoint.x.ToString() + ", " + targetPoint.y.ToString() + " 種類:" + Stage.StageBody[readcounter].ToString());
                readcounter++;
            }
        }
        readcounter = 0;
        return PlayerObject;
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
