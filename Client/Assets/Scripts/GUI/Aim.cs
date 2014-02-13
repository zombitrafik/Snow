using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {
	public Texture2D AimPic;
	public GUISkin SkinForAim;

	private static Aim instance;
	private bool visible = false;

	public static Aim Instance{
		get{
			return instance;		
		}
	}
	public void ChangeVisible(){
		visible = !visible;
	}
	// Use this for initialization
	void Start () {
		instance = this;
	}
	public void DrawAim(){
		if (visible) {
			GUI.skin = SkinForAim;
			GUI.Box(new Rect(Screen.width/2-AimPic.width/2,Screen.height/2-AimPic.height/2,AimPic.width,AimPic.height), AimPic);		
		}
	}
}
