using UnityEngine;
using System.Collections;

public class Chat : MonoBehaviour {

	public GUIStyle style;

	private static Chat instance;
	private string chatText = "";
	private string messageText = "";
	private bool isActive=false;
	private Rect areaRect = new Rect(10,Screen.height-143,300,100);
	private Rect fieldRect = new Rect(10,Screen.height-40,300,30);

	public static Chat Instance{
		get{
			return instance;		
		}
	}
	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void UpdateChat(string newMessage){
		chatText += newMessage;
	}

	public void addToTextField(string val){
		messageText+=val;
	}

	public void removeFromTextField(){
		if(messageText.Length!=0){
			messageText=messageText.Remove(messageText.Length-1);
		}
	}

	public void SetActive(bool val){
		messageText="";
		if(!val && !messageText.Equals("")){
			LobbyNetworkManager.Instance.SendChatMessage(messageText);
		}
		isActive = val;
	}

	public void DrawChat(){
		GUI.TextArea (areaRect, chatText,style);
		if(isActive){
				messageText = GUI.TextField (fieldRect, messageText);
		}
	}
}
