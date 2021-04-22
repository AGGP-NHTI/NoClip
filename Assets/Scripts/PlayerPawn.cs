using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
	public GameObject ProjectileSpawn;

	[Header("Movement")]
	public float moveSpeed = 5; // Movement Speed
	public float verticalSensitivity = 3.0f;
	public float horizontalSensitivity = 200.0f;
	public float jumpHeight = 2f; // Generic Jump Height
	//public CharacterController CC; //This is the CharacterController Component --- NOT A SCRIPT
	public GameObject playerCamera;
	public float gravity = -9.81f;  // The speed in which the pawn will fall
	public float jumpForce = 300.0f;
	public float jumpTime = 0.0f;
	public float jumpTimeMax = 10.0f;
	public float jumpTimeCount = 1.0f;
	Rigidbody rb;
	float minCamClamp = -89.9f;
	float maxCamClamp = 89.9f;
	float camPitch = 0.0f;
	bool forwards = false;
	bool backwards = false;
	bool right = false;
	bool left = false;
	bool isJumping = false;
	Vector2 leftStick = Vector2.zero;
	Vector2 rightStick = Vector2.zero;

	[Header("Wallrun")]
	public LayerMask isWall;
	public float wallRunForce, checkDist;
	bool wallRight, wallLeft, isWallRunning;

	[Header("Health")]
	public AudioSource criticalHealth;
	public AudioSource coreStabile;
	public AudioSource coreCritical;

	[Tooltip("As Percentage of Max Health")]
	[Range(0f, 1f)]
	public float criticalLevel = 0.2f;

	[Tooltip("As Percentage of Max Health")]
	[Range(0f,1f)]
	public float warningLevel = 0.1f;
	bool healthCritical = false;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		//rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.useGravity = true;

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Health -= Time.deltaTime;
		rb.useGravity = false;

		HealthUpdate();

		// Detect if player is on ground
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		GetInput();

		MoveStrafe(leftStick);
		RotateRight(rightStick.x);
		CameraPitch(rightStick.y);

		CheckForWall();
		WallRunInput();

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			isJumping = true;
		}

		if (!isGrounded && !isWallRunning)
		{
			rb.useGravity = true;
			Physics.gravity = new Vector3(0, gravity, 0);
		}
	}

	void FixedUpdate()
	{
		if(isJumping && (jumpTime < jumpTimeMax))
		{
			Jump();
			jumpTime += jumpTimeCount;
		}
		
		if(jumpTime >= jumpTimeMax)
		{
			jumpTime = 0.0f;
			isJumping = false;
		}
	}

	//Old Look/Jump/Movement Code Using CharacterController Component
	/*public override void Look()
	{
		rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
		rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);	//Set max angle for looking up and down
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);	//Rotate Player along X 
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); //Rotate Camera along Y

		if (wallRight && isWallRunning)
		{
			if (Mathf.Abs(wallRunCamTilt) < maxCamTilt)
			{
				wallRunCamTilt += Time.deltaTime * maxCamTilt * 2;
			}
		}

		if (wallLeft && isWallRunning)
		{
			if (Mathf.Abs(wallRunCamTilt) < maxCamTilt)
			{
				wallRunCamTilt -= Time.deltaTime * maxCamTilt * 2;
			}
		}

		if (!isWallRunning && wallRunCamTilt > 0 && !isGrounded)
		{
			wallRunCamTilt = 0.0f;
			playerCamera.transform.localRotation = Quaternion.Euler(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, wallRunCamTilt);
		}

		if (!isWallRunning && wallRunCamTilt < 0 && !isGrounded)
		{
			wallRunCamTilt = 0.0f;
			playerCamera.transform.localRotation = Quaternion.Euler(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, wallRunCamTilt);
		}
	}

	public override void Move(float x, float z)
	{
		//Original Movement
		//rb.velocity = gameObject.transform.right * x * moveSpeed + gameObject.transform.forward * z * moveSpeed;
		
		//For player movement using CharacterController Component
		Vector3 move = gameObject.transform.right * x + gameObject.transform.forward * z;
		CC.Move(move * moveSpeed * Time.deltaTime);

		//For Falling
		if(!isWallRunning)
		{
			velocity.y += gravity * Time.deltaTime;
			//CC.Move(velocity * Time.deltaTime);
		}
		if(isWallRunning)
		{
			velocity.y += wallGrav * Time.deltaTime;
			//CC.Move(velocity * Time.deltaTime);
		}
	}*/

	/*public override void Jump()
	{
		if (isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		/*if (isWallRunning)
		{
			if(wallLeft && !Input.GetKey(KeyCode.D) || (wallRight && !Input.GetKey(KeyCode.A)))
			{
				CC.Move(transform.up * (jumpForce / 5) * Time.deltaTime);
				CC.Move(transform.forward * (jumpForce / 5) * Time.deltaTime);
			}

			if (wallLeft && Input.GetKey(KeyCode.D))
			{
				StopWallRun();
				CC.Move(transform.up * (jumpForce / 5) * Time.deltaTime);
				CC.Move(transform.forward * jumpForce * Time.deltaTime);
			}

			if (wallRight && Input.GetKey(KeyCode.A))
			{
				StopWallRun();
				CC.Move(transform.up * (jumpForce / 5) * Time.deltaTime);
				CC.Move(transform.forward * jumpForce * Time.deltaTime);
			}
		}
	}*/


	void HealthUpdate()
	{
		//Detect when player is at a percentage of their health, if true then play critical health sound
		if (Health < (StartingHealth * warningLevel) && criticalHealth.isPlaying == false)
		{
			criticalHealth.Play();
			healthCritical = true;
		}
		else if (Health > (StartingHealth * warningLevel) && healthCritical == true)
		{
			coreStabile.Play();
			criticalHealth.Stop();
			healthCritical = false;
		}

		if (Health < StartingHealth * criticalLevel && Health > (StartingHealth * criticalLevel) - 0.1)
		{
			coreCritical.Play();
		}

		if (Health < 0)
		{
			criticalHealth.Stop();
			Health = 0;
		}

		if (Health > StartingHealth)
		{
			Health = StartingHealth;
		}
	}

	//New Movement Using Rigidbody
	void GetInput()
	{
		leftStick = Vector2.zero;
		rightStick = Vector2.zero;
		forwards = Input.GetKey(KeyCode.W);
		backwards = Input.GetKey(KeyCode.S);
		left = Input.GetKey(KeyCode.A);
		right = Input.GetKey(KeyCode.D);

		rightStick.x = Input.GetAxis("Mouse X");
		rightStick.y = Input.GetAxis("Mouse Y");
		KeyToAxis();
	}

	void KeyToAxis()
	{
		if (forwards)
		{
			leftStick.y = 1;
		}
		if (backwards)
		{
			leftStick.y = -1;
		}
		if (right)
		{
			leftStick.x = 1;
		}
		if (left)
		{
			leftStick.x = -1;
		}
	}

	void MoveStrafe(Vector2 value)
	{
		MoveStrafe(value.x, value.y);
	}

	void MoveStrafe(float horizontal, float vertical)
	{
		rb.velocity = Vector3.zero;
		rb.velocity += (gameObject.transform.forward * vertical * moveSpeed);
		rb.velocity += (gameObject.transform.right * horizontal * moveSpeed);

		//Will move faster if moving diagonally
	}

	public override void Jump()
	{
		if(!isWallRunning)
		{
			//rb.velocity = new Vector3(0, jumpForce, 0);
			rb.AddForce(Vector3.up * jumpForce * 50);
			rb.AddForce(Vector3.up * jumpForce * 50);
		}
		else
		{
			if ((wallLeft && !Input.GetKey(KeyCode.D)) || (wallRight && !Input.GetKey(KeyCode.A)))
			{
				Debug.Log("normal jump");
				rb.AddForce(Vector2.up * jumpForce * 25);
				rb.AddForce(Vector3.up * jumpForce * 25);
			}

			if (wallRight && Input.GetKey(KeyCode.A))
			{
				Debug.Log("right wall hop");
				rb.AddForce(-transform.right * jumpForce * 250);
				rb.AddForce(transform.forward * jumpForce * 150);
			}

			if (wallLeft && Input.GetKey(KeyCode.D))
			{
				Debug.Log("left wall hop");
				rb.AddForce(transform.right * jumpForce * 250);
				rb.AddForce(transform.forward * jumpForce * 150);
			}

			//Always add forward force
			rb.AddForce(transform.forward * jumpForce * 100);
		}
	}

	void CameraPitch(float value)
    {
        camPitch -= (value * verticalSensitivity);
        camPitch = Mathf.Clamp(camPitch, minCamClamp, maxCamClamp);

        playerCamera.transform.localRotation = Quaternion.Euler(camPitch, 0f, 0f);
        //cameraObj.Rotate(Vector3.up * yRotation);
    }

	void RotateRight(float value)
	{
		gameObject.transform.Rotate(Vector3.up * value * Time.deltaTime * horizontalSensitivity);
	}
	
	private void WallRunInput()
	{
		if(Input.GetKey(KeyCode.D) && wallRight && !isGrounded)
		{
			StartWallRun();
		}

		if (Input.GetKey(KeyCode.A) && wallLeft && !isGrounded)
		{
			StartWallRun();
		}
	}

	private void StartWallRun()
	{
		isWallRunning = true;
		isGrounded = true;

		rb.AddForce(gameObject.transform.forward * wallRunForce);

		if(wallRight)
		{
			rb.AddForce(gameObject.transform.right * wallRunForce / 5 * Time.deltaTime);
		}
		else
		{
			rb.AddForce(-gameObject.transform.right * wallRunForce / 5 * Time.deltaTime);
		}
	}

	private void StopWallRun()
	{
		isWallRunning = false;
	}

	private void CheckForWall()
	{
		wallRight = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.right, checkDist, isWall);
		wallLeft = Physics.Raycast(playerCamera.transform.position, -playerCamera.transform.right, checkDist, isWall);

		if(!wallLeft && !wallRight)
		{
			StopWallRun();
		}
	}

	public override void Fire1(bool value)
	{
	
	}

	public override void Fire2(bool value)
	{

	}

	public override void Fire3(bool value)
	{

	}

	protected override void OnDeath()
	{

	}
}
