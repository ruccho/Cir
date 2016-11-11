using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Advertisements;


public class PlayingManager : MonoBehaviour
{
    //rotationは左がプラス

    GameObject PlayerObject;
    StageConstructor stageConstructor;
    public GameObject Stage;
    public GameObject StageBorder;
    public float targetRotation = 0f;
    int turningState = 0;
    //90の約数を設定すること
    public float rotationPerFrame = 1;
    float tempRotation;
    public AudioClip RotateEnd;
    public AudioClip Open;


    bool RotateSEPlayed;
    bool isMoving = false;
    float timerRotationFinished;
    bool isRotationFinished = false;
    public float CoolTimeSeconds;
    public GameObject FadeImage;
    public float FadeTimeSeconds;
    public GameObject MainCamera;
    public GameObject MenuPanel;
    public GameObject InfoPanel;
    public GameObject ClearPanel;
    public GameObject ViewOnTwitterButton1;
    public GameObject ViewOnTwitterButton2;
    public GameObject SeeDetailButton;
    public GameObject StageTitleText;
    public GameObject StageDescriptionText;
    public GameObject RemainTurnText;
    public GameObject GameOverPanel;
    public GameObject BGMObject;
    public float DefaultGravity;

    bool perfectBonus = true;
    struct PlayState
    {
        public readonly Vector2 position;
        public readonly int stageRotation;

        public PlayState(Vector2 pos, int rotation)
        {
            position = pos;
            stageRotation = rotation;
        }
    }
    List<PlayState> StateHistory;
    bool historySaved = false;

    bool isCleared = false;
    float cameraZoomLevel;
    int turn_remain;
    private CameraModeType CameraMode;
    enum CameraModeType
    {
        BIRDSEYE, ZOOM
    }

    private PlaySceneModeType PlaySceneMode;
    enum PlaySceneModeType
    {
        PRESET, TWITTER, CODE, TEST
    }
    void Awake()
    {
#if (UNITY_ANDROID || UNITY_IOS)
        Advertisement.Initialize("1152993");
#endif
    }

    // Use this for initialization
    void Start()
    {
        StateHistory = new List<PlayState>();
        RotateSEPlayed = false;
        CameraMode = CameraModeType.BIRDSEYE;
        RefreshCameraView();
        stageConstructor = Stage.GetComponent<StageConstructor>();
        PlayerObject = stageConstructor.initialize(MainCamera);
        StageBorder.GetComponent<StageBorderConstructor>().Construct();
        //if(PlayerObject == null) GameObject.Destroy(Stage);
        targetRotation = Stage.transform.eulerAngles.z;
        tempRotation = Stage.transform.eulerAngles.z;
        cameraZoomLevel = MainCamera.GetComponent<Camera>().orthographicSize;
        ClearPanel.SetActive(false);
        if (new StageStruct(PlayerPrefs.GetString("CurrentStageQuery")).StageTurnCount == 0)
        {
            perfectBonus = false;
            turn_remain = -1;
        }
        else
        {
            turn_remain = new StageStruct(PlayerPrefs.GetString("CurrentStageQuery")).StageTurnCount;
        }
        refreshRemain();
        //プリセット・シェア・テストのいずれのプレイモードであるか
        switch (PlayerPrefs.GetString("CurrentStageInfo"))
        {
            case "PRESET":
                PlaySceneMode = PlaySceneModeType.PRESET;
                ViewOnTwitterButton1.SetActive(false);
                ViewOnTwitterButton2.SetActive(false);
                SeeDetailButton.SetActive(false);
                break;
            case "TWITTER":
                ViewOnTwitterButton1.SetActive(true);
                ViewOnTwitterButton2.SetActive(true);
                StageTitleText.GetComponent<Text>().text = new StageStruct(PlayerPrefs.GetString("CurrentStageQuery")).StageTitle;
                StageDescriptionText.GetComponent<Text>().text = new StageStruct(PlayerPrefs.GetString("CurrentStageQuery")).StageDescription;
                PlaySceneMode = PlaySceneModeType.TWITTER;
                break;
            case "CODE":
                PlaySceneMode = PlaySceneModeType.CODE;
                ViewOnTwitterButton1.SetActive(false);
                ViewOnTwitterButton2.SetActive(false);
                break;
            case "TEST":
                PlaySceneMode = PlaySceneModeType.TEST;
                ViewOnTwitterButton1.SetActive(false);
                ViewOnTwitterButton2.SetActive(false);
                break;
        }


        //フェードアウト
        FadeImage.GetComponent<Fade>().FadeOut(FadeTimeSeconds);
    }

    // Update is called once per frame
    void Update()
    {


        //落下中かをチェック
        //VelocityのYがマイナス（落下中）の時で判定
        isMoving = ((int)PlayerObject.GetComponent<Rigidbody2D>().velocity.y < 0);
        //Debug.Log(PlayerObject.GetComponent<Rigidbody2D>().velocity.y.ToString());


        if (turningState == 1 && targetRotation > tempRotation)
        {

            //回転中に設定
            isRotationFinished = false;
            historySaved = false;

            //回転終了からの時間を記録するタイマーをリフレッシュ
            timerRotationFinished = 0f;

            //左回り設定で、左回りの余地あり
            tempRotation += rotationPerFrame * Time.deltaTime * 60;
            if (targetRotation < tempRotation)
            {
                tempRotation = targetRotation;
            }
        }
        else if (turningState == -1 && targetRotation < tempRotation)
        {

            //回転中に設定
            isRotationFinished = false;
            historySaved = false;

            //回転終了からの時間を記録するタイマーをリフレッシュ
            timerRotationFinished = 0f;

            //右回り設定で、右回りの余地あり
            tempRotation -= rotationPerFrame * Time.deltaTime * 60;
            if (targetRotation > tempRotation)
            {
                tempRotation = targetRotation;
            }
        }
        else
        {
            //それ以外、つまり回転終了
            turningState = 0;
            tempRotation = targetRotation;
            PlayerObject.GetComponent<Rigidbody2D>().gravityScale = DefaultGravity;
            //回転終了からの時間を記録するタイマーを更新
            timerRotationFinished += Time.deltaTime;

            //回転終了からの時間をチェック、クールタイムが終了していればisRotationFinishedをtrueに
            isRotationFinished = (timerRotationFinished >= CoolTimeSeconds);
            if (isRotationFinished == true)
            {
                RotateSEPlayed = false;
            }
        }
        if (RotateSEPlayed == false && isRotationFinished == false && Mathf.Abs(targetRotation - tempRotation) < 30)
        {
            GetComponent<AudioSource>().PlayOneShot(RotateEnd);
            RotateSEPlayed = true;
        }
        //落下終了時一フレームのみ実行
        if (!historySaved && turningState == 0 && !isMoving && isRotationFinished)
        {
            StateHistory.Add(new PlayState(PlayerObject.transform.localPosition, (int)Stage.transform.localRotation.eulerAngles.z));
            historySaved = true;
        }

        GameOverCheck();

        Stage.transform.eulerAngles = new Vector3(0, 0, tempRotation);
        //Debug.Log("回転が終了してから: " + timerRotationFinished + "秒");
    }

    public void RotateLeft()
    {
        if (turningState != 0 || isMoving || !isRotationFinished) return;//回転中or落下中or回転終了からすぐの場合は無効
        turn_remain--;
        refreshRemain();
        PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        turningState = 1;
        targetRotation = tempRotation + 90;
    }
    public void RotateRight()
    {
        if (turningState != 0 || isMoving || !isRotationFinished) return;//回転中or落下中or回転終了からすぐの場合は無効
        turn_remain--;
        refreshRemain();
        PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        turningState = -1;
        targetRotation = tempRotation - 90;

    }

    float FixRotation(float rotation)
    {
        //回転の数値を0<=x<360に修正
        if (rotation >= 360)
        {
            return rotation - 360;
        }
        else if (rotation < 0)
        {
            return rotation + 360;
        }
        else
        {
            return rotation;
        }

    }

    public void switchCameraMode()
    {
        switch (CameraMode)
        {
            case CameraModeType.BIRDSEYE:
                CameraMode = CameraModeType.ZOOM;
                break;
            case CameraModeType.ZOOM:
                CameraMode = CameraModeType.BIRDSEYE;
                break;
        }
        GetComponent<AudioSource>().PlayOneShot(Open);
        RefreshCameraView();
    }

    void RefreshCameraView()
    {
        switch (CameraMode)
        {
            case CameraModeType.BIRDSEYE:
                MainCamera.transform.parent = null;
                MainCamera.GetComponent<Camera>().orthographicSize = cameraZoomLevel;
                MainCamera.transform.position = new Vector3(0, 0, MainCamera.GetComponent<Camera>().transform.position.z);
                break;
            case CameraModeType.ZOOM:
                MainCamera.transform.parent = PlayerObject.transform;
                MainCamera.GetComponent<Camera>().orthographicSize = 7;
                MainCamera.transform.localPosition = new Vector3(0, 0, MainCamera.GetComponent<Camera>().transform.position.z);
                break;
        }
    }

    public void OpenMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(Open);
        MenuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        GetComponent<AudioSource>().PlayOneShot(Open);
        MenuPanel.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        switch (PlaySceneMode)
        {
            case PlaySceneModeType.PRESET:
                SceneManager.LoadScene("PresetStageSelection");
                break;
            case PlaySceneModeType.TWITTER:
                SceneManager.LoadScene("ShareStageSelector");
                break;
            case PlaySceneModeType.CODE:
                SceneManager.LoadScene("ShareModeMenu");
                break;
            case PlaySceneModeType.TEST:
                SceneManager.LoadScene("StageCreation");
                break;
        }
    }

    public void Goal()
    {
        BGMObject.GetComponent<AudioSource>().enabled = false;
        if (PlaySceneMode == PlaySceneModeType.PRESET)
        {
            if (PlayerPrefs.GetInt("ClearedPresetStageNumber") < PlayerPrefs.GetInt("CurrentPresetStageNumber"))
            {
                PlayerPrefs.SetInt("ClearedPresetStageNumber", PlayerPrefs.GetInt("CurrentPresetStageNumber"));
            }
        }
        isCleared = true;
        ClearPanel.SetActive(true);
        if (PlaySceneMode == PlaySceneModeType.TEST)
        {
            PlayerPrefs.SetInt("isTested", 1);
        }
        ClearPanel.GetComponent<Animator>().SetTrigger("StageCleared");
    }

    public void ViewOnTwitter()
    {
        Application.OpenURL(PlayerPrefs.GetString("TweetURL"));
    }

    public void OpenInfoPanel()
    {
        InfoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        InfoPanel.SetActive(false);
    }

    void refreshRemain()
    {
        if (turn_remain <= -1)
        {
            if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                RemainTurnText.GetComponent<Text>().text = "回数制限なし";
            }else
            {
                RemainTurnText.GetComponent<Text>().text = "No limit";
            }
        }
        else
        {
            if(Application.systemLanguage == SystemLanguage.Japanese)
            {
                RemainTurnText.GetComponent<Text>().text = "あと" + turn_remain.ToString() + "回";
            }
            else
            {
                RemainTurnText.GetComponent<Text>().text = "remaining: " + turn_remain.ToString();
            }
        }
    }
    void GameOverCheck()
    {
        if (turningState != 0 || isMoving || !isRotationFinished || isCleared == true) return;
        if (turn_remain == 0)
        {
            Debug.Log("GAME OVER");
            GameOverPanel.SetActive(true);
        }
    }


    public void undo()
    {
        if (turningState != 0 || isMoving || !isRotationFinished) return;//回転中or落下中or回転終了からすぐの場合は無効
        if (StateHistory.Count <= 1) return;
        StateHistory.RemoveAt(StateHistory.Count - 1);
        if (turn_remain > -1) turn_remain++;
        refreshRemain();
        targetRotation = StateHistory[StateHistory.Count - 1].stageRotation;
        PlayerObject.transform.localPosition = StateHistory[StateHistory.Count - 1].position;
    }
#if (UNITY_ANDROID || UNITY_IOS)
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }else
        {
            Debug.Log("Advertisement is not ready");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                turn_remain = -1;
                refreshRemain();
                GameOverPanel.SetActive(false);
                perfectBonus = false;
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
#endif
}
