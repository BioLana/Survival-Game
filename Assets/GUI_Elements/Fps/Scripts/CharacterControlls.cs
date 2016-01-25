using UnityEngine;
using System.Collections;

public class CharacterControlls : MonoBehaviour {
	public KeyCode Run = KeyCode.LeftShift;
	public KeyCode Jump = KeyCode.Space;
	public KeyCode Crouch = KeyCode.C;
	public KeyCode Crawl = KeyCode.Z;
	public bool canRunBackwards = false;
	[HideInInspector]// the reason that theese variables are public but hidden 
	public bool running;//is that they are used in differant scripts but dont
	[HideInInspector]//   really need to be seen in the inspector
	public bool crouching;
	[HideInInspector]
	public bool crawling;
	[HideInInspector]
	public float speed;// this is what speed we are going at it is used later on in the script to play differant animations

	public float walkSpeed = 4.0f;// what speed you will b ehwne you are just pressing w 
	public float runSpeed = 9.0f;//the speed that you will be when you are pressing w and the sprint key
	public float crouchSpeed = 2.0f;// the speed that you will be when you are pressing w and the crouch key
	public float crawlSpeed = 1.0f;//the speed that you will be when you press w and the crawling key. (crawling is not yet fully implemented. i am missng the animations
	public float ClimbSpeed = 2.0f;// the speed that you will be going when you are climbing. and pressing the jumping key.i may set this to a custom key if requested.
	public float SwimSpeed = 4.0f;// the speed you will go when you are pressing the jumping key and in the water. again if needed i will change this to a custom key.
	public float jumpSpeed = 8.0f;// this is the speed that you will go up at when jumping.
	public float gravity = 20.0f;//this is the gravity that the player wll use 20 best resembles earths gravity.
	// If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
	public bool limitDiagonalSpeed = true;

	
	// Units that player can fall before a falling damage function is run. To disable, type "infinity" in the inspector
	public float fallingDamageThreshold = 10.0f;
	
	// If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
	public bool slideWhenOverSlopeLimit = false;
	
	// If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
	public bool slideOnTaggedObjects = false;
	
	public float slideSpeed = 12.0f;
	
	// If checked, then the player can change direction while in the air
	public bool airControl = false;
	
	// Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
	public float antiBumpFactor = .75f;
	
	// Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping
	public int antiBunnyHopFactor = 1;
	
	private Vector3 moveDirection = Vector3.zero;
	[HideInInspector]
	public bool grounded = false;
	private CharacterController controller;
	private Transform myTransform;
	private RaycastHit hit;
	private float fallStartLevel;
	[HideInInspector]
	public bool falling;
	private float slideLimit;
	private float rayDistance;
	private Vector3 contactPoint;
	private bool playerControl = false;
	private int jumpTimer;
	//public Animator anim;							// a reference to the animator on the character
	float CapsuleHeight;
	public float CapsuleHeightCrouch;// how big is the player when hes crouching?
	public float CapsuleHeightCrawl;// how big is the player when hes crouching?
	Vector3 Capsulecenter;
	public Vector3 CapsulecenterCrouch;// sometimes when the player crouches his collider might move down under the floor to counteract this we raise it by 0.5 on the y axis
	public Vector3 CapsulecenterCrawl;// sometimes when the player crouches his collider might move down under the floor to counteract this we raise it by 0.5 on the y axis
	[HideInInspector]
	public bool inWater;
	[HideInInspector]
	public bool onLadder;
	float inputX;
	float tempgrav;
	[HideInInspector]
	public float inputY;
	// this is just an example underwater effect you can change this if you like 
	public MonoBehaviour UnderwaterEffect;
	void Start() {
		tempgrav = gravity;// we use a tempory gravity value so that when we enter the water the gravity changes
		controller = GetComponent<CharacterController>();
		myTransform = transform;//we store the players transform once so that we dont have to keep calling it again and again
		speed = walkSpeed;// setting the default speed to walkspeed otherwise we would just stand still
		rayDistance = controller.height * .5f + controller.radius;//setting the rays distance to half of the player so that it dosent go through the floor
		slideLimit = controller.slopeLimit - .1f;// setting the slide limit slightly lower than the controllers limit
		jumpTimer = antiBunnyHopFactor;// setting up our timer for anti bunnyhop
		CapsuleHeight = controller.height;//getting it once so that we dont have to keep calling it and risking it messing up with crouching
		Capsulecenter = controller.center;
	}
	
	void FixedUpdate() {// we use fixed update so that i never misses an input
		// most of this code is taken from the unity wiki as well as the comments as i didint see the point in rewriting code that already exists
		inputX = Input.GetAxis("Horizontal");//getting the inputs from wasd and the arrow keys that are set up by unity.
		inputY = Input.GetAxis("Vertical");
		// If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
		float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed)? .7071f : 1.0f;
		
		if (grounded) {// we only realy want to be moving and doing other stuff if we are grounded
			bool sliding = false;
			// See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
			// because that interferes with step climbing amongst other annoyances
			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance)) {
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
					sliding = true;
			}
			// However, just raycasting straight down from the center can fail when on steep slopes
			// So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
			else {
				Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
					sliding = true;
			}
			
			// If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
			if (falling) {
				falling = false;
				if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
					FallingDamageAlert (fallStartLevel - myTransform.position.y);
			}

				
				
				
			
			    
			// If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
			if ( (sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide") ) {
				Vector3 hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
				playerControl = false;
			}
			// Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
			else {
				moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
				moveDirection = myTransform.TransformDirection(moveDirection) * speed;
				playerControl = true;
			}
			
			// Jump! But only if the jump button has been released and player has been grounded for a given number of frames
			if (!Input.GetKey(Jump))
				jumpTimer++;
			else if (jumpTimer >= antiBunnyHopFactor) {
				moveDirection.y = jumpSpeed;
				jumpTimer = 0;
			}
		}
		else {
			// If we stepped over a cliff or something, set the height at which we started falling
			if (!falling) {
				falling = true;
				fallStartLevel = myTransform.position.y;
			}
			
			// If air control is allowed, check movement but don't touch the y component
			if (airControl && playerControl) {
				moveDirection.x = inputX * speed * inputModifyFactor;
				moveDirection.z = inputY * speed * inputModifyFactor;
				moveDirection = myTransform.TransformDirection(moveDirection);
			}
		}
		
		// Apply gravity
		moveDirection.y -= gravity * Time.deltaTime;
		
		// Move the controller, and set grounded true or false depending on whether we're standing on something
		grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;


//		anim.SetFloat("Speed", inputY);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
//		anim.SetFloat("Direction", inputX); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
//		if (speed == runSpeed) {
//			anim.SetBool ("Running", true);
//		} else {
//			anim.SetBool ("Running", false);
//		}
//		if (grounded == false) {
//			anim.SetBool ("Falling", true);
//		} else {
//			anim.SetBool ("Falling", false);
//		}
//		if (crouching == true) {
//			anim.SetBool ("Crouching", true);
//		} else {
//			anim.SetBool ("Crouching", false);
//		}
//		if (crawling == true) {
//			anim.SetBool ("Crawling", true);
//		} else {
//			anim.SetBool ("Crawling", false);
//		}
	}
	
	void Update(){// the code here checks to see what we are doing its a bit messy but is easier to read this way
		if (Input.GetKey (Run)) {
			if (canRunBackwards == true) 
			{
				speed = runSpeed;
				running = true;
			}
			if (canRunBackwards == false)
			{
				if (inputY >= 0.1f)
				{
					speed = runSpeed;
					running = true;
				}else{
					speed = walkSpeed;
					running = false;
				}
			}
		} else if (Input.GetKeyUp (Run)) {
			speed = walkSpeed;
			running = false;
		} 
		if (Input.GetKeyDown (Crouch)) {
			crouching = true;
		}else if (Input.GetKeyUp (Crouch)) {
			PreventStandingInLowHeadroom();
		}
		if (Input.GetKeyDown (Crawl)) {
			speed = crawlSpeed;
			crawling = true;
		}else if (Input.GetKeyUp (Crawl)) {
			PreventStandingInLowHeadroom();
		}
		if (crouching == true) {
			controller.height = CapsuleHeightCrouch;
			controller.center = CapsulecenterCrouch;
			speed = crouchSpeed;
		} else if (crouching == false) {
			controller.height = CapsuleHeight;
			controller.center = Capsulecenter;
		}
		if (crawling == true) {
			controller.height = CapsuleHeightCrawl;
			controller.center = CapsulecenterCrawl;
			speed = crouchSpeed;
		} else if (crouching == false) {
			controller.height = CapsuleHeight;
			controller.center = Capsulecenter;
		}

		if (inWater == true) {
			gravity = 5;
			speed = SwimSpeed;
			falling = false;
			UnderwaterEffect.enabled = true;
			if (Input.GetKey(Jump))
			{
				moveDirection.y = SwimSpeed;
			}
			if (Input.GetKey(Crouch))
			{
				moveDirection.y = -SwimSpeed;
			}
		}  
		if (inWater == false){
			gravity = tempgrav;
			UnderwaterEffect.enabled = false;
		}
		if (onLadder == true) {
			falling = false;
			if (Input.GetKey(Jump))
			{
				moveDirection.y = ClimbSpeed;
			}
			if (Input.GetKey(Crouch))
			{
				moveDirection.y = -ClimbSpeed;
			}
		}  
	}

	void PreventStandingInLowHeadroom()// this function here raycasts up from the character to make sure if we are able to stand or not
	{
		if (crouching == true || crawling ==true)
		{
			var startPos = transform.position + new Vector3(0, CapsuleHeightCrouch - (CapsuleHeight * 0.5f),0);
			var length = (CapsuleHeight - CapsuleHeightCrouch);
			if (!Physics.Raycast(startPos ,Vector3.up,length))
			{
				crouching = false;
				crawling = false;
				speed = walkSpeed;
			}
		}
	}
	// Store point that we're in contact with for use in FixedUpdate if needed
	void OnControllerColliderHit (ControllerColliderHit hit) {
		contactPoint = hit.point;
	}
	void FallingDamageAlert (float fallDistance) {
		GetComponent<PlayerStats> ().health -= fallDistance / 50;// i divide it by 50 here as the full value when falling is far to large chnage this if you want
		print ("Ouch! Fell " + fallDistance + " units!");
	}
}
