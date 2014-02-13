using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour {
	private bool isActive = false;
	private static EscapeMenu instance;

	public static EscapeMenu Instance{
		get{
			return instance;		
		}
	}

	void Start () {
		instance = this;
	}

	public void changeActive(){
		isActive = !isActive;
		Screen.showCursor = isActive;
	}
	
	public void DrawEscapeMenu(){
		if (isActive) {
			GUI.Box(new Rect(Screen.width/2-65, Screen.height/2-150,130,150)," ");
			GUI.Box(new Rect(Screen.width/2-40, Screen.height/2-170,80,20),"Settings");
			
			//Graphics
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-145,100,20),"Graphics")){
				//Graphics menu
			}
			//Sound
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-120,100,20),"Sound")){
				//Sound menu
			}
			//HotKeys
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-95,100,20),"HotKeys")){
				//HotKeys menu
			}
			//Exit
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-70,100,20),"Exit")){
				Application.Quit();
			}
			//Back
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-30,100,20),"Back")){
				changeActive();
			}		
		}
	}
}