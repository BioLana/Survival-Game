using UnityEngine;
using System.Collections;


public class WeaponSway : MonoBehaviour {
	public float MoveAmount  = 8;
	public float MoveSpeed  = 1;
	[HideInInspector]
	public Transform GUN;
	[HideInInspector]
	public float MoveOnX ;
	[HideInInspector]
	public float MoveOnY ;
	[HideInInspector]
	public Vector3 DefaultPos ;
	[HideInInspector]
	public Vector3 NewGunPos ;
	
	bool walking;

	[System.Serializable]
	public class AnimationSettings
	{
		public Animator anim;
		public Vector3 walkRightPos;
		public Vector3 walkLeftPos;
		public float walkAnimSpeed = 5;

		public Vector3 runRightPos;
		public Vector3 runLeftPos;
		public float runAnimSpeed = 10;

		public Vector3 crouchRightPos;
		public Vector3 crouchLeftPos;
		public float crouchAnimSpeed= 2;
	}
	public AnimationSettings animationSettings = new AnimationSettings();
	float timer;
	void Start(){
		DefaultPos = transform.localPosition;
		GUN = transform;
	}            
	
	
	
	
	void Update () {
		MoveOnX = Input.GetAxis("Mouse X") * Time.deltaTime * MoveAmount;
		MoveOnY = Input.GetAxis("Mouse Y") * Time.deltaTime * MoveAmount;
		NewGunPos = new Vector3 (DefaultPos.x+MoveOnX, DefaultPos.y+MoveOnY, DefaultPos.z);
		GUN.transform.localPosition = Vector3.Lerp(GUN.transform.localPosition, NewGunPos, MoveSpeed*Time.deltaTime);

		float inputY = Input.GetAxis("Vertical");
		float inputX = Input.GetAxis("Horizontal");
		timer -= Time.deltaTime;


		if (inputX > 0.1f || inputX  < -0.1f || inputY > 0.1f || inputY  < -0.1f) {
			walking = true;
		}else{
			walking = false;
		}
		if (walking == true && GetComponent<Weapon> ().aiming == false && animationSettings.anim.GetBool ("Running") == false && animationSettings.anim.GetBool ("Crouching") == false) {
			transform.localPosition = Vector3.Lerp (animationSettings.walkLeftPos, animationSettings.walkRightPos, .5f + (Mathf.Sin (timer * animationSettings.walkAnimSpeed) / 2f));
		} else if (walking == true && GetComponent<Weapon> ().aiming == false && animationSettings.anim.GetBool ("Running") == true) {
			transform.localPosition = Vector3.Lerp (animationSettings.runLeftPos, animationSettings.runRightPos, .5f + (Mathf.Sin (timer * animationSettings.runAnimSpeed) / 2f));
		} else if (walking == true && GetComponent<Weapon> ().aiming == false && animationSettings.anim.GetBool ("Crouching") == true) {
			transform.localPosition = Vector3.Lerp (animationSettings.crouchLeftPos, animationSettings.crouchRightPos, .5f + (Mathf.Sin (timer * animationSettings.crouchAnimSpeed) / 2f));
		}
	}
}