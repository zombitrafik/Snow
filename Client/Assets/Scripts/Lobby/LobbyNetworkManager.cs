using System;
using System.Collections;
using UnityEngine;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Logging;

// The Neywork manager sends the messages to server and handles the response.
public class LobbyNetworkManager : MonoBehaviour
{
	private bool running = false;
	
	public readonly static string ExtName = "Snow";  // The server extension we work with
	public readonly static string ExtClass = "Extension.SnowExtension"; // The class name of the extension
	
	private static LobbyNetworkManager instance;
	public static LobbyNetworkManager Instance {
		get {
			return instance;
		}
	}
	
	private SmartFox smartFox;  // The reference to SFS client
	
	void Awake() {
		instance = this;	
	}
	
	void Start() {
		smartFox = SmartFoxConnection.Connection;
		if (smartFox == null) {
			return;
		}	
		
		SubscribeDelegates();
		
		running = true;
	}
	
	// This is needed to handle server events in queued mode
	void FixedUpdate() {
		if (!running) return;
		smartFox.ProcessEvents();
	}
	
	private void SubscribeDelegates() {
		smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
	}
	
	public void UnsubscribeDelegates() {
		smartFox.RemoveAllEventListeners();
	}

	public void SendChatMessage(string message){
		Room room = smartFox.LastJoinedRoom;
		SFSObject mes = new SFSObject();
		mes.PutUtfString ("message", message);
		ExtensionRequest request = new ExtensionRequest("chatMessage", mes, room);
		smartFox.Send(request);
	}

	/// <summary>
	/// When connection is lost we load the login scene
	/// </summary>
	private void OnConnectionLost(BaseEvent evt) {
		UnsubscribeDelegates();
		Screen.lockCursor = false;
		Screen.showCursor = true;
		Application.LoadLevel("Authorization");
	}
	
	// This method handles all the responses from the server
	private void OnExtensionResponse(BaseEvent evt) {
		try {
			string cmd = (string)evt.Params["cmd"];
			ISFSObject dt = (SFSObject)evt.Params["params"];
			if (cmd == "chatMessage") {
				HandleChatMessage(dt);
			}/*
			else if (cmd == "anim") {
				HandleAnimation(dt);
			}*/
		}
		catch (Exception e) {
			Debug.Log("Exception handling response: "+e.Message+" >>> "+e.StackTrace);
		}
		
	}
	
	private void HandleChatMessage(ISFSObject dt) {
		
		string message = dt.GetUtfString("message");
		Chat.Instance.UpdateChat (message);
	}

	// When a user leaves room destroy his object
	private void OnUserLeaveRoom(BaseEvent evt) {
		User user = (User)evt.Params["user"];
		//Room room = (Room)evt.Params["room"];
		
		PlayerManager.Instance.DestroyEnemy(user.Id);
		Debug.Log("User "+user.Name+" left");
	}
	
	void OnApplicationQuit() {
		UnsubscribeDelegates();
	}
}
