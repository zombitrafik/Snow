using UnityEngine;
using System.Collections;

public class DestroyBall : MonoBehaviour {
	public GameObject ObjToDel;
	private float start;
	private Vector3 pos,lastPos;
	private string[] temp;
	void Start(){
		start = Time.time;
	}


	// Use this for initialization
	void OnCollisionEnter(Collision collision) {
		temp = ObjToDel.name.Split (')');
		if ((collision.gameObject.name).Equals("Player(Clone)")){
			int damage = CalculateDamage (Time.time - start);
			Health.Instance.remove(damage);
			NetworkManager.Instance.SendHealthChange(temp[1],damage);
		}
		Destroy(ObjToDel);
	}


	int CalculateDamage(float shotTime){
		return Mathf.Clamp (((int)(1.25 / shotTime) * 3), 3, 20);

	}
}
