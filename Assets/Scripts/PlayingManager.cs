using UnityEngine;
using System.Collections;

public class PlayingManager : MonoBehaviour {
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

	bool isMoving = false;
	float timerRotationFinished;
	bool isRotationFinished = true;
	public float CoolTimeSeconds;
    public GameObject FadeImage;
    public float FadeTimeSeconds;
    public GameObject MainCamera;

	// Use this for initialization
	void Start () {
		stageConstructor = Stage.GetComponent<StageConstructor>();
		PlayerObject = stageConstructor.initialize(MainCamera);
        StageBorder.GetComponent<StageBorderConstructor>().Construct();
		if(PlayerObject == null) GameObject.Destroy(gameObject);
		targetRotation = Stage.transform.eulerAngles.z;
		tempRotation = Stage.transform.eulerAngles.z;
        
        //フェードアウト
        FadeImage.GetComponent<Fade>().FadeOut(FadeTimeSeconds);
	}

	// Update is called once per frame
	void Update () {


		//落下中かをチェック
		//VelocityのYがマイナス（落下中）の時で判定
		isMoving = ((int)PlayerObject.GetComponent<Rigidbody2D>().velocity.y < 0);
		//Debug.Log(PlayerObject.GetComponent<Rigidbody2D>().velocity.y.ToString());

		if(turningState == 1 && targetRotation > tempRotation){

			//回転中に設定
			isRotationFinished = false;

			//回転終了からの時間を記録するタイマーをリフレッシュ
			timerRotationFinished = 0f;

			//左回り設定で、左回りの余地あり
			tempRotation += rotationPerFrame;
		}else if(turningState == -1 && targetRotation < tempRotation){

			//回転中に設定
			isRotationFinished = false;

			//回転終了からの時間を記録するタイマーをリフレッシュ
			timerRotationFinished = 0f;

			//右回り設定で、右回りの余地あり
			tempRotation -= rotationPerFrame;
		}else{
			//それ以外、つまり回転終了
			turningState = 0;
			tempRotation = targetRotation;
			PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 1;

			//回転終了からの時間を記録するタイマーを更新
			timerRotationFinished += Time.deltaTime;

			//回転終了からの時間をチェック、クールタイムが終了していればisRotationFinishedをtrueに
			isRotationFinished = (timerRotationFinished >= CoolTimeSeconds);
		}

		Stage.transform.eulerAngles = new Vector3(0,0,tempRotation);

		//Debug.Log("回転が終了してから: " + timerRotationFinished + "秒");
	}

	public void RotateLeft(){
		if(turningState != 0 || isMoving || !isRotationFinished) return;//回転中or落下中or回転終了からすぐの場合は無効
		PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		turningState = 1;
		targetRotation = tempRotation + 90;
	}
	public void RotateRight(){
		if(turningState != 0 || isMoving || !isRotationFinished) return;//回転中or落下中or回転終了からすぐの場合は無効
		PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		turningState = -1;
		targetRotation = tempRotation - 90;

	}

	float FixRotation(float rotation){
		//回転の数値を0<=x<360に修正
		if(rotation >= 360){
			return rotation - 360;
		} else if(rotation < 0){
			return rotation + 360;
		}else{
			return rotation;
		}
		
	}

    public void switchCameraMode()
    {

    }

}
