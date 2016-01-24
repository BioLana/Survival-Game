using UnityEngine;
using System.Collections;

public class PlayerMovementInput : MonoBehaviour {

	const float 			baseSpeed				=			1.55f;
	const float 			crouchSpeedFactor		=			0.35f;
	const float 			sprintSpeedFactor		=			3.25f;

	bool 					_isCrouching			=			false;
	bool 					_isSprinting			=			false;

	float					speedFactor				=			1.0f;

	Rigidbody				playerBody;
	Vector3					movementVector;

	public bool 			ToggleCrouch			=			true;
	public bool				ToggleSprint			=			false;

	public bool Sprinting {
		get { return _isSprinting; }
		set
		{
			_isSprinting = value;
			if (value) {
				speedFactor = 3.0f;
			} else if (!value) {
				speedFactor = 1.0f;
			}
		}
	}

	public bool Crouching {
		get { return _isCrouching; }
		set
		{
			_isCrouching = value;
			if (value) {
				speedFactor = 0.5f;
			} else if (!value) {
				speedFactor = 1.0f;
			}
		}
	}

	void Start() {
		playerBody = GetComponent<Rigidbody> ();
	}

    void Update()
	{
		// Get movement input
		// Crouch
		if (Input.GetButtonDown ("Crouch")) {
			if (!Sprinting) {
				if (!Crouching) {
					Crouching = true;
				} else if (Crouching && ToggleCrouch) { // Should only apply if Toggle Crouch is on
					Crouching = false;
				}
			}
		}
		if (Input.GetButtonUp ("Crouch")) {
			if (Crouching && !ToggleCrouch) {
				Crouching = false;
			}
		}
		// Sprint
		if (Input.GetButtonDown ("Sprint")) {
			if (!Crouching) {
				if (!Sprinting) {
					Sprinting = true;
				} else if (Sprinting && ToggleSprint) { // Should only apply if Toggle Sprint is on
					Sprinting = false;
				}
			}
		}
		if (Input.GetButtonUp ("Sprint")) {
			if (Sprinting && !ToggleSprint) {
				Sprinting = false;
			}
		}
		// Jump
		if (Input.GetButtonDown ("Jump")) {
			// Check if the player is airborne
			// Need to rethink this and check the player height/collision with whatever the ground at that time is
			if (playerBody.position.y < 1.1) {
				playerBody.AddForce (0, 200, 0);
			}
		}

		// Assign the directional vector from the input axes and relevant speed factors
		movementVector = new Vector3 (
			Input.GetAxis ("Horizontal") * baseSpeed * speedFactor,
			0.0f,
			Input.GetAxis ("Vertical") * baseSpeed * speedFactor);
    }
    
    void FixedUpdate()
	{
		// Move the player towards the front direction of the playerbody
		if (movementVector.magnitude != 0) {
			playerBody.MovePosition (transform.position + (transform.TransformDirection(movementVector) * Time.deltaTime));
		}
    }
}
