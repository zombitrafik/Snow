using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Attack : MonoBehaviour {
	public AnimationClip a_attack;
	public float a_attackSpeed;
	public Transform Ball;
	private float r = 0.2f;
	public Transform spep;
	private RaycastHit hitSec;

	public static Attack instance;

	public static Attack Instance {
		get {
			return instance;
		}
	}

	void Start () {
		instance = this;
		animation[a_attack.name].speed = a_attackSpeed;
	}
	

	void Update () {
		if(!MyGUI.showEscapeMenu){
			if(Input.GetKey(KeyCode.Mouse0)){
				if (!animation.IsPlaying(a_attack.name)){
					animation.Play(a_attack.name);
					r = 0.2f;
					r = 0.2f;
				}
			}
			if(animation[a_attack.name].time>r){
				Transform start = GameObject.Find("SpawnBalls").transform;;
				Vector3 end = Vector3.zero;

				if(MouseLookPlus.AttackAtFirstPersonView){
					end = MouseLookPlus.hit.point + new Vector3(0,(0.168f-(50-Mathf.Clamp(MouseLookPlus.hit.distance,0,50))*0.00335f)*Mathf.Clamp(MouseLookPlus.hit.distance,0,50),0);
					GameObject.Find("SpawnBalls").transform.LookAt(end);
					Transform newBall = (Transform) Instantiate(Ball, start.position, Quaternion.identity);
					newBall.name +=PlayerManager.Instance.PlayerName();
					newBall.rigidbody.AddForce(start.forward*2f);
				} else {
					Ray ray = new Ray(start.position, GameObject.Find("Spawn").transform.position);
					if(Physics.Raycast(ray, out hitSec, 1000)){
						end = hitSec.point + new Vector3(0,(0.168f-(50-hitSec.distance)*0.00335f)*hitSec.distance,0);
						start.LookAt(end);
						Transform newBall = (Transform) Instantiate(Ball, start.position, Quaternion.identity);
						newBall.name +=PlayerManager.Instance.PlayerName();
						newBall.rigidbody.AddForce(start.forward*2f);
					}
				}
				NetworkManager.Instance.SendShot(start.position,end);

				r = 1f;
			}
		}
	}


	public void shot(Vector3 startPoint, Vector3 endPoint, string name){
		Transform sp = (Transform) Instantiate(spep, startPoint, Quaternion.identity);
		Transform ep = (Transform) Instantiate(spep, endPoint, Quaternion.identity);
		sp.LookAt(ep);
		Transform newBall = (Transform) Instantiate(Ball, sp.position, Quaternion.identity);
		newBall.name +=name;
		newBall.rigidbody.AddForce(sp.forward*2f);
		GameObject[] arr = GameObject.FindGameObjectsWithTag("Finish");
		for(int i=0;i<arr.Length;i++){
			Destroy(arr[i]);
		}
	}


}


