using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class Player2controller : MonoBehaviour {
	
	//
	// Animationclips
	//
	
	public AnimationClip[] Clips;
	private Animation animation;
	public string characterstate;
	
	//
	// Parameters for all movement
	//
	
	public bool canControl=true;
	public float ForwardSpeed= 6.0f; // The speed the character walks forwards
	public float BackwardSpeed= 4.0f; // The speed the character walks backwards
	public float inAirAcceleration = 3.0f ;
	public float jumpHeight=4.0f; // How high is the character able to jump
	public float gravity=60.0f; // the gravity for this character
	public float maxFallspeed= 20.0f; //How fast is your character allowed to fall
	// the gravity in a controlled descend mode
	public float speedSmoothing = 10.0f;// How fast does he change speeds, Higher is better
	public float rotateSpeed=10.0f;// How fast the character takes to rotate. Needed when positions change 
	private Vector3 direction= Vector3.zero;
	private float speed=0.0f; // the currect speed
	private float verticalSpeed=0.0f; // the current vertical speed
	private bool isMove= false; //Is our character moving?
	private Vector3 velocity; // velocity when grounded
	private Vector3 inAirVelocity= Vector3.zero; // velocity when in the air 
	private float hangtime = 0.0f; // How long has our character been in the air
	private float h; //reads the input
	
	//
	// Parameters for jumpung
	//
	
	
	public bool canJump; // can we jump
	//This prevents jumping to fast in succesion
	private float JumpRepeatTime= 0.05f;
	private float JumpTimeOut= 0.15f;
	private bool jumping= false; //Are we jumping
	private bool reachedApex=false; // When you are at the apex of your jump
	private float lastButtonTime= -10.0f; // Last time the jump button was clicked down
	private float lastJumpTime= -1.0f; // Last time we jumped we performed a jump
	
	//
	// Other Parameters
	//
	
	private Vector3 pos; // needed for stationing the characters in the z-plane
	static CharacterController controller; 
	public Transform spawnPoint;
	private Vector3 lastpos;// keeps track of where you are
	public Transform Opponent;
	private CollisionFlags collisionFlags;
	
	/// 
	/// For Combo's
	/// 
	private bool punchcombo=false;
	private bool kickcombo=false;
	public int punch=0;
	public int kick=0;
	public bool hurtcombo=false;
	public int hurttimer=0;
	
	/// <summary>
	/// Input variables
	/// </summary
	
	public string horizontalstring;
	public string jumpstring;
	public string punchstring;
	public string kickstring;
	
	void Start () {
	direction=transform.TransformDirection (Vector3.forward);
	controller = (CharacterController)GetComponent(typeof(CharacterController));
	animation = (Animation)GetComponent(typeof(Animation));
	if(!animation)
		Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
	Spawn();
	}
	
	void FixedUpdate()// FixedUpdate is called after a fixed time. 
	{
		pos= new Vector3 (transform.position.x, transform.position.y, 0.0f); // lateupdate!!!
		transform.position=pos;
	}
	
	// Update is called once per frame. 
	void Update () {
		if(controller.isGrounded && characterstate!="punching"& characterstate!="punching2" && characterstate!="kicking"&& characterstate!="kicking2" && characterstate!="hurting")
		{
		characterstate="idle";
		}
	
		if (Input.GetButtonDown(jumpstring) && canControl) // checks if the jumpbutton was pushed and sets the last time that it was pushed to prevent characters from jumping to quickly
		{
		characterstate="jumping";
		lastButtonTime = Time.time;
		}
		
		if(punchcombo)
		{
			punch++;
			if(punch==46 || punch==96 || punch==146)
			{
				punch=0;
				punchcombo=false;
				animation.CrossFade(Clips[0].name);
				characterstate="idle";
			}
		}
		
		if(kickcombo)
		{
			kick++;
			if(kick==46 || kick==96 || kick==146)
			{
				kick=0;
				kickcombo=false;
				animation.CrossFade(Clips[0].name);
				characterstate="idle";
			}
		}
		
		if(hurtcombo)
		{
			hurttimer++;
			if(hurttimer==20)
			{
				hurttimer=0;
				hurtcombo=false;
				animation.CrossFade(Clips[0].name);
				characterstate="idle";
			}
		}
		UpdateSmoothedMovementDirection(); //checks for direction and our speed.
		
		ApplyGravity(); // Apply the gravity logic
		
		if(GetComponent<CharacterController>().isGrounded)
		{
		ApplyJump(); // Apply the jumping logic
		}
		
		//Saves lastposition for velocity calculation
		lastpos= transform.position; 
		
		
		
		//Calculate actual motion
		Vector3 currentMovementOffset= direction*speed + new Vector3 (0.0f,verticalSpeed,0.0f) + inAirVelocity;
		
		// We always want the movement to be framerate independant. this is done by multiplying with time.deltatime.
		currentMovementOffset *= Time.deltaTime;
		
		//Move our character
		collisionFlags= controller.Move(currentMovementOffset);
		
		// Calculate the velocity based on the current and previous position.  
		// This means our velocity will only be the amount the character actually moved as a result of collisions.
		velocity = (transform.position - lastpos) / Time.deltaTime;
		
		// Set rotation to the direction of the other character
		if(Opponent.position.x< transform.position.x)
		{
			transform.rotation= Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left) , Time.deltaTime*rotateSpeed);
		}
		if(Opponent.position.x > transform.position.x)
		{
			transform.rotation= Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right) , Time.deltaTime*rotateSpeed);
		}
		
		// We are in jump mode but just became grounded
		
		if(controller.isGrounded)
		{
			inAirVelocity= Vector3.zero;// Set the velocity to 0 when groundend
			if (jumping)
			{
			jumping=false; // If we're still falling when grounded we shut it down and send a message
			SendMessage("DidLand",SendMessageOptions.DontRequireReceiver);
			Vector3 jumpMoveDirection = direction* speed +inAirVelocity; // Sets the direction we walk when landing
			if(jumpMoveDirection.sqrMagnitude > 0.01f)
				{
					direction= jumpMoveDirection.normalized;
				}
			}
		}
		
		//Animation sector
		if(animation)
		{
			if(characterstate=="jumping")
			{
				animation[Clips[2].name].speed= 2.0f;
				animation.Play(Clips[2].name);
			}
			else
			{
				if(characterstate=="walking")
				{
					animation[Clips[2].name].speed= 1.0f;
					animation.Play(Clips[1].name);
				}
				else{
					
					if(characterstate=="punching")
					{
					animation[Clips[4].name].speed= 0.4f;
					animation.Play(Clips[4].name);
					}
					else
					{
						if(characterstate=="punching2")
						{
						animation[Clips[5].name].speed= 0.4f;
						animation[Clips[5].name].wrapMode= WrapMode.Once;
						animation.Play(Clips[5].name);
						}
						else
						{
								if(characterstate=="kicking")
								{
									animation[Clips[6].name].speed= 0.4f;
									animation[Clips[6].name].wrapMode= WrapMode.Once;
									animation.Play(Clips[6].name);	
								}
								else
								{
									if(characterstate=="kicking2")
									{
										animation[Clips[7].name].speed= 0.4f;
										animation[Clips[7].name].wrapMode= WrapMode.Once;
										animation.Play(Clips[7].name);	
									}
									else
									{
										if(characterstate=="hurting")
										{
										animation[Clips[3].name].speed= 1.0f;
										animation.Play(Clips[3].name);
										}
										else
										{
										animation[Clips[2].name].speed= 1.0f;
										animation.Play(Clips[0].name);
										
									}
								}
							}
						}
					}
				}
			}
		}
	
	}
	
	public void Spawn()
	{
	//Resets the characters Speed
	verticalSpeed=0.0f;
	speed=0.0f;
		
	//Puts the character in the spawnpoint
	transform.position=spawnPoint.position;
	pos=spawnPoint.position;
	}
	
	public void OnDeath() // When the character died so when he lost or won a round but the game is not over yet
	{
	 Spawn();
	}
	
	public void UpdateSmoothedMovementDirection()
	{

		h= Input.GetAxisRaw(horizontalstring); // Reads if we are getting the input

		if(!canControl) //checks if our guy can move (handy for intro's) 
		{
			h=0.0f; 
		}
		
		if(Input.GetButtonDown(punchstring) && canControl)
		{
			h=0.0f;
			if(punchcombo==false)
			{
			characterstate="punching";
			punchcombo=true;
			punch=0;
			}
			
			if(punch<=46 &&punch >=1)
				{
				characterstate="punching2";
				punch=50;
				}
				else
				{
					if(punch<=100 && punch >=47)
					{
					characterstate="punching";
					punch=0;
					}
				}
			}
		
		if(Input.GetButtonDown(kickstring) && canControl)
		{
		h=0.0f;
			if(kickcombo==false)
			{
			characterstate="kicking";
			kickcombo=true;
			kick=0;
			}
			if(kick<=46 &&kick >=1)
				{
				characterstate="kicking2";
				kick=50;
				}
				else
				{
					if(kick<=100 && kick >=47)
					{
					characterstate="kicking";
					kick=0;
					}
				}
		}
		
		isMove= Mathf.Abs((int)h) > 0.1f;
		
		if(isMove) // We set our direction is our character can move. H will be either negative or positive depending on the input
		{
		direction= new Vector3(h,0.0f, 0.0f);
		}
		
		if(controller.isGrounded) // checks if our character is grounded
		{
			
			float cursmooth= speedSmoothing* Time.deltaTime; //Needed for smoothing 
			float targetSpeed= Mathf.Min(Mathf.Abs(h),1.0f); //calculates the speed we want to get for...
			
			if(h<0 && Opponent.position.x< transform.position.x)
			{
				targetSpeed*= ForwardSpeed;// walking forwards when you are standing right
				if(characterstate!="jumping"&& characterstate!="punching"&& characterstate!="punching2" && characterstate!="kicking"&& characterstate!="kicking2"&&characterstate!="hurting")
				characterstate="walking";
			}
			else
			{
				if(h>0 && Opponent.position.x< transform.position.x)
				{
				targetSpeed*=BackwardSpeed;					 // walking backwards when you are standing right
					if(characterstate!="jumping"&& characterstate!="punching"&& characterstate!="punching2" && characterstate!="kicking"&& characterstate!="kicking2" &&characterstate!="hurting")
					characterstate="walking";
				}
				else
				{
					if(h<0 && Opponent.position.x > transform.position.x)
					{
						targetSpeed*=BackwardSpeed;		// walking backwards when you are standing left
						if(characterstate!="jumping"&& characterstate!="punching"&& characterstate!="punching2" && characterstate!="kicking"&& characterstate!="kicking2" &&characterstate!="hurting")
						characterstate="walking";
					}
					else
					{
						if(Opponent.position.x == transform.position.x)
						{ // when the positions are the same when jumping nothing should be changed.
						} 
						else
						{
						targetSpeed*= ForwardSpeed;		// Walking forwards when you are standing left
						if(characterstate!="jumping"&& characterstate!="punching"&& characterstate!="punching2" && characterstate!="kicking"&& characterstate!="kicking2" &&characterstate!="hurting")
						characterstate="walking";
						}
					}
				}
			}
			if(h==0.0f && characterstate!="jumping" && characterstate!="punching"&& characterstate!="punching2" && characterstate!="kicking"&& characterstate!="kicking2" &&characterstate!="hurting")
			{
				characterstate="idle";
			}
			speed= Mathf.Lerp(speed,targetSpeed, cursmooth); // calculates our current speed.
			hangtime=0.0f; // since we are grounded, our hangtime is zero
		}
		else // In air
		{
			hangtime+= Time.deltaTime;			
			if(isMove)
			{
			inAirVelocity+=new Vector3(Mathf.Sign(h),0.0f,0.0f)* Time.deltaTime * inAirAcceleration;
			}
		}
	}
	
	public void ApplyJump()
	{
		if(lastJumpTime + JumpRepeatTime > Time.time) // Prevents jumping to fast after eachother
		{
			return;
		}
		
		if(controller.isGrounded)
		{
			if(canJump && Time.time<lastButtonTime + JumpTimeOut)
			{
				// jumps when pressing the button and with a timeout
				verticalSpeed= CalculateJumpVerticalSpeed(jumpHeight);
				SendMessage("didJump",SendMessageOptions.DontRequireReceiver);

			}
		}
		
	}
	
	public void ApplyGravity() // we're applying the gravity here
	{
		bool jumpButton= Input.GetButton("Jump");// checks if we pushed the button
		
		if(!canControl) // If we cannot control the character, the jumpbutton will never be pushed
		{
			jumpButton=false;
		}
		 
		if( jumping && !reachedApex && verticalSpeed <=0.0f) // When we reach the apex of the jump we send a message
		{
			reachedApex= true;
			SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
		}
		
		if(controller.isGrounded) // sets our verticalspeed when falling
		{
			verticalSpeed= -gravity* Time.deltaTime;
		}
		else
		{
			verticalSpeed -= gravity *Time.deltaTime;
		}
	verticalSpeed = Mathf.Max(verticalSpeed, -maxFallspeed); // Makes sure we don't fall faster then the maxfallspeed
	}
	
	public float CalculateJumpVerticalSpeed( float targetjumpHeight)
	{
		// from the gravity and the jumpheight we deduce the vertical upward speed for the character to reach the apex
		return Mathf.Sqrt(2* targetjumpHeight *gravity);
	}
	
	public void DidJump() // Everything that happens when we are jumping
	{
		jumping=true;
		reachedApex= false;
		lastJumpTime= Time.time;
		lastButtonTime=-10.0f;
	}
	
	public void ControllerColliderHit(ControllerColliderHit hit)
	{
	if (hit.moveDirection.y > 0.01) 
		return;
	}
}
