
using System;
using System.Collections;
using UnityEngine;

// Displaying enemy info like name and health
public class EnemyInfo : MonoBehaviour
{

	public ProgressBar lifeBar;
	public TextMesh enemyName;
	
	private Renderer[] renderers;
	
	void Awake() {
		renderers = this.GetComponentsInChildren<Renderer>();
	}
	
	public void SetName(string name) {
		this.enemyName.text = name;
	}
	
	public void SetLife(float val) {
		lifeBar.SetValue(val);
	}
	
	public void Hide() {
		foreach (Renderer rend in renderers) {
			rend.enabled = false;
		}
	}
	
	public void Show() {
		foreach (Renderer rend in renderers) {
			rend.enabled = true;
		}
	}
	
	void LateUpdate() {
		transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
	}
	
}

