using UnityEngine;
using System.Collections;

public class PlayerRotationInput : MonoBehaviour {

	Rigidbody				playerBody;
	public GameObject		playerHead;

	public Camera			FirstPersonCamera;
	public Camera			ThirdPersonCamera;

	bool					isFirstPerson 				= 		true;

	// Below values aren't used yet - need to set manually
	//const float 			FirstPersonHeadOffset 		= 		0.5f;
	//public const float		ThirdPersonXCamOffset 		= 		0.5f;
	//public const float		ThirdPersonYCamOffset 		= 		0.5f;
	//public const float		ThirdPersonZCamOffset 		= 		0.5f;

	const float				HeadingSensitivity			=		0.5f;
	const float				PitchSensitivity			=		0.5f;
	const float				PitchMax					=		60.0f;
	const float				PitchMin					=		-60.0f;

	float					heading						=		0.0f;
	float					pitch						=		0.0f;

	// Use this for initialization
	void Start () {
		playerBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Pitch player head in the most inefficient way possible
		pitch += Input.GetAxis("LookVertical") * PitchSensitivity;
		pitch = Mathf.Clamp (pitch, PitchMin, PitchMax);
		// Assign local Euler angles because Unity won't let me do anything else...
		playerHead.transform.localEulerAngles = new Vector3(
			-pitch,
			playerHead.transform.localEulerAngles.y,
			playerHead.transform.localEulerAngles.z);

		// Rotate player body
		heading =  Input.GetAxis("LookHorizontal") * HeadingSensitivity;
		playerBody.transform.Rotate (0.0f, heading, 0.0f);

		// Assign camera perspective
		if (Input.GetButtonUp ("ToggleCamera")) {
			isFirstPerson = !isFirstPerson;
			FirstPersonCamera.enabled = isFirstPerson;
			ThirdPersonCamera.enabled = !isFirstPerson;

			playerHead.GetComponent<Renderer> ().enabled = !isFirstPerson;
		}
	}
}
