using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerMovementInput : MonoBehaviour {

	const float 			baseSpeed				=			4.0f;
	const float				baseJump				=			400.0f;
	const float				jumpVelFactor			=			0.65f;

	bool 					_isCrouching			=			false;
	bool 					_isSprinting			=			false;
    bool                    _isGrounded             =           true;

    float                   collFrictionCoeff       =           0.6f;
	float					speedFactor				=			1.0f;
	float					speed					=			1.0f;
    float                   playerVertCast, playerHorizCast, 
    playerDepthCast, playerDiagCast;

	Rigidbody				playerBody;
	CapsuleCollider			playerCollider;
    CustomCollision         custCollision;
	Vector3					movementVector;

	public bool 			ToggleCrouch			=			true;
	public bool				ToggleSprint			=			false;

	public bool Sprinting {
		get { return _isSprinting; }
		set {
			if (value) {
				_isSprinting = true;
				speedFactor = 3.0f;
			} else {
				_isSprinting = false;
				speedFactor = 1.0f;
			}
		}
	}

	public bool Crouching {
		get { return _isCrouching; }
		set {
			_isCrouching = value;
			if (value) {
				speedFactor = 0.5f;
			} else if (!value) {
				speedFactor = 1.0f;
			}
		}
	}

    public bool PlayerGrounded {
        get {return _isGrounded; }
        set {
            _isGrounded = value;
        }

    }

	void Start() {
		playerBody = GetComponent<Rigidbody> ();
		playerCollider = GetComponent<CapsuleCollider> ();
        custCollision = new CustomCollision(transform);

		playerVertCast = playerCollider.height / 2.0f + 0.1f;
		playerHorizCast = playerCollider.radius + 0.1f;
        playerDiagCast = 0.2f;
        playerDepthCast = playerCollider.radius + 0.1f;
	}

    void Update()
	{ // Get movement input
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
				if (!Sprinting && PlayerGrounded) {
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
			if (PlayerGrounded) {
                // Calculate the rebound vector of the player's current
                // collision points
                Vector3 jumpRebound = CollisionReboundVector();
				// Create a jump force in the direction normal to the collision
                Vector3 jumpForce = new Vector3(0, baseJump, 0);
				jumpForce += playerBody.velocity * jumpVelFactor + 
                    jumpRebound * collFrictionCoeff;
				// Jump in that direction
				playerBody.AddForce (jumpForce);
			}
		}

		// Assign the directional vector from the input axes and relevant speed factors
		speed = baseSpeed * speedFactor;
		movementVector = new Vector3 (
			Input.GetAxis ("Horizontal") * speed,
			0.0f,
			Input.GetAxis ("Vertical") * speed);
    }

    void FixedUpdate()
	{
		// Move the player relative to the front direction of the playerbody
		if (movementVector.magnitude != 0 && PlayerGrounded) {
			playerBody.velocity = transform.TransformDirection(movementVector);
		}
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10.0f, 10.0f, 200.0f, 50.0f), PlayerGrounded.ToString());
        GUI.Label(new Rect(10.0f, 15.0f, 200.0f, 50.0f), custCollision.NumHits.ToString());

    }

	void PopulatePlayerCollisions() {
		// Perform a raycast in 16 different orientations to determine the current
		// collision points of the player

        custCollision.GenerateHits(transform.position, playerDepthCast,
        playerVertCast, playerHorizCast, playerDiagCast);

        if (custCollision.NumHits > 0)
        {
            PlayerGrounded = true;
        } 
        else {PlayerGrounded = false;}
	}

    Vector3 CollisionReboundVector()
    {
        /* Use the collision hits from the customer collision class to find
        /  the total normal vector from all collision points
        /  This can be used to calculate a vector to use when making the 
        /  player jump off of a surface */
        Vector3 reboundVector;

        reboundVector = custCollision.FindCollisionNormal();

        return reboundVector;
    }

	void OnCollisionEnter(Collision colInfo) {
        // Call to populate collision state of player
		//Debug.DrawRay(transform.position, hit.normal, Color.red, 2.5f);

        PopulatePlayerCollisions();

	}

	void OnCollisionExit(Collision colInfo) {
        // Call ot populate collision state of player
		//Debug.DrawRay (transform.position, hit.normal, Color.blue, 2.5f);
        PopulatePlayerCollisions();

	}


}
