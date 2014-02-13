using UnityEngine;
using System.Collections.Generic;

public class InputSystem : MonoBehaviour {
	private bool isGUI=false;
	private float lastRemoveTime=0;
	private static InputSystem instance;

	public static InputSystem Instance{
		get{
			return instance;		
		}
	}
	
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(isGUI){
				Chat.Instance.SetActive(false);
				isGUI=false;
			}else{
				EscapeMenu.Instance.changeActive();
			}
			return;
		}
		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown ("return")) {
			Chat.Instance.SetActive(!isGUI);
			isGUI=!isGUI;
			return;
		}

		if(isGUI){
			if(Input.GetKey(KeyCode.Backspace)){
				if(Time.time-lastRemoveTime>0.1){
					Debug.Log("proc");
					lastRemoveTime=Time.time;
					Chat.Instance.removeFromTextField();
				}
				return;
			}
			if(Input.GetKeyUp(KeyCode.Backspace)){
				lastRemoveTime=0;
			}
			Chat.Instance.addToTextField(Input.inputString);
		}else{
			foreach (KeyValuePair<string,KeyCode> bind in BindSystem.Instance.bindsList) {
				if(Input.GetKeyDown(bind.Value)) keyDown(bind.Key); //поменять имя
				if(Input.GetKeyUp(bind.Value)) keyUp(bind.Key); //поменять имя
				if(Input.GetKey(bind.Value)) justKey(bind.Key); //поменять имя
			}	
		}

	}
	private void keyDown(string key){
		switch(key){
			case "shot":
				break;
			case "aim": 
				Aim.Instance.ChangeVisible();
				break;
	
		}
	}
	
	private void keyUp(string key){
		switch(key){
		case "shot":
			break;
		case "aim": 
			Aim.Instance.ChangeVisible();
			break;
			
		}
	}

	private void justKey(string key){
		switch(key){
		case "moveLeft":
			Step.Instance.Move(4,Time.deltaTime);
			break;
		case "moveRight":
			Step.Instance.Move(2,Time.deltaTime);
			break;
		case "turnLeft":
			Step.Instance.Rotate(-1,Time.deltaTime);
			break;
		case "turnRight":
			Step.Instance.Rotate(1,Time.deltaTime);
			break;
		case "moveForward":
			Step.Instance.Move(1,Time.deltaTime);
			break;
		case "moveBack":
			Step.Instance.Move(3,Time.deltaTime);
			break;
		case "jump":
			Step.Instance.Jump();
			break;
		}
	}

}
