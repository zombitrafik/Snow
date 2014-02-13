using UnityEngine;
using System;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public GameObject lifeMeterFill;
	public Transform leftTransform;
	public Transform rightTransform;
	
	private int axis = 0;	
	private float leftPos;
	private float rightPos;
	
	private Transform thisTransform;
	private Transform fillTransform;
			
	private bool isEnabled = true;
	
	void Awake() {
		//fillRenderer = lifeMeterFill.renderer; 
		fillTransform = lifeMeterFill.transform;
		leftPos = leftTransform.localPosition[axis];
		rightPos = rightTransform.localPosition[axis];
	}

	public void SetValue(float f) {
		if (!isEnabled) return;
		
		f = Mathf.Clamp(f, 0, 1);
		Debug.Log ("progr setvalue " + f);
		Vector3 lp = fillTransform.localPosition;
		lp[axis] = Mathf.Lerp(leftPos, rightPos, f) + (1-f)/2;
		fillTransform.localPosition = lp;
		
		Vector3 sc = fillTransform.localScale;
		sc[axis] = f;
		fillTransform.localScale = sc;
	}
	
	public void Enable() {
		isEnabled = true;
		this.gameObject.SetActive (true);
		Debug.Log ("enable " + this.gameObject.activeInHierarchy);
	}
	
	public void Disable() {
		isEnabled = false;
		this.gameObject.SetActive(false);
		Debug.Log ("disable " + !this.gameObject.activeInHierarchy);
	}
	
}
