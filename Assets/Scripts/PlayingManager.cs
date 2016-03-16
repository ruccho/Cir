using UnityEngine;
using System.Collections;

public class PlayingManager : MonoBehaviour {
	//rotationは左がプラス

	public GameObject PlayerPrefab;
	public GameObject PlayerObject;
	public GameObject StartPos;
	public GameObject Stage;
	public float targetRotation = 0f;
	int turningState = 0;
	//90の約数を設定してください
	public float rotationPerFrame = 1;
	float tempRotation;

	// Use this for initialization
	void Start () {
		PlayerObject = (GameObject)Instantiate(PlayerPrefab, StartPos.transform.position, StartPos.transform.rotation);
		PlayerObject.transform.parent = Stage.transform;
		targetRotation = Stage.transform.eulerAngles.z;
		tempRotation = Stage.transform.eulerAngles.z;
	}

	// Update is called once per frame
	void Update () {
		/*
		if (System.Math.Abs(targetRotation % 360) == 0 || System.Math.Abs(targetRotation % 360) == 180) {
			PlayerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			PlayerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
		}
		if (System.Math.Abs(targetRotation % 360) == 90 || System.Math.Abs(targetRotation % 360) == 270){
			PlayerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			PlayerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
		}*/
		if(turningState == 1 && targetRotation > tempRotation){
			//左回り設定で、左回りの余地あり
			tempRotation += rotationPerFrame;
		}else if(turningState == -1 && targetRotation < tempRotation){
			//右回り設定で、右回りの余地あり
			tempRotation -= rotationPerFrame;
		}else{
			//それ以外、つまり回転終了
			turningState = 0;
			tempRotation = targetRotation;
			PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 1;

		}

		Stage.transform.eulerAngles = new Vector3(0,0,tempRotation);

		/*
		Debug.Log(isTurning.ToString());
		if(isTurning){
			Quaternion stageRotation = Stage.transform.rotation;
			if(stageRotation.z < targetRotation){
				//左回り中
				Stage.transform.Rotate(new Vector3(0,0,stageRotation.z + rotationPerFrame));
			}else if(stageRotation.z > targetRotation){
				//右回り中
				Stage.transform.Rotate(new Vector3(0,0,stageRotation.z - rotationPerFrame));
			}else{
				//回転終了
				isTurning = false;
				Player.GetComponent<Rigidbody2D>().WakeUp();
			}
		}*/
	}

	public void RotateLeft(){
		if(turningState != 0) return;//回転中の場合は無効
		PlayerObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		turningState = 1;
		targetRotation = tempRotation + 90;
	}
	public void RotateRight(){
		if(turningState != 0) return;//回転中の場合は無効
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
}
