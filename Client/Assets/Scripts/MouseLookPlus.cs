using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


[AddComponentMenu("Infinite Camera-Control/Mouse Orbite with zoom")]
public class MouseLookPlus : MonoBehaviour {
	//
	private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	private RotationAxes axes = RotationAxes.MouseXAndY;
	private float sensitivityX = 5F;
	private float sensitivityY = 5F;


	public static RaycastHit hit;
	public static bool AttackAtFirstPersonView;
	
	public Transform Ball;
	
	private float minimumY = -60F;
	private float maximumY = 60F;
	
	private float rotationY = 0F;
	private GameObject player;

	public Transform target;
	public Transform CamPos;

	public Transform LeftPointToAttack;
	public Transform RightPointToAttack;
	
	private float xSpeed = 8.0f;
	private float ySpeed = 8.0f;
	
	private float scrollSpeed = 10.0f;
	
	private float zoomMin = 1.0f;
	private float zoomMax = 20.0f;
	
	public float distance = 2;
	private float Ypos;
	
	public Vector3 position;
	public bool isActivated;

	private bool check1 = false;
	private bool check2 = false;
	private Transform cam;
	
	private bool FirstPersonView;

	float x = 0.0f;
	float y = 0.0f;
	
	private string CamPosName;
	
	private static MouseLookPlus instance;

	public static MouseLookPlus Instance {
		get {
			return instance;
		}
	}
	void Start () {
		instance = this;

		player = PlayerManager.Instance.GetPlayerObject();
		player.transform.rigidbody.centerOfMass = GameObject.Find("CenterOfMass").transform.localPosition;
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10, player.transform.position.z -10);
		transform.Rotate(25,0,0);

		Vector3 angles = transform.eulerAngles;
		x = angles.x;
		y = angles.y;
		position = -(transform.forward*distance) + target.position;
		transform.position = position;
		Ypos = transform.position.y;

		Transform LeftPoint = (Transform) Instantiate(LeftPointToAttack,target.position+Vector3.left+Vector3.up*1.2f+Vector3.back, Quaternion.identity);
		LeftPoint.parent = GameObject.Find("Player(Clone)").transform;
		
		Transform RightPoint = (Transform) Instantiate(RightPointToAttack,target.position+Vector3.right+Vector3.up*1.2f+Vector3.back, Quaternion.identity);
		RightPoint.parent = GameObject.Find("Player(Clone)").transform;

	}


	void Update () {
		bool showEscapeMenu = MyGUI.showEscapeMenu;
		if(!showEscapeMenu){
			Screen.showCursor = false;
			if(!check1&&!check2){
				if(Input.GetMouseButtonDown(0)){
					AttackAtFirstPersonView = true;
				}
			} 
			if(Input.GetMouseButtonDown(1)){
				Transform NewCam = (Transform)Instantiate(CamPos, transform.position, transform.rotation); 
				NewCam.name+=PlayerManager.Instance.PlayerName();
				CamPosName=NewCam.name;
				NewCam.parent = GameObject.Find("Player(Clone)").transform;
				check1 = true;
				check2 = false;
			}
			if(check1) {
				TransformCameraToFirstPersonView();
			}
			if(FirstPersonView){
				FirstPersonCamRotate();
			}

		}

		if(Input.GetMouseButtonUp(1)){
				FirstPersonView = false;
				if(GameObject.Find(CamPosName)==true){
				cam = GameObject.Find(CamPosName).transform;
					check1=false;
					check2 = true;
				}
			}
		if(check2){
			TransformCameraToThirdPersonView();
		}
		if(GameObject.Find(CamPosName)==null&&!showEscapeMenu){
			AttackAtFirstPersonView = false;
			CamRotate();
		}
	}



	void TransformCameraToFirstPersonView(){
		Transform RightPoint = GameObject.Find("RightPointToAttack(Clone)").transform;
		//Transform LeftPoint = GameObject.Find("LeftPointToAttack(Clone)").transform;
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, RightPoint.position.x, 20*Time.deltaTime),Mathf.Lerp(transform.position.y, RightPoint.position.y, 20*Time.deltaTime),Mathf.Lerp(transform.position.z, RightPoint.position.z, 20*Time.deltaTime));//target.position;
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 20*Time.deltaTime);
		if  (transform.position==RightPoint.position){
			FirstPersonView = true;
			check1=false;
		} 
	}


	void TransformCameraToThirdPersonView(){
		if(cam.position==transform.position){
			foreach(GameObject i in GameObject.FindGameObjectsWithTag("EditorOnly")){
				Destroy(i);
		}
			check2 = false;
		}
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, cam.position.x, 20*Time.deltaTime),Mathf.Lerp(transform.position.y, cam.position.y, 20*Time.deltaTime),Mathf.Lerp(transform.position.z, cam.position.z, 20*Time.deltaTime));
		transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, 20*Time.deltaTime);
	}


	void FirstPersonCamRotate(){
			if (axes == RotationAxes.MouseXAndY) {
				float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				GameObject.Find("Player(Clone)").transform.Rotate(0, rotationX, 0);
				transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);
		
			}

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		if (Physics.Raycast(ray, out hit, 1000)){
			if(Input.GetMouseButtonDown(0)){
				Debug.Log(hit.distance);
			}
		}
	}
	void CamRotate(){
			x += Input.GetAxis("Mouse X") * xSpeed;
			y -= Input.GetAxis("Mouse Y") * ySpeed;
			transform.RotateAround(target.position, transform.up, x);
			transform.RotateAround(target.position, transform.right, y);

			transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(transform.localPosition.y,0,0.8f*distance), Mathf.Clamp(transform.localPosition.z,-20f,0));

			transform.LookAt(target);
			
			//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
			//transform.rotation = Quaternion.LookRotation(target.position - transform.position);
			//GameObject.Find("Cube").transform.rotation = Quaternion.LookRotation(target.position - transform.position);
			Ypos = transform.position.y;
		 	
			x=0;
			y=0;
			
			if (Input.GetAxis("Mouse ScrollWheel") != 0){
				
				distance = Vector3.Distance (transform.position, target.position);
				
				distance = ZoomLimit(distance - Input.GetAxis("Mouse ScrollWheel")*scrollSpeed, zoomMin, zoomMax);
				position = -(transform.forward*distance) + target.position;
				Ypos = position.y;
				transform.position = position;
				
			}
		
			float d2 = Vector3.Distance(transform.position, target.position);
			if(d2!=distance){
				position = -(transform.forward*distance) + target.position;
				position.y = Ypos;
				transform.position = position;
			}
	}

	public static float ZoomLimit(float dist, float min, float max){
		if (dist < min)
			dist = min;
		if (dist > max)
			dist = max;
		return dist;
	}

}