using UnityEngine;
using System.Collections.Generic;

public class BindSystem : MonoBehaviour {
	public IDictionary<string, KeyCode> bindsList = new Dictionary<string, KeyCode>(); 
	private static BindSystem instance;

	public static BindSystem Instance{
		get{
			return instance;		
		}
	}

	// Use this for initialization
	void Start () {
		//NetworkManager.Instance.SendBindsRequest(); через отдельный общий для инфы класс может быть.
		instance = this;
		bindsList.Add ("shot", KeyCode.Mouse0);
		bindsList.Add ("aim", KeyCode.Mouse1);
		bindsList.Add("moveLeft", KeyCode.Q);
		bindsList.Add("moveRight", KeyCode.E);
		bindsList.Add("turnLeft", KeyCode.A);
		bindsList.Add("turnRight", KeyCode.D);
		bindsList.Add("moveForward",KeyCode.W);
		bindsList.Add("moveBack",KeyCode.S);
		bindsList.Add("jump",KeyCode.Space);
	}

}
