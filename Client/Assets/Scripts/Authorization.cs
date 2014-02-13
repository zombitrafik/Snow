using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;

public class Authorization : MonoBehaviour {
	public Font myFont;             //для русского языка
	private SmartFox smartFox;         //наш коннектор
	private string serverName = "127.0.0.1";  //адрес сервера
	private string serverPort = "9933";
	private string zone = "Snow";     //название зоны
	private string username = "kimreik";     //имя пользователя по умолчанию
	private string password = "zxczxc";    //пароль по умолчанию
	private Room currentRoom;
	private String roomName = "";
	public delegate void ExtensionResponceDelegate(string cmd, SFSObject parameters);

	void Start() {
		Application.runInBackground = true;
		Application.targetFrameRate = 50;
		if (SmartFoxConnection.IsInitialized) {
			smartFox = SmartFoxConnection.Connection;
		}
		else {
			smartFox = new SmartFox();
		}
	}



	void OnGUI() {
			GUI.skin.font = myFont;                 //задаем шрифт
			GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, 
			                             Screen.height / 2 - 150, 300, 300), "", "box");   //рисуем область отображения
			GUILayout.BeginVertical();               //начинаем вертикальную группу
			GUILayout.Label("Server IP");            //далее идут пары Label+TextField для считывания данных
			serverName = GUILayout.TextField(serverName);
			GUILayout.Label("Server Port");
			serverPort = GUILayout.TextField(serverPort);

			GUILayout.Label("Login");
			username = GUILayout.TextField(username, 24);
			GUILayout.Label("Password");
			password = GUILayout.PasswordField(password, "*"[0], 24);
			//проверяем заполненность всех данных, если true - отображаем кнопку соединения
			if (serverName != "" && serverPort != "" && username != "" && password != "") {
				if (GUILayout.Button("Connect")) {
					AddEventListeners();
					try {
						smartFox.Connect(serverName, int.Parse(serverPort));
					}
					catch (Exception) {
						AddEventListeners();
					}	
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
	}

	private void AddEventListeners() {
		smartFox.RemoveAllEventListeners();
		smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);   
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost); 
		smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);  
		smartFox.AddEventListener(SFSEvent.LOGOUT, OnLogout); 
		smartFox.AddEventListener(SFSEvent.UDP_INIT, OnUdpInit); 
		smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom); 
	}
	private void UnregisterSFSSceneCallbacks() {
		smartFox.RemoveAllEventListeners();
	}

	///EVENTS
	public void OnConnection(BaseEvent evt) {
		bool success = (bool)evt.Params["success"];
		if (success) {
						SmartFoxConnection.Connection = smartFox;
						//отправляем наши имя, пустой пароль и название зоны для присоединения
						smartFox.Send (new LoginRequest (username, password, zone));
				}

			
	}
	
	public void OnConnectionLost(BaseEvent evt) {
		UnregisterSFSSceneCallbacks();
	}
	public void OnLogin(BaseEvent evt) {
		try {
			if (evt.Params.Contains("success") && !(bool)evt.Params["success"]) {
			}
			else {
				smartFox.InitUDP(serverName, int.Parse(serverPort));
			}
		}
		catch (Exception) {}
	}

	void OnLogout(BaseEvent evt) {
		smartFox.Disconnect();
	}
	
	public void OnUdpInit(BaseEvent evt) {
		if (evt.Params.Contains("success") && !(bool)evt.Params["success"]) {
		}
		else {
			SetupRoomList();//берем список комнат
		}
	}

	public void OnJoinRoom(BaseEvent evt) {
		//Application.LoadLevel("Game");
		Application.LoadLevel("Lobby");
	}

	private void SetupRoomList() {
		List<Room> allRooms = smartFox.RoomManager.GetRoomList();
		foreach (Room room in allRooms) {
			if (!room.IsGame)
				continue;
			roomName = room.Name;
			break;
		}
		if (roomName=="") {
			roomName = smartFox.MySelf.Name + " game";
			short numMaxUsers = 100;
			RoomSettings settings = new RoomSettings(roomName);
			settings.GroupId = "default";
			settings.IsGame = true;
			settings.MaxUsers = numMaxUsers;
			settings.MaxSpectators = 0;
			settings.Extension = new RoomExtension("Snow", "Extension.SnowExtension");
			smartFox.Send(new CreateRoomRequest(settings, true, smartFox.LastJoinedRoom));
		}
		else {
			smartFox.Send(new JoinRoomRequest(roomName));
		}
	}

	void FixedUpdate() {
		smartFox.ProcessEvents();
	}

}