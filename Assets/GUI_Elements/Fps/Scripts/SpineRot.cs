using UnityEngine;
using System.Collections;

public class SpineRot : MonoBehaviour {

	public float sensitivityZ = 2F;
	
	public float minimumZ = -60F;
	public float maximumZ = 15F;
	
	public float rotationZ = 0F;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		rotationZ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothMouseLook>().rotationY * sensitivityZ;
	}
	void LateUpdate () {
		if (GetComponentInParent<ThirdPerson> ().thirdPerson == true) {
			rotationZ = Mathf.Clamp (rotationZ, minimumZ, maximumZ);
			transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, -rotationZ);
		}
	}
}
