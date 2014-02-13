using UnityEngine;
using System.Collections;

public class LobbyGUI : MonoBehaviour {

	Rect startButtonPos = new Rect(Screen.width/2 - 50,Screen.height-80, 100, 40);

	// Use this for initialization
	void Start () {
	}

	void OnGUI(){
		if (GUI.Button(startButtonPos, "Start")) {
			try {
				LobbyNetworkManager.Instance.UnsubscribeDelegates();
				Application.LoadLevel("Game");
			}
			catch (ExitGUIException e) {
			}	
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
