using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour {
	public static bool showEscapeMenu = false;
	private static MyGUI instance;

	public static MyGUI Instance{
		get{
			return instance;		
		}
	}
	void Start () {
		instance = this;
	}

	void OnGUI(){

		if (TimeManager.Instance == null) return;
		GUI.Label(new Rect(10, 30, 300, 20), "Ping: "+(int)TimeManager.Instance.AveragePing);

		Health.Instance.DrawHealth();
		Chat.Instance.DrawChat();
		Aim.Instance.DrawAim();
		EscapeMenu.Instance.DrawEscapeMenu();

		/*Show list of current players
		if(Input.GetKey(KeyCode.Tab)&&!showEscapeMenu){
			GUI.Box(new Rect(Screen.width/2-300, Screen.height/2-200,600,400)," ");
		}
		
		//Show Esc-menu
		*/


	}
}
