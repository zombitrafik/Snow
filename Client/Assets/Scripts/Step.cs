using UnityEngine;
using System.Collections;

public class Step : MonoBehaviour {
	public static float Health = 100.0f;
	public float speed = 10.0f;
	public float rotationSpeed = 100.0f;
	private float translation;
	private float rotation;

	private float lastJump;

	private static Step instance;

	public static Step Instance{
		get{
			return instance;
		}
	}


	void Start () {
		instance=this;
	}

	// 1 - forward, 2 - right, 3 - back, 4 - left

	public void Move(int direction, float deltaTime){
		translation=deltaTime*speed;
		switch(direction){
			case 1:
				transform.Translate (0, 0, translation);	
			break;
			case 2:
				transform.Translate(translation,0,0);
			break;
			case 3:
				transform.Translate (0, 0, -translation);	
			break;
			case 4:
			transform.Translate (-translation, 0, 0);
			break;
		}
	}

	// -1 - left, 1 - right

	public void Rotate(int direction, float deltaTime){
		rotation=deltaTime*rotationSpeed;
		if(direction==1){
			transform.Rotate(0,rotation,0);
		}else{
			transform.Rotate(0,-rotation,0);
		}
	}

	public void Jump(){
		if(Time.time-lastJump>=2){
			transform.rigidbody.velocity = new Vector3(0,8,0);
			lastJump=Time.time;	
		}
	}

}