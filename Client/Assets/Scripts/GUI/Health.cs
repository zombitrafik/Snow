using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	private int health = 100;
	// Use this for initialization
	private static Health instance;
	public static Health Instance {
		get {
			return instance;
		}
	}
	void Start () {
		instance = this;
	}

	public int getHealth(){
		return health;
	}

	public void remove(int a){
		health -= a;
	}

	public void DrawHealth(){
		GUI.Label(new Rect(80, Screen.height-20, 300, 20), ""+health);
	}
}
